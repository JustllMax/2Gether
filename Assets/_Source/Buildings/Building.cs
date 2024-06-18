using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, ITargetable, IDamagable
{


    private const int UPGRADE_COUNTER_LIMIT = 2;
    private const int LEVEL_LIMIT = 3;

    [SerializeField] protected float DestroyObjectDelay;
    
    [SerializeField] private BuildingUpgradePath _upgradeTiers;
    [Header("Audio")]
    [SerializeField] protected AudioClip createDestroySound; // Played when building is created
    [SerializeField] protected AudioClip upgradeSound; // Played when building is upgraded
    [SerializeField] protected AudioClip activationSound; // Played when attacking / activating
    [SerializeField] protected AudioClip takeHitSound; // Played when receiving damage
    protected AudioSource audioSource;
    [Header("Particles")]
    [SerializeField] float particlesScaleModifier = 1;

    private BuildingStatistics _buildingStatistics;
    private int currentLevel = 0;
    private int upgradeCounter = 0;
    protected float AttackCoolDownTimer = 5f;
    [SerializeField] protected LayerMask targetLayerMask;
    [SerializeField] protected LayerMask obstructionMask;
    [HideInInspector] public bool IsTargetable { get; set; }
    [HideInInspector] public TargetType TargetType { get; set; }

    protected float maxHealth;
    [HideInInspector] public float Health { get; set; }
    protected Animator animator;
    public virtual void Awake()
    {

        
        if(GetComponentInChildren<Animator>() != null)
            animator = GetComponentInChildren<Animator>();

        IsTargetable = true;
        TargetType = TargetType.Building;
        _buildingStatistics = _upgradeTiers.GetStatsForLevel(currentLevel);
        maxHealth = _buildingStatistics.HealthPoints;
        Health = maxHealth;
        audioSource = GetComponent<AudioSource>();

    }

    public virtual void Start()
    {

    }



    public abstract bool TakeDamage(float damage);

    public virtual void Kill()
    {
        IsTargetable = false;
        AudioManager.Instance.PlaySFX(createDestroySound);
        DustSpawner.SpawnDust(transform.position, particlesScaleModifier);
        Invoke("DestroyObj", DestroyObjectDelay);
    }

    public bool Heal(float amount)
    {
        maxHealth = GetBaseStatistics().HealthPoints;
        Health += amount;
        Health = Mathf.Clamp(Health, 0f, maxHealth);
        return true;
    }



    #region ChildrenMethods
    public void OnCreate()
    {
        AudioManager.Instance.PlaySFX(createDestroySound);
        DustSpawner.SpawnDust(transform.position, particlesScaleModifier);
        
    }
    public abstract void OnAttack();
    public virtual void OnSell()
    {
        AudioManager.Instance.PlaySFX(createDestroySound);
        GoldManager.Instance.GoldAdd(GetSellCost());
        DustSpawner.SpawnDust(transform.position, particlesScaleModifier);

    }

    public virtual void OnUpgrage()
    {
        maxHealth = GetBaseStatistics().HealthPoints;
        Health = maxHealth;
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
            Upgrade();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Upgrade() 
    {
        AudioManager.Instance.PlaySFX(upgradeSound);
        upgradeCounter++;

        if(upgradeCounter == UPGRADE_COUNTER_LIMIT)
        {
            UpgradeParticlesSpawner.SpawnParticles(transform.position, 2f);
            currentLevel++;
            _buildingStatistics = _upgradeTiers.GetStatsForLevel(currentLevel);
            upgradeCounter = 0;
            OnUpgrage();
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
        return GetBaseStatistics().SellCost;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public AudioClip GetActivationSFX()
    {
        return activationSound;
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


    void DestroyObj()
    {
        Destroy(gameObject);
    }
}