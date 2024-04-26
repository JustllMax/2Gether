using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class AIController : MonoBehaviour
{
    [Header("Enemy Statistics")]
    [SerializeField] EnemyStatistics stats;

    [Header("AI States")]
    [SerializeField] List<AIState> _AIStates;
    AIState previousState;
    AIState nextState;
    AIState currentState;


    [Header("Audio")]
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip deathSound;


    Animator _animator;
    NavMeshAgent _navMeshAgent;
    Vector3 lastPosition;
    bool isStunned = false;

 
    Transform currentTarget;
    public float distanceToTarget;
    float attackTimer = 0f;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        currentState = _AIStates[0];
        currentState.OnStart(this);
    }


    public void Update()
    {
        if(attackTimer < stats.AttackCooldown)
        {
            attackTimer += Time.deltaTime;

        }
        if (currentState != null)
        {
            currentState.OnUpdate(this);
            ChangeState();
        }
    }

    public void ChangeState()
    {
        nextState = GetNextState();

        if (nextState == null) { return; }
        if (currentState != null && !currentState.CanChangeToState(this))
        {
            return;
        }
        if (nextState == currentState)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.OnExit(this);
        }

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

    public void AttackPerformed()
    {
        attackTimer = 0f;
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
    public bool CanAttack()
    {
        if (currentTarget != null)
        {
            if(currentTarget.GetComponent<ITargetable>().IsTargetable && attackTimer >= stats.AttackCooldown)
            {
                return true;
            }
        }
        return false;
    }


    public void SetCurrentTarget(Transform target)
    {
        currentTarget = target;
    }

    public Transform GetCurrentTarget()
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
