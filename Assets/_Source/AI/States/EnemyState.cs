using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateWeight
{
    Low,
    Medium,
    High,
    Highest
}


public abstract class EnemyState : ScriptableObject
{
    StateWeight weight;
    public abstract void Enter(AIController controller);
    public abstract void Exit(AIController controller);
    public abstract void OnUpdate(AIController controller);
    public abstract void OnFixedUpdate(AIController controller);
    public abstract bool CanChangeToState(AIController controller);
}