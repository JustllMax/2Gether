using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;


public class AIController : MonoBehaviour
{
    AIState previousState;
    AIState nextState;
    AIState currentState;

    [SerializeField] List<AIState> _AIStates;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip deathSound;

    private EnemyStatistics stats;

    Vector3 lastPosition;
    Transform currentTarget;

    private void Start()
    {
        currentState = _AIStates[0];
        currentState.OnStart(this);
    }


    public void Update()
    {
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
        StateWeight highestWeight = StateWeight.Low;

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
    
    #region GetSet
    public EnemyStatistics GetEnemyStats()
    {
        return stats;
    }

    #endregion GetSet



}
