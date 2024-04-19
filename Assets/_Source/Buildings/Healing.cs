using UnityEngine;

public class Healing : Building
{
  
    private float rangePlayer = 5f;
    public int price = 200;

    private string playerTag = "Player";
    private string buildingTag = "Building";

    private GameObject PlayerObject;
    private GameObject BuildingObject;
    public ParticleSystem particles;


    void Start()
    {
  
        

    }

    void Update()
    {
       
    }


    public override void OnCreate()
    {

    }

    public override void OnAttack()
    {
        if (particles != null)
        {
            particles.Play();
            
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
            Debug.Log("player entered zone");
            OnAttack();

        }
        if (collision.CompareTag(buildingTag))
        {
            Debug.Log("building in the zone");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player left zone");
            if (particles != null)
                particles.Stop();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag(playerTag))
            OnAttack();
    }
}