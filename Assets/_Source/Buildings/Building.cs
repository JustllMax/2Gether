using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, ITargetable, IDamagable
{


    private const int UPGRADE_COUNTER_LIMIT = 2;
    private const int LEVEL_LIMIT = 3;

    [SerializeField] protected float DestroyObjectDelay;
    [SerializeField] protected Animator animator;
    [SerializeField] private BuildingUpgradePath _upgradeTiers;
    [Header("Audio")]
    [SerializeField] protected AudioClip createDestroySound; // Played when building is created
    [SerializeField] protected AudioClip upgradeSound; // Played when building is upgraded
    [SerializeField] protected AudioClip activationSound; // Played when attacking / activating
    [SerializeField] protected AudioClip takeHitSound; // Played when receiving damage
    [SerializeField] protected AudioSource audioSource;
    [Header("Particles")] 
    [SerializeField] protected ParticleSystem createDestroyParticles;
    [SerializeField] protected ParticleSystem upgradeParticles;

    private BuildingStatistics _buildingStatistics;
    private float attackTimer = 0;
    private int currentLevel = 1;
    private int upgradeCounter = 0;
    protected float AttackCoolDownTimer = 5f;
    [SerializeField] protected LayerMask targetLayerMask;
    [SerializeField] protected LayerMask obstructionMask;
    [HideInInspector] public bool IsTargetable { get; set; }
    [HideInInspector] public TargetType TargetType { get; set; }
    [HideInInspector] public float Health { get; set; }

    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        IsTargetable = true;
        TargetType = TargetType.Building;
        _buildingStatistics = _upgradeTiers.GetStatsForLevel(currentLevel);
        Health = _buildingStatistics.HealthPoints;

    }

    public virtual void Start()
    {

    }



    public abstract bool TakeDamage(float damage);
    public abstract void Kill();




    #region ChildrenMethods
    public abstract void OnCreate();
    public abstract void OnAttack();
    public virtual void OnSell()
    {
        GoldManager.Instance.GoldAdd(GetSellCost());
    }

    public bool CanAttack()
    {
        return (attackTimer >= _buildingStatistics.ActivationTime);
    }
    #endregion ChildrenMethods

    #region Upgrade

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

    #endregion Upgrade



    #region GetSet
    public BuildingStatistics GetBaseStatistics()
    {
        return _upgradeTiers.GetStatsForLevel(currentLevel);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;  
    }

    public int GetSellCost()
    {
        return _buildingStatistics.SellCost + _buildingStatistics.SellCost * upgradeCounter / 2;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public AudioClip GetUpgradeSFX()
    {
        return upgradeSound;
    }

    public AudioClip GetCreateDestroySoundSFX()
    {
        return createDestroySound;
    }

    #endregion  GetSet

}