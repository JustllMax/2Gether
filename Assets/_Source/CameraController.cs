using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float panSpeed = 100f;
    [SerializeField]
    private float panBorderThickness = 10f;
    [SerializeField]
    private float scrollSpeed = 10f;
    [SerializeField]
    private float smoothSpeed = 0.5f;


    [SerializeField]
    private float minDepth = 50f;
    [SerializeField]
    private float maxDepth = 200f;

    //Variables can be adjusted, but it's important to maintain equal values for 'Left' and 'Backward', as well as for 'Right' and 'Forward' to ensure a consistent and balanced isometric camera movement boundary.
    [SerializeField]
    private float cameraLeft = -100f;
    [SerializeField]
    private float cameraRight = 55f;
    [SerializeField]
    private float cameraBackward = -100f;
    [SerializeField]
    private float cameraForward = 55f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos += forward * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos -= forward * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos += right * panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos -= right * panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minDepth, maxDepth);

        // Nowe ograniczenia dla X i Z z wykorzystaniem struktury CameraBounds
        pos.x = Mathf.Clamp(pos.x, cameraLeft, cameraRight);
        pos.z = Mathf.Clamp(pos.z, cameraBackward, cameraForward);

        transform.position = Vector3.Lerp(transform.position, pos, smoothSpeed);
    }
}