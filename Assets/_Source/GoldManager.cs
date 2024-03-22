using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    private int GoldCount = 0;
    private int FixGain = 100;

    private float MainBaseIncome = 0.2f;
    private float ComboIncome = 50f;
    private float DeathIncome = 5f;


    public static GoldManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void GoldAdd(int income)
    {
        GoldCount +=  income;
    }

    public bool GoldSpend(int value)
    {
        var tempG = GoldCount;
        if(tempG-value < 0)
        {
            return false;
        }
        GoldCount -= value;
        return true;
    }

    public void StartOfDayIncome(float mainBaseHp, int combo, int deaths)
    {
        GoldCount += (int)Mathf.Round(FixGain + (mainBaseHp * MainBaseIncome) + (combo * ComboIncome) - (deaths * DeathIncome));
    }

    public void SetFixGain(int fixGain)
    {
        this.FixGain = fixGain;
    }

   


}
