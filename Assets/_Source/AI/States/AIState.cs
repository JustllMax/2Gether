using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeightType
{
    Lowest,
    Low,
    Medium,
    High,
    Highest
}


public abstract class AIState : ScriptableObject
{
    public WeightType weight;
    public abstract void OnStart(AIController controller);
    public abstract void OnExit(AIController controller);
    public abstract void OnTick(AIController controller);
    public abstract void OnLateUpdate(AIController controller);
    public abstract bool CanChangeToState(AIController controller);
    public abstract bool CanExitState(AIController controller);
    public abstract void OnTargetChanged(AIController controller);
}