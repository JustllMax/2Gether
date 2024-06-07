using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float moveDistance = 5f; // Distance to move in each direction

    private Rigidbody rb;
    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = rb.position;
    }

    void FixedUpdate()
    {
        // Calculate the direction based on the current movement
        Vector3 direction = movingRight ? Vector3.right : Vector3.left;

        // Calculate the target position based on the current direction
        Vector3 targetPos = startPos + direction * moveDistance;

        // Move towards the target position
        Vector3 movement = direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // If the object has reached the target position
        if ((movingRight && rb.position.x >= targetPos.x) || (!movingRight && rb.position.x <= targetPos.x))
        {
            // Change direction
            movingRight = !movingRight;
        }
    }
}
