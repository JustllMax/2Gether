using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Transform target;
    private float speed = 4f;
    private float animationTime = 0f;

    public void SeekTarget(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 direction = target.position - transform.position;
        float distance = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distance, Space.World);

        /*

        animationTime += Time.deltaTime;
        animationTime = animationTime % 5f; 

        Vector3 direction = MathParabola.Parabola(transform.position, target.position, 5f, animationTime / 5f);

        float distance = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distance, Space.World);
        */


    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit Ground");
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit Enemy");
            Destroy(gameObject);
        }
    }
}
