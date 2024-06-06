using UnityEngine;

public class Healing : Building
{
  
    private string playerTag = "Player";
    private string buildingTag = "Building";

    private GameObject PlayerObject;
    private GameObject BuildingObject;
    public ParticleSystem particles;


    public override void Start()
    {
        base.Start();
     
    }

    public void Update()
    {
       
    }



    #region ChildrenMethods

    public override void OnCreate()
    {

    }

    public override void OnAttack()
    {
        if (particles != null)
        {
            particles.Play();
            Debug.Log("++++");
        }
        else
        {
            Debug.Log("particle system not detected");
        }
    }


    public override bool TakeDamage(float damage)
    {
        audioSource.PlayOneShot(takeHitSound);
        Health -= damage;
        if (Health <= 0)
        {
            Kill();
            return true;
        }
        return false;
    }

    public override void Kill()
    {
        IsTargetable = false;
        audioSource.PlayOneShot(createDestroySound);
        createDestroyParticles.Play();
        Invoke("DestroyObj", DestroyObjectDelay);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    public override void OnSell()
    {
        base.OnSell();
        Kill();
    }

    #endregion ChildrenMethods
    public void OnTakeDamage()
    {
        //todo 
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, rangeBuilding);
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, rangePlayer);
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

}