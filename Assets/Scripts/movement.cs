using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 2f;
    private float strafeDistance = 4f; 
    private float changeDirectionTime = 2f; 
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
   
        timer += Time.deltaTime;

 
        if (timer >= changeDirectionTime)
        {
            timer = 0f;
            speed *= -1f; 
        }

        float targetZ = Mathf.PingPong(Time.time * speed, strafeDistance * 2) - strafeDistance;

        rb.velocity = new Vector3(0f, 0f, targetZ);
    }
}
