using System.Diagnostics;
using UnityEngine;

public class Healing : Building
{
  
    private float rangePlayer = 10f;
    public int price = 200;

    private string playerTag = "Player";
    private string buildingTag = "Building";

    private GameObject PlayerObject;
    private GameObject BuildingObject;



    void Update()
    {
       AttackCooldown();
    }


    public override void OnCreate()
    {

    }

    public override void OnAttack()
    {
        if(AttackCoolDownTimer < 0 )
        {
            //
        }
    }

    public override void OnTakeDamage()
    {
        //todo 
    }

    public override void OnUpgrade()
    {

    }

    public override void OnSell()
    {
        GoldManager.Instance.GoldAdd(price);
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, rangeBuilding);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangePlayer);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(playerTag))
        {
            UnityEngine.Debug.Log("player entered zone");
            OnAttack();

        }
        if (collision.CompareTag(buildingTag))
        {
            UnityEngine.Debug.Log("building in the zone");

        }
    }

    private void OnTriggerStay(Collider other)
    {
            OnAttack();

    }
}