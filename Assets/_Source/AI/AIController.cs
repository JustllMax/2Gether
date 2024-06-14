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


[Serializable]
public struct AITarget
{
    public Transform transform;
    public ITargetable targetable;
    public Vector3 positionOffset;

    public AITarget(Transform transform, ITargetable targetable, Vector3 positionOffset)
    {
        this.transform = transform;
        this.targetable = targetable;
        this.positionOffset = positionOffset;
    }

    public AITarget(Transform transform, ITargetable targetable)
    {
        this.transform = transform;
        this.targetable = targetable;
        this.positionOffset = Vector3.zero;
    }

    public Vector3 Position => transform.position + positionOffset;
};

public class AIController : MonoBehaviour, IDamagable
{
    [Header("Enemy Statistics")]
    [SerializeField] EnemyStatistics stats;

    [Header("AI States")]
    [SerializeField] List<AIState> _AIStates;
    AIState previousState;
    AIState nextState;
    [Foldout("DEBUG INFO")]
    [SerializeField] protected AIState currentState;

    [Header("Audio")]
    [SerializeField] protected AudioClip hurtSound;
    public AudioClip attackSound;
    [SerializeField] protected AudioClip deathSound;
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] protected float DeathInvokeTime = 2f;
    DisintegrationEffect _deathEffect;
    Animator _animator;
    protected NavMeshAgent _navMeshAgent;
    protected bool isStunned = false;
    protected bool isDead = false;
    protected float attackTimer = 0f;

    [Foldout("DEBUG INFO")]
    [SerializeField] private AITarget currentTarget = new AITarget();

    [Foldout("DEBUG INFO")]
    [SerializeField] protected Collider[] hitboxColliders;

    private static LayerMask[] targetMasks;

    [Foldout("DEBUG INFO")]
    public bool isWalking;

    [Foldout("DEBUG INFO")]
    public float distanceToTarget;

    [Foldout("DEBUG INFO")]
    public float lastAttackTime = 0f;

    [Foldout("DEBUG INFO")]
    public uint ammoCount = 0;

    [Foldout("DEBUG INFO")]
    public bool isReloading;

    [Foldout("DEBUG INFO")]
    public bool canSwitchTarget;

    [Foldout("DEBUG INFO")]
    [SerializeField] private float _health;

    private float _rotationSpeed;

    public float Health { get => _health; set => _health = value; }
    public bool IsAlive { get => !isDead; }

    private void Awake()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        _deathEffect = GetComponent<DisintegrationEffect>();
        _animator = GetComponentInChildren<Animator>();

        hitboxColliders = GetComponentsInChildren<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        Health = stats.Health;
        ApplyDefaultMovement();

        canSwitchTarget = stats.PrimaryTarget != stats.SecondaryTarget;

        if (targetMasks == null)
        {
            targetMasks = new LayerMask[4];
            targetMasks[0] = LayerMask.GetMask("Building");
            targetMasks[1] = LayerMask.GetMask("Player");
            targetMasks[2] = LayerMask.GetMask("MainBuilding");
            targetMasks[3] = LayerMask.GetMask("Building", "MainBuilding");
        }
    }

    private void Start()
    {
        SetNavMeshAgentType(stats.PrimaryTarget);
        currentState = _AIStates[0];
        currentState.OnStart(this);
    }

    void SetNavMeshAgentType(TargetType focus)
    {
        string agentTypeName = "";
        if(focus == TargetType.Player)
        {
            agentTypeName = NavAgentTypeNames.PlayerChase.ToString();
        }
        else
        {
            agentTypeName = NavAgentTypeNames.BuildingChase.ToString();
        }
        int? agentType = GetNavMeshAgentID(agentTypeName);
        if(agentType != null)
        {
            _navMeshAgent.agentTypeID = (int)agentType;
        }
    }

    private int? GetNavMeshAgentID(string name)
    {
        for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
            if (name == NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
            {
                return settings.agentTypeID;
            }
        }
        return null;
    }
    public void Update()
    {
        if (isDead)
        {
            return;
        }

        lastAttackTime += Time.deltaTime;


        if (!HasTarget())
        {
            SearchForTarget();
        }

        if (HasTarget())
        {
            distanceToTarget = Vector3.Distance(currentTarget.Position, transform.position);

            //Try to switch target
            if (canSwitchTarget)
            {
                SwitchTarget();
            }
        }

        if (currentState != null)
        {
            ChangeState();
            currentState.OnUpdate(this);
        }

        if (_rotationSpeed != 0)
        {
            Vector3 lookrotation = currentTarget.transform.position - transform.position;
            lookrotation.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), _rotationSpeed * Time.deltaTime);
        }

        _animator.SetFloat("walk_speed", _navMeshAgent.velocity.magnitude);
    }

    public void LateUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.OnLateUpdate(this);
        }
    }

    private void SwitchTarget()
    {
        //Try to switch if current target is not searched for in secondary mask
        if ((targetMasks[(int)currentTarget.targetable.TargetType].value & targetMasks[(int)stats.SecondaryTarget].value) == 0)
        {
            AITarget secondaryTarget = GetClosestTarget(stats.SwitchRange, targetMasks[(int)stats.SecondaryTarget]);
            if (secondaryTarget.transform != null)
            {
                SetCurrentTarget(secondaryTarget);
            }
        }
        //Remove target if it is out of range
        else if (distanceToTarget > stats.SwitchRange)
        {
            SetCurrentTarget(new AITarget());
        }
    }

    public bool HasTarget()
    {
        //W Unity nie mo¿na robiæ (currentTarget.targetable != null && currentTarget.targetable) bo jak jest null to wywali b³¹d
        if (currentTarget.transform != null && currentTarget.targetable != null)
        {
            if (currentTarget.targetable.IsTargetable)
            {
                return true;
            }
        }

        return false;
    }

    private void SearchForTarget()
    {
        AITarget target = GetClosestTarget(stats.SearchRange, targetMasks[(int)stats.PrimaryTarget]);
        if (target.transform != null)
        {
            SetCurrentTarget(target);
        }
    }

    private AITarget GetClosestTarget(float range, LayerMask layer)
    {
        Transform target = null;
        ITargetable targetable = null;
        Vector3 closestPoint = Vector3.zero;
        float minDistance = float.MaxValue;

        //Check for targets in range
        Collider[] hits = Physics.OverlapSphere(transform.position, range, layer);
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
                if (distanceToTarget < range && distanceToTarget < minDistance)
                {
                    target = newTransform;
                    targetable = newTarget;
                    closestPoint = newClosestPoint;
                    minDistance = distanceToTarget;
                }
            }
        }

        if (target != null)
        {
            //Project hitpoint onto navmesh surface
            closestPoint = ProjectToNavSurface(closestPoint) - target.position;
        }


        return new AITarget(target, targetable, closestPoint);
    }

    public Vector3 ProjectToNavSurface(in Vector3 point, float maxDistance = 2.5f)
    {
        if (NavMesh.SamplePosition(point, out NavMeshHit navhit, maxDistance, _navMeshAgent.areaMask))
        {
            return navhit.position;
        }
        return point;
    }

    public void ChangeState()
    {
        nextState = GetNextState();

        if (nextState == null) { return; }

        if (nextState == currentState)
        {
            return;
        }

        if (currentState != null && !currentState.CanExitState(this))
        {
            return;
        }

        currentState.OnExit(this);

        if (nextState != null && nextState != currentState)
        {
            previousState = currentState;
            nextState.OnStart(this);
            currentState = nextState;

        }
    }


    private AIState GetNextState()
    {
        List<AIState> states = new List<AIState>();
        StateWeight highestWeight = StateWeight.Lowest;

        foreach (var state in _AIStates)
        {
            if (state.CanChangeToState(this))
            {
                if (state.weight > highestWeight)
                {
                    highestWeight = state.weight;
 
                }
            }
        }

        foreach (var state in _AIStates)
        {
            if (state.CanChangeToState(this) && state.weight == highestWeight)
            {
                states.Add(state);
                //Debug.Log(state.name);
            }
        }

        if (states.Count > 0)
        {
            int randomIndex = Random.Range(0, states.Count);
            return states[randomIndex];
        }

        return null;


    }

    public bool CanAttack()
    {
        return !isDead && HasTarget();
    }

    public virtual bool TakeDamage(float damage)
    {
        if(isDead)
            return false;

        Health -= damage;
        AudioManager.Instance.PlaySFXAtSource(hurtSound, audioSource);
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
        Health = Mathf.Clamp(Health, 0f, stats.Health);
        return true;
    }

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

    public void SetMovementStats(in EnemyMovement enemyMovement)
    {
        _navMeshAgent.speed = enemyMovement.MovementSpeed;
        _navMeshAgent.angularSpeed = enemyMovement.TurnSpeed;
        _navMeshAgent.acceleration = enemyMovement.Acceleration;
        _rotationSpeed = enemyMovement.ExtraRotationSpeed;
    }

    public void ApplyDefaultMovement()
    {
        SetMovementStats(stats.Movement);
    }

    public void RefreshTargetPos()
    {
        if (HasTarget())
            _navMeshAgent.SetDestination(currentTarget.Position);
        else 
            _navMeshAgent.ResetPath();
    }

    public bool ShouldChangePath()
    {
        return !_navMeshAgent.hasPath || !_navMeshAgent.pathPending && (_navMeshAgent.remainingDistance <= 1f || _navMeshAgent.isPathStale);
    }

    public GameObject InstantiateGameObject(GameObject obj, Transform parent)
    {
        return Instantiate(obj, parent);
    }
    #region GetSet

    public bool IsDead()
    {
        return isDead;
    }

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

    public void SetCurrentTarget(AITarget target)
    {
        currentTarget = target;
        RefreshTargetPos();
    }

    public AITarget GetCurrentTarget()
    {
        return currentTarget;
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



}
