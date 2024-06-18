using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Minefield : Building
{
    [SerializeField] List<Mine> mines;

    public override void Awake()
    {
        base.Awake();
        IsTargetable = false;
        mines = new List<Mine>();
        mines = GetComponentsInChildren<Mine>().ToList();
    }



    #region ChildrenMethods

    public override void OnAttack()
    {
        Debug.Log("Test");
        int counter = 0;
        foreach(var mine in mines)
        {
            if(mine.enabled == false)
            {
                counter++;
                
            }
        }
        Debug.Log("disabled mines " + counter);
        if(counter == mines.Count)
        {
            Kill();
        }
    }

    public override void OnUpgrage()
    {
        base.OnUpgrage();
        foreach(var mine in mines)
        {
            mine.SetMineUp();
        }
    }
    public override bool TakeDamage(float damage)
    {
        return false;
    }


    #endregion ChildrenMethods

    public BuildingOffensiveStatistics GetStatistics()
    {
        return GetBaseStatistics() as BuildingOffensiveStatistics;
        
    }

    public LayerMask GetTargetLayerMask()
    {
        return targetLayerMask;
    }


}
