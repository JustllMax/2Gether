using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using UnityEngine.InputSystem.XR;
using UnityEditor;


[Serializable]
public struct AITarget
{
    public Transform transform;
    public ITargetable targetable;

    
    public AITarget(Transform transform, ITargetable targetable)
    {
        this.transform = transform;
        this.targetable = targetable;
    }
};

public class AIController : MonoBehaviour, IDamagable
{
    [Header("Enemy Statistics")]
    [SerializeField] EnemyStatistics stats;

    [Header("AI States")]
    [SerializeField] List<AIState> _AIStates;
    AIState previousState;
    AIState nextState;
    protected AIState currentState;

    [Header("CHANGE ACCORDINGLY TO CHASE/BUILDING. BOTH IS FOR BOSSES")]
    [SerializeField] TargetType AITargetFocus;

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
    protected LayerMask targetLayerMask;
    protected float attackTimer = 0f;

    [Foldout("DEBUG INFO")]
    [SerializeField] private AITarget currentTarget = new AITarget();

    [Foldout("DEBUG INFO")]
    [SerializeField] protected Collider[] hitboxColliders;



    [Foldout("DEBUG INFO")]
    public float distanceToTarget;

    [Foldout("DEBUG INFO")]
    public float lastAttackTime = 0f;

    [Foldout("DEBUG INFO")]
    public uint ammoCount = 0;

    [Foldout("DEBUG INFO")]
    public bool isReloading;

    [Foldout("DEBUG INFO")]
    public Vector3 wanderTarget;


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
    }

    private void Start()
    {

        SetLayerTargeting(AITargetFocus);
        SetNavMeshAgentType(AITargetFocus);
        currentState = _AIStates[0];
        currentState.OnStart(this);
    }

    void SetNavMeshAgentType(TargetType focus)
    {
        string agentTypeName = "";
        if(focus.ToString() == TargetType.Player.ToString())
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
            _navMeshAgent.ResetPath();
            SearchForTarget();
        }

        if (currentTarget.transform != null)
        {
             distanceToTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
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

    void SetLayerTargeting(TargetType targetType)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int buildingLayer = LayerMask.NameToLayer("Building");
        LayerMask mask;

        switch (targetType)
        {
            case TargetType.Building:
                mask = (1 << buildingLayer);
                targetLayerMask = mask;
                return;

            case TargetType.Player:
                mask = (1 << playerLayer);
                targetLayerMask = mask;
                return;

            case TargetType.Both:
                mask = (1 << playerLayer) | (1 << buildingLayer);
                targetLayerMask = mask;
                return;

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
        Transform target = null;
        ITargetable targetable = null;

        float minDistance = float.MaxValue;
        Collider[] hits = Physics.OverlapSphere(GetCurrentPosition(), stats.SearchRange, targetLayerMask);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out ITargetable t))
            {
                if (t.IsTargetable)
                {
                    float distanceToTarget = Vector3.Distance(hit.transform.position, transform.position);
                    if (distanceToTarget < minDistance)
                    {
                        target = hit.transform;
                        targetable = t;
                        minDistance = distanceToTarget;
                    }
                }
            }
        }
        if(target != null)
            SetCurrentTarget(new AITarget(target, targetable));
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

    public void RangedAttackPerformed()
    {
        attackTimer = 0f;
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
            _navMeshAgent.SetDestination(currentTarget.transform.position);
        else 
            _navMeshAgent.ResetPath();
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
