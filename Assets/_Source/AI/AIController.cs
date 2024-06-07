using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using UnityEngine.InputSystem.XR;


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
    AIState currentState;

    [Header("CHANGE ACCORDINGLY TO CHASE/BUILDING. BOTH IS FOR BOSSES")]
    [SerializeField] TargetType AITargetFocus;

    [Header("Audio")]
    [SerializeField] AudioClip hurtSound;
    public AudioClip attackSound;
    [SerializeField] AudioClip deathSound;
    [HideInInspector] public AudioSource audioSource;
    [SerializeField] protected Collider hitboxCollider;
    [SerializeField] protected float DeathInvokeTime = 2f;
    DisintegrationEffect _deathEffect;
    Animator _animator;
    NavMeshAgent _navMeshAgent;
    protected bool isStunned = false;
    protected bool isDead = false;
    protected LayerMask targetLayerMask;
    protected float attackTimer = 0f;

    [Foldout("DEBUG INFO")]
    [SerializeField] private AITarget currentTarget = new AITarget();

    [Foldout("DEBUG INFO")]
    public float distanceToTarget;

    [Foldout("DEBUG INFO")]
    public float lastAttackTime = 0f;


    [Foldout("DEBUG INFO")]
    public bool isReloading;

    [Foldout("DEBUG INFO")]
    public float comboLength;
    [Foldout("DEBUG INFO")]
    [SerializeField] private float _health;

    public float Health { get => _health; set => _health = value; }
    public bool IsAlive { get => !isDead; }

    public float maxHealth;
    private void Awake()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        _deathEffect = GetComponent<DisintegrationEffect>();
        _animator = GetComponentInChildren<Animator>();

        hitboxCollider = GetComponentInChildren<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        comboLength = GetEnemyStats().attackCombo.Length;
        maxHealth = GetEnemyStats().Health;
        Health = maxHealth;
        _navMeshAgent.speed = GetEnemyStats().MovementSpeed;
        Debug.Log("agent type id " + _navMeshAgent.agentTypeID);
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

        if (lastAttackTime < stats.ComboDelay)
        {
            lastAttackTime += Time.deltaTime;

        }


        if (ShouldSearchForTarget())
        {
            SearchForTarget();
        }

        if (currentTarget.transform != null)
        {
             distanceToTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
        }

        if (currentState != null)
        {
            currentState.OnUpdate(this);
            ChangeState();
        }

        _animator.SetFloat("walk_speed", _navMeshAgent.velocity.magnitude / stats.MovementSpeed);
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

    private bool ShouldSearchForTarget()
    {
        if (currentTarget.transform != null)
        {
            if (currentTarget.targetable != null)
            {
                if(currentTarget.targetable.IsTargetable)
                    return false;
            }
        }
        return true;
    }

    private void SearchForTarget()
    {
        Transform target = null;
        ITargetable targetable = null;


        float radius = GetEnemyStats().AttackRange*3f;

        float minDistance = float.MaxValue;


        Collider[] hits = Physics.OverlapSphere(GetCurrentPosition(), radius, targetLayerMask);
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

        // Find the highest state weight
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

        // Collect all states with the highest weight
        foreach (var state in _AIStates)
        {
            if (state.CanChangeToState(this) && state.weight == highestWeight)
            {
                states.Add(state);
                Debug.Log(state.name);
            }
        }

        // Choose a random state from the collected states
        if (states.Count > 0)
        {
            int randomIndex = Random.Range(0, states.Count);
            return states[randomIndex];
        }

        return null; // Return null if no state is found


    }

    public void RangedAttackPerformed()
    {
        attackTimer = 0f;
        comboLength--;
    }
    public bool CanAttack()
    {
        if (currentTarget.transform != null)
        {
            if (currentTarget.targetable != null && currentTarget.targetable.IsTargetable && comboLength > 0)
            {
                 return true;
            }
        }
        return false;
    }

    public bool TakeDamage(float damage)
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
        hitboxCollider.enabled = false;
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
        Health = Mathf.Clamp(Health, 0f, maxHealth);
        return true;
    }

    public bool AnimationComplete(string animName)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        return !stateInfo.IsName(animName) || (stateInfo.normalizedTime >= 1 && !_animator.IsInTransition(0));
    }

    public void PlayAnimation(string animName, float crossTime = 0.1f)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);


        if (!stateInfo.IsName(animName))
        {
            Debug.LogError("test1");
            _animator.CrossFadeInFixedTime(animName, crossTime);
        } else if (stateInfo.normalizedTime >= 1 && !_animator.IsInTransition(0))
        {
            Debug.LogError("test2");
            _animator.Play(animName, -1, 0);

        }
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
