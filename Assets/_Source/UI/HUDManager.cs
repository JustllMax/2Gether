using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;
using NaughtyAttributes;
public class HUDManager : MonoBehaviour
{

    private static HUDManager _instance;
    public static HUDManager Instance { get { return _instance; } }


    [SerializeField]List<Sprite> Icons;

    Dictionary<GunType, Sprite> IconsDictionary;

    [Header("Character")]
    [Foldout("References")][SerializeField] Slider HealthBar;
    [Foldout("References")] [SerializeField] TMP_Text HealthCurrentText;
    [Foldout("References")][SerializeField] List<Slider> Dashes;

    [Header("Gun")]
    [Foldout("References")] [SerializeField] Slider AmmoBar;
    [Foldout("References")] [SerializeField] TMP_Text AmmoCurrentText;
    [Foldout("References")] [SerializeField] TMP_Text AmmoMaxText;
    [Foldout("References")] [SerializeField] Image IconImage;

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

        IconsDictionary = new Dictionary<GunType, Sprite>();
        GunType t = 0;
        foreach(var icon in Icons)
        {
            for (int i = 0; i < 4; i++ ){
                t = (GunType)i;
                if (icon.name.ToLower().Contains(t.ToString().ToLower()))
                {
                    IconsDictionary.Add(t, icon);
                }
            }
        }
        
    }

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

    public void SetMaxHealth(float maxHealth)
    {
        HealthBar.maxValue = maxHealth;
    }
    public void SetCurrentHealth(float currentHealth)
    {
        currentHealth = Mathf.Clamp(currentHealth, HealthBar.minValue, HealthBar.maxValue);

        HealthCurrentText.text = currentHealth.ToString();
        HealthBar.value = currentHealth;
    }

    public void SetIcon(GunType type)
    {
        IconImage.sprite = IconsDictionary[type];  
    }

    public void SetMaxAmmo(float maxAmmo)
    {
        AmmoMaxText.text = maxAmmo.ToString();
        AmmoBar.maxValue = maxAmmo;
    }
    public void SetCurrentAmmo(float currentAmmo)
    {
        currentAmmo = Mathf.Clamp(currentAmmo, AmmoBar.minValue, AmmoBar.maxValue);

        AmmoCurrentText.text = currentAmmo.ToString();
        AmmoBar.value = currentAmmo;
    }

    public void SwitchGunOnHUD(float currentAmmo, float maxAmmo, GunType type)
    {
        SetMaxAmmo(maxAmmo);
        SetCurrentAmmo(currentAmmo);
        SetIcon(type);
    }
}
