using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using NaughtyAttributes;


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

    [SerializeField] TargetType AITargetFocus;

    [Header("Audio")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] BoxCollider hitboxCollider;
    [SerializeField] float DeathInvokeTime = 2f;
    DisintegrationEffect _deathEffect;
    Animator _animator;
    NavMeshAgent _navMeshAgent;
    bool isStunned = false;
    bool isDead = false;
    private float attackTimer = 0f;

    [Foldout("DEBUG INFO")]
    [SerializeField] private AITarget currentTarget = new AITarget();

    [Foldout("DEBUG INFO")]
    public float distanceToTarget;

    [Foldout("DEBUG INFO")]
    public float lastAttackTime = 0f;


    [Foldout("DEBUG INFO")]
    public bool isReloading;

    [Foldout("DEBUG INFO")]
    public float remainingAttacks;

    private float _health;
    public float Health { get => _health; set => _health = value; }

    private void Awake()
    {
        _deathEffect = GetComponent<DisintegrationEffect>();
        _animator = GetComponentInChildren<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        remainingAttacks = GetEnemyStats().AttackAmount;
        Health = GetEnemyStats().Health;
        _navMeshAgent.speed = GetEnemyStats().MovementSpeed;
    }

    private void Start()
    {
        currentState = _AIStates[0];
        currentState.OnStart(this);
    }


    public void Update()
    {

        if (isDead)
        {
            return;
        }

        if (lastAttackTime < stats.AttackFireRate)
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


    } 

    private bool ShouldSearchForTarget()
    {
        if (GetCurrentTarget().transform  != null)
        {
            if (GetCurrentTarget().targetable != null)
            {
                if(GetCurrentTarget().targetable.IsTargetable)
                    return false;
            }
        }
        return true;
    }

    private void SearchForTarget()
    {
        Transform target = null;
        ITargetable targetable = null;
        //Layermask that hits everything except the terrain
        int layerMask = ~(1 << LayerMask.NameToLayer("Terrain"));
        float radius = GetEnemyStats().AttackRange*3f;

        float minDistance = float.MaxValue;


        Collider[] hits = Physics.OverlapSphere(GetCurrentPosition(), radius, layerMask);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out ITargetable t))
            {
                if (t.IsTargetable && ShouldTarget(t))
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

    bool ShouldTarget(ITargetable t)
    {
        if (t.TargetType == AITargetFocus || AITargetFocus == TargetType.Both) {
            return true;
        }
        return false;
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
        remainingAttacks--;
    }
    public bool CanAttack()
    {
        if (currentTarget.transform != null)
        {
            if (currentTarget.targetable.IsTargetable && remainingAttacks > 0)
            {
                 return true;
            }
        }
        return false;
    }

    public bool TakeDamage(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Kill();
            return true;
        }
        return false;
    }

    public void Kill()
    {
        isDead = true;
        hitboxCollider.enabled = false;
        GetNavMeshAgent().enabled = false;
        if (GetAnimator().GetNextAnimatorStateInfo(0).IsName(AIAnimNames.DEATH.ToString()))
        {
            GetAnimator().CrossFade(AIAnimNames.DEATH.ToString(), 0.1f);
        }
        _deathEffect.Execute();
        Invoke("DestroyObj", DeathInvokeTime);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }


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
