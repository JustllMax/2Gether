using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    private const int UPGRADE_COUNTER_LIMIT = 2;
    private const int LEVEL_LIMIT = 3;

    [SerializeField] private BuildingUpgradePath _upgradeTiers;
    
    private BuildingStatistics _buildingStatistics;
    private float attackTimer = 0;
    private int currentLevel = 1;
    private int upgradeCounter = 0;

    //[Header("Audio")]
    AudioClip buildSound; // Played when building is created
    AudioClip upgradeSound; // Played when player upgrades the building
    AudioClip activationSound; // Played when attacking / activating


    public virtual void Start()
    {
        _buildingStatistics = _upgradeTiers.GetStatsForLevel(currentLevel);
    }

    public virtual void Update()
    {
        /*
        if(attackTimer < _buildingStatistics.ActivationTime) 
        {
            attackTimer += Time.deltaTime;
        }
        */
    }

    protected float AttackCoolDownTimer = 5f;

    public virtual void AttackCooldown()
    {
        AttackCoolDownTimer -= Time.deltaTime;
    }

    public abstract void OnCreate();
    public abstract void OnAttack();

    public abstract void OnTakeDamage();


    public bool TryUpgrading(CardStatistics cardStats)
    {
        if(GetCurrentLevel() == LEVEL_LIMIT)
        {
            return false;
        }

        if(cardStats == _buildingStatistics)
        {
            OnUpgrade();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnUpgrade() 
    {
        //Play upgrade clip
        upgradeCounter++;

        if(upgradeCounter == UPGRADE_COUNTER_LIMIT)
        {
            currentLevel++;
            _buildingStatistics = _upgradeTiers.GetStatsForLevel(currentLevel);
            upgradeCounter = 0;
        }     
    }
    public void OnSell()
    {
        GoldManager.Instance.GoldAdd(GetSellCost());
    }

    public bool CanAttack()
    {
        return (attackTimer >= _buildingStatistics.ActivationTime);
    }

    public BuildingStatistics GetStats()
    {
        return _upgradeTiers.GetStatsForLevel(currentLevel);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;  
    }

    public int GetSellCost()
    {
        return _buildingStatistics.BaseSellCost + _buildingStatistics.BaseSellCost * upgradeCounter / 2;
    }


}