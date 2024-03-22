using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;


public class AIController : MonoBehaviour
{
    public List<EnemyState> states;
    private EnemyStats stats;

    public EnemyState CurrentState { get; private set; }
    public void SetInitialState(List<EnemyState>states) 
    { 
        CurrentState = states[0];
        CurrentState.Enter(this);
    }


    //EnemyState TryChangingState()
    //{

    //}
   
    

    public void ChangeState(EnemyState states)
    {
        if (CurrentState == states || states == null)
        {
            return;
        }
        CurrentState.Exit(this);
        CurrentState = states;
        CurrentState.Enter(this);

    }
    public void Update()
    {
        CurrentState.OnUpdate(this);
    }
    public void FixedUpdate()
    {
        CurrentState.OnFixedUpdate(this);
    }

    public EnemyStats GetEnemyStats()
    {
        return stats;
    }



}
