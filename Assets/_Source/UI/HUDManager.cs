using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Cysharp.Threading.Tasks;

public class HUDManager : MonoBehaviour
{

    #region Variables
    private static HUDManager _instance;
    public static HUDManager Instance { get { return _instance; } }

    [SerializeField] float HUDAppearanceDelay = 2f;
    [SerializeField]List<Sprite> Icons;
    [SerializeField]List<Sprite> Crosshairs;

    Dictionary<GunType, Sprite> IconsDictionary;
    Dictionary<GunType, Sprite> CrosshairsDictionary;
    
    [Foldout("References")][SerializeField] GameObject NightUI;

    [Header("Character")]
    [Foldout("References")][SerializeField] Slider HealthBar;
    [Foldout("References")] [SerializeField] TMP_Text HealthCurrentText;
    [Foldout("References")][SerializeField] List<Slider> Dashes;

    [Header("Gun")]
    [Foldout("References")][SerializeField] Image CrosshairImage;
    [Foldout("References")] [SerializeField] Slider AmmoBar;
    [Foldout("References")] [SerializeField] TMP_Text AmmoCurrentText;
    [Foldout("References")] [SerializeField] TMP_Text AmmoReserveText;
    [Foldout("References")] [SerializeField] Image IconImage;
    #endregion Variables
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        SetupIconsDictionary();
        SetupCrosshairsDictionary();

    }

    private void OnEnable()
    {
        DayNightCycleManager.NightBegin += OnNightStart;
        DayNightCycleManager.NightEnd += OnNightEnd;
    }

    private void OnDisable()
    {
        DayNightCycleManager.NightBegin -= OnNightStart;
        DayNightCycleManager.NightEnd -= OnNightEnd;
    }

    void OnNightStart()
    {
        _ = SetNightUIActivation(true);
    }

    void OnNightEnd()
    {
        _ = SetNightUIActivation(false);
    }

    async UniTaskVoid SetNightUIActivation(bool enabled)
    {
        if(enabled)
            await UniTask.WaitForSeconds(HUDAppearanceDelay);
        NightUI.SetActive(enabled);
    }

    #region Setup

    void SetupIconsDictionary()
    {
        if (Icons == null || Icons.Count == 0)
            return;

        IconsDictionary = new Dictionary<GunType, Sprite>();
        GunType t = 0;
        foreach (var icon in Icons)
        {
            for (int i = 0; i < 4; i++)
            {
                t = (GunType)i;
                if (icon.name.ToLower().Contains(t.ToString().ToLower()))
                {
                    IconsDictionary.Add(t, icon);
                }
            }
        }

        foreach (var item in IconsDictionary)
        {
            Debug.Log("Dictionary: " + item.Value);
        }
    }

    void SetupCrosshairsDictionary()
    {
        if (Crosshairs == null || Crosshairs.Count == 0)
            return;


        CrosshairsDictionary = new Dictionary<GunType, Sprite>();
        GunType t = 0;
        foreach (var crosshair in Crosshairs)
        {
            for (int i = 0; i < 4; i++)
            {
                t = (GunType)i;
                if (crosshair.name.ToLower().Contains(t.ToString().ToLower()))
                {
                    IconsDictionary.Add(t, crosshair);
                }
            }
        }
    }

    #endregion Setup




    #region Dash

    public void SetAllDashesMaxTimer(float maxTimer)
    {
        foreach(var dash in  Dashes)
        {
            dash.maxValue = maxTimer;
        }
    }

    public void SetDashCurrentTimer(int dashIndex, float timerValue)
    {

        timerValue = Mathf.Clamp(timerValue, Dashes[dashIndex].minValue, Dashes[dashIndex].maxValue);

        for(int i = dashIndex+1; i<Dashes.Count; i++)
        {
            Dashes[i].value = Dashes[i].minValue;
        }
        Dashes[dashIndex].value = timerValue;
    }

    #endregion Dash

    #region Health

    public void SetMaxHealth(float maxHealth)
    {
        HealthBar.maxValue = maxHealth;
    }
    public void SetCurrentHealth(float currentHealth)
    {
        currentHealth = Mathf.Clamp(currentHealth, HealthBar.minValue, HealthBar.maxValue);
        HealthBar.value = currentHealth;
        HealthCurrentText.text = HealthBar.value.ToString();
    }

    public void SetMaxAmmo(int maxAmmo)
    {
        AmmoBar.maxValue = maxAmmo;
    }

    #endregion Health

    #region Gun
    public void SetIcon(GunType type)
    {
        if (IconsDictionary != null)
            IconImage.sprite = IconsDictionary[type];
    }

    public void SetCrosshair(GunType type)
    {
        if (CrosshairsDictionary != null)
            CrosshairImage.sprite = CrosshairsDictionary[type];
    }
    public void SetCurrentAmmo(int currentAmmo)
    {
        currentAmmo = Mathf.Clamp(currentAmmo, (int)AmmoBar.minValue, (int)AmmoBar.maxValue);

        AmmoCurrentText.text = currentAmmo.ToString();
        AmmoBar.value = currentAmmo;
    }


    public void SetCurrentAmmo(int currentAmmo, int reserveAmmo)
    {
        currentAmmo = Mathf.Clamp(currentAmmo, (int)AmmoBar.minValue, (int)AmmoBar.maxValue);
        AmmoReserveText.text = reserveAmmo.ToString();
        AmmoCurrentText.text = currentAmmo.ToString();
        AmmoBar.value = currentAmmo;
    }

    public void SwitchGunOnHUD(int currentAmmoMag, int maxAmmoMag, int reserveAmmo, GunType type)
    {
        SetMaxAmmo(maxAmmoMag);
        SetCurrentAmmo(currentAmmoMag, reserveAmmo);
        SetIcon(type);
        SetCrosshair(type);
    }

    #endregion Gun
}
