using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float pushForce = 10f;

    private Vector3 targetPos;
    private bool isPushed = false;

    private void Start()
    {
       
        
    }

    private void Update()
    {
        if (!isPushed)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            transform.Translate(direction * pushForce * Time.deltaTime);


            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isPushed = true;
            }
        }
        else
        {
 
            
        }
    }

    public void SetTargetPosition(Vector3 pos)
    {

        if (!isPushed)
        {
            targetPos = pos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("ground");
            Destroy(gameObject);
        }

    }
}
