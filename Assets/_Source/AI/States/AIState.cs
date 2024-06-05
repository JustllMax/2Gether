using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateWeight
{
    Lowest,
    Low,
    Medium,
    High,
    Highest,
    AfterPlayerDeath
}


public abstract class AIState : ScriptableObject
{
    public StateWeight weight;
    public AIAnimNames animName;
    public float AnimDelay;
    public abstract void OnStart(AIController controller);
    public abstract void OnExit(AIController controller);
    public abstract void OnUpdate(AIController controller);
    public abstract bool CanChangeToState(AIController controller);
}