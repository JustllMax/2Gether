using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using UnityEngine.InputSystem.XR;
using UnityEditor;
using UnityEngine.UIElements;


#region AITarget
[Serializable]
public class AITarget
{
    public TargetProperties properties;

    public Transform transform;
    public ITargetable targetable;
    public Vector3 positionOffset;

    public AITarget(in TargetProperties properties, Transform transform, ITargetable targetable, in Vector3 positionOffset)
    {
        this.properties = properties;
        this.transform = transform;
        this.targetable = targetable;
        this.positionOffset = positionOffset;
    }

    public bool IsValid()
    {
        if (transform != null && targetable != null)
        {
            if (targetable.IsTargetable)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 Position => transform.position + positionOffset;
};
#endregion

public class AIController : MonoBehaviour, IDamagable
{
    [Header("Enemy Statistics")]
    [SerializeField] EnemyStatistics stats;

    [Header("Audio")]
    public AudioClip hurtSound;
    public AudioClip attackImmuneSound;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioSource audioSource;

    [SerializeField] protected float DeathInvokeTime = 2f;
    DisintegrationEffect _deathEffect;
    Animator _animator;
    protected NavMeshAgent _navMeshAgent;
    protected bool isStunned = false;
    protected bool isDead = false;
    protected float attackTimer = 0f;
    protected float _rotationSpeed;

    private LinkedListNode<AIController> _node;

    #region Debug info
    [Foldout("DEBUG INFO")]
    [SerializeField] private float _health;

    [Foldout("DEBUG INFO")]
    [SerializeField] protected AIState currentState;

    [Foldout("DEBUG INFO")]
    [SerializeField] private AITarget _currentTarget;

    [Foldout("DEBUG INFO")]
    public bool canChangeTarget;

    [Foldout("DEBUG INFO")]
    public float distanceToTarget;

    [Foldout("DEBUG INFO")]
    [SerializeField] private bool _usesPathNavmesh;

    [Foldout("DEBUG INFO")]
    public bool isWalking;

    [Foldout("DEBUG INFO")]
    public float lastAttackTime = 0f;

    [Foldout("DEBUG INFO")]
    public uint ammoCount = 0;

    [Foldout("DEBUG INFO")]
    public bool isShooting;

    [Foldout("DEBUG INFO")]
    public bool isReloading;

    [Foldout("DEBUG INFO")]
    [SerializeField] protected Collider[] hitboxColliders;

    [Foldout("DEBUG INFO")]
    public double lastTickTime = 0f;

    [Foldout("DEBUG INFO")]
    public float tickDeltaTime = 0f;

    #endregion

    #region Initialization
    private void Awake()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        _deathEffect = GetComponent<DisintegrationEffect>();
        _animator = GetComponentInChildren<Animator>();

        hitboxColliders = GetComponentsInChildren<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _currentTarget = null;
        canChangeTarget = true;
        distanceToTarget = float.MaxValue;
        Health = stats.health;
        ApplyDefaultMovement();
    }

    private void Start()
    {
        _node = AIManager.Instance.RegisterEnemy(this);
        WalksOnPath = stats.walksOnPath;
    }

    #endregion
    private void OnDestroy()
    {
        AIManager.Instance.UnregisterEnemy(_node);
    }


    public void OnTick(double time)
    {
        tickDeltaTime = (float)(time - lastTickTime);
        lastTickTime = time;

        if (isDead)
        {
            return;
        }

        lastAttackTime += tickDeltaTime;

        TryChangeTarget();

        TryChangeState();
        if (currentState != null)
        {
            currentState.OnTick(this);
        }
    }

    public void LateUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (_rotationSpeed != 0)
        {
            Vector3 lookrotation = _currentTarget.transform.position - transform.position;
            lookrotation.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }

        _animator.SetFloat("walk_speed", _navMeshAgent.velocity.magnitude);

        if (currentState != null)
        {
            currentState.OnLateUpdate(this);
        }
    }

    #region Target Handling
    public AITarget CurrentTarget
    {
        get => _currentTarget;
        set
        {
            _currentTarget = value;

            if (_currentTarget != null)
            {
                distanceToTarget = Vector3.Distance(_currentTarget.Position, transform.position);
                SetMovementStats(_currentTarget.properties.movement);
                if (!_currentTarget.properties.walkOnPath)
                    WalksOnPath = false;
            }
            else
            {
                distanceToTarget = float.MaxValue;
                ApplyDefaultMovement();
                if (!stats.walksOnPath)
                    WalksOnPath = false;
            }
                

            if (currentState != null)
            {
                currentState.OnTargetChanged(this);
            }
        }
    }

    public void TryChangeTarget()
    {
        bool targetLost = false;

        if (HasTarget())
        {
            distanceToTarget = Vector3.Distance(_currentTarget.Position, transform.position);

            if (!canChangeTarget)
                return;

            //Lose target if out of range
            if (distanceToTarget > _currentTarget.properties.loseTargetRange)
            {
                targetLost = true;
                _currentTarget = null;
            } 
            else if (!_currentTarget.properties.canAbandonTarget) //In range and can't abandon, skip
            {
                return;
            }
        }

        if (!canChangeTarget)
            return;

        AITarget nextTarget = GetNextTarget();

        if (nextTarget == null)
        {
            if (targetLost)
                CurrentTarget = null;

            return;
        }

        CurrentTarget = nextTarget;
    }


    private AITarget GetNextTarget()
    {
        WeightType targetWeight = WeightType.Lowest;
        List<AITarget> possibleTargets = new List<AITarget>();

        foreach (var targetStats in stats.targetProperties)
        {
            if (targetStats.weight < targetWeight)
                break;

            //Skip if current target has been chosen using the same properties
            if (_currentTarget != null && _currentTarget.properties == targetStats)
            {
                targetWeight = targetStats.weight;
                continue;
            }


            AITarget newTarget = GetClosestTarget(targetStats);

            if (newTarget != null)
            {
                targetWeight = targetStats.weight;
                possibleTargets.Add(newTarget);
            }
        }

        //Random if same weight
        if (possibleTargets.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleTargets.Count);
            return possibleTargets[randomIndex];
        }

        return null;
    }

    public bool HasTarget()
    {
        return _currentTarget != null && _currentTarget.IsValid();
    }

    private AITarget GetClosestTarget(in TargetProperties properties)
    {
        Transform target = null;
        ITargetable targetable = null;
        Vector3 closestPoint = Vector3.zero;
        float minDistance = float.MaxValue;

        //Check for targets in range
        Collider[] hits = Physics.OverlapSphere(transform.position, properties.maxSearchRange, AIManager.Instance.GetLayerFromType(properties.targetType));
        foreach (Collider hit in hits)
        {
            ITargetable newTarget;
            Transform newTransform;

            if (hit.attachedRigidbody != null)
            {
                hit.attachedRigidbody.TryGetComponent<ITargetable>(out newTarget);
                newTransform = hit.attachedRigidbody.transform;
            } else
            {
                hit.TryGetComponent<ITargetable>(out newTarget);
                newTransform = hit.transform;
            }

            if (newTarget != null && newTarget.IsTargetable)
            {
                Vector3 newClosestPoint = hit.ClosestPointOnBounds(transform.position);
                float distanceToTarget = Vector3.Distance(newClosestPoint, transform.position);
                if (distanceToTarget < properties.maxSearchRange && distanceToTarget < minDistance)
                {
                    target = newTransform;
                    targetable = newTarget;
                    closestPoint = newClosestPoint;
                    minDistance = distanceToTarget;
                }
            }
        }

        //Project hitpoint onto navmesh surface
        if (target != null && AIManager.Instance.SampleNavSurface(closestPoint, 2.5f, stats.agentType, properties.walkOnPath, out var surfacePoint))
        {
            closestPoint = surfacePoint - target.position;
            return new AITarget(properties, target, targetable, closestPoint);
        }

        return null;
    }

    public bool SampleNavSurface(in Vector3 point, out Vector3 pointOnSurface, float maxDistance = 2.5f)
    {
        if (NavMesh.SamplePosition(point, out NavMeshHit navhit, maxDistance, _navMeshAgent.areaMask))
        {
            pointOnSurface = navhit.position;
            return true;
        }

        pointOnSurface = Vector3.zero;
        return false;
    }
    #endregion

    #region States

    private void TryChangeState()
    {
        if (currentState != null && !currentState.CanExitState(this))
            return;

        AIState nextState = GetNextState();

        if (nextState == null || nextState == currentState)
            return;

        if (currentState != null)
            currentState.OnExit(this);
        nextState.OnStart(this);
        currentState = nextState;
    }


    private AIState GetNextState()
    {
        WeightType stateWeight = WeightType.Lowest;
        List<AIState> possibleStates = new List<AIState>();

        foreach (var state in stats.AIStates)
        {
            if (state.CanChangeToState(this))
            {
                if (state.weight < stateWeight)
                    break;
                else
                {
                    stateWeight = state.weight;
                    possibleStates.Add(state);
                }
            }
        }

        //Random if same weight
        if (possibleStates.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleStates.Count);
            return possibleStates[randomIndex];
        }

        return null;
    }

    #endregion

    public bool CanAttack()
    {
        return !isDead && HasTarget();
    }

    #region Damage

    public float Health { get => _health; set => _health = value; }
    public bool IsAlive { get => !isDead; }
    public virtual bool TakeDamage(float damage)
    {
        if(isDead)
            return false;

        Health -= damage;
        if (damage > 0)
            AudioManager.Instance.PlaySFXAtSource(hurtSound, audioSource);
        else 
            AudioManager.Instance.PlaySFXAtSource(attackImmuneSound, audioSource);

        if(Health <= 0)
        {
            AudioManager.Instance.PlaySFXAtSource(deathSound, audioSource);
            Kill();
            return true;
        }
        return false;
    }

    public virtual void Kill()
    {
        isDead = true;
        foreach (Collider col in hitboxColliders)
        {
            col.enabled = false;
        }

        if (currentState != null)
            currentState.OnExit(this);

        GetNavMeshAgent().enabled = false;

        PlayAnimation("DEATH");
        Invoke("Desintegrate", DeathInvokeTime);

        WaveManager.Instance.waveSystem.enemyCount--;
    }
    
    void Desintegrate()
    {
        _deathEffect.Execute();
    }

    public bool Heal(float amount)
    {

        Health += amount;
        Health = Mathf.Clamp(Health, 0f, stats.health);
        return true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    #endregion

    public void PlaySound(AudioClip clip)
    {
        AudioManager.Instance.PlaySFXAtSource(clip, audioSource);
    }

    #region Animation Control
    public bool AnimationComplete(string animName)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        return !stateInfo.IsName(animName) || (stateInfo.normalizedTime >= 1 && !_animator.IsInTransition(0));
    }

    public bool AllAnimationsComplete()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        return (stateInfo.normalizedTime >= 1 && !_animator.IsInTransition(0));
    }

    public void PlayAnimation(string animName, float crossTime = 0.1f)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);


        if (!stateInfo.IsName(animName))
        {
            _animator.CrossFadeInFixedTime(animName, crossTime);
        } else if (stateInfo.normalizedTime >= 1 && !_animator.IsInTransition(0))
        {
            _animator.Play(animName, -1, 0);
        }
    }

    #endregion

    #region Movement
    public bool WalksOnPath 
    { 
        get => _usesPathNavmesh;
        set
        {
            _usesPathNavmesh = value;
            int id = AIManager.Instance.GetAgentIDFromType(stats.agentType, value);
            if (_navMeshAgent.agentTypeID != id)
            {
                _navMeshAgent.agentTypeID = id;
            }
        }
    }


    public void SetMovementStats(in EnemyMovement enemyMovement)
    {
        _navMeshAgent.speed = enemyMovement.movementSpeed;
        _navMeshAgent.angularSpeed = enemyMovement.turnSpeed;
        _navMeshAgent.acceleration = enemyMovement.acceleration;
        _rotationSpeed = enemyMovement.extraRotationSpeed;
    }

    public void ApplyDefaultMovement()
    {
        SetMovementStats(stats.defaultMovement);
    }

    public void ApplyTargetMovement()
    {
        if (HasTarget())
            SetMovementStats(_currentTarget.properties.movement);
        else
            SetMovementStats(stats.defaultMovement);
    }

    public void RefreshTargetPos()
    {
        if (!_navMeshAgent.isOnNavMesh)
            return;

        if (HasTarget())
        {
            _navMeshAgent.SetDestination(_currentTarget.Position);
        }
        else 
            _navMeshAgent.ResetPath();
    }

    public bool ShouldChangePath()
    {
        return !_navMeshAgent.hasPath || !_navMeshAgent.pathPending && (_navMeshAgent.remainingDistance <= 1f || _navMeshAgent.isPathStale);
    }

    #endregion

    #region GetSet

    public EnemyStatistics GetEnemyStats()
    {
        return stats;
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public bool IsStunned()
    {
        return isStunned;
    }
    public void SetStun(bool val)
    {
        isStunned = val;
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }
    public NavMeshAgent GetNavMeshAgent()
    {
        return _navMeshAgent;
    }



    #endregion GetSet


    public GameObject InstantiateGameObject(GameObject obj, Transform parent)
    {
        return Instantiate(obj, parent);
    }
}
