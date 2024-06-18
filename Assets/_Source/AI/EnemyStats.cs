using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[CreateAssetMenu(fileName = "AIStats_", menuName = "2Gether/AI/Data/Statistics")]
public class EnemyStatistics : ScriptableObject
{
    public float health;

    [Tooltip("Movement of the enemy when it doesn't have a target")]
    public EnemyMovement defaultMovement;

    public TargetProperties[] targetProperties;

    [Tooltip("Agent type defines the size of the agent")]
    public NavAgentType agentType;

    [Tooltip("Defines if the enemy walks on paths when it doesn't have a target")]
    public bool walksOnPath;

    [SerializeField]
    public AIState[] AIStates;



    private void OnValidate()
    {
        if (targetProperties != null)
        {
            System.Array.Sort(targetProperties, (x, y) => y.weight.CompareTo(x.weight));

            for (int i = 0; i < targetProperties.Length; i++)
            {
                if (targetProperties[i].loseTargetRange < targetProperties[i].maxSearchRange)
                    targetProperties[i].loseTargetRange = targetProperties[i].maxSearchRange;
            }
        }

        if (AIStates != null)
            System.Array.Sort(AIStates, (x, y) => y.weight.CompareTo(x.weight));
    }
}
