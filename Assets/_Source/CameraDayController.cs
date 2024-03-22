using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class CameraDayController : MonoBehaviour
{

    PlayerInputAction playerInputAction;

    [Header("Camera Settings")]

    [SerializeField] private float panSpeed = 100f;
    [SerializeField] private float panBorderThickness = 10f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float smoothSpeed = 0.5f;
     
    [SerializeField] float rotateSpeed = 90;

    

    [Header("Camera Limitations")]
    [SerializeField] private float minDepth = 1f;
    [SerializeField] private float maxDepth = 200f;

    [SerializeField] private float LeftRightDistance = -100f;
    [SerializeField] private float TopDownMovement = 55f;



    private void Awake()
    {

        playerInputAction = new PlayerInputAction();
        playerInputAction.BuilderController.Enable();
        playerInputAction.BuilderController.Rotate.performed += Rotate;
    }


    void LateUpdate()
    {

        MoveByMouse();
        MoveByKeyboard();
        Zoom();
           


    }

    private void MoveByMouse()
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

        
        //Camera bounds
        pos.x = Mathf.Clamp(pos.x, LeftRightDistance, TopDownMovement);
        pos.z = Mathf.Clamp(pos.z, LeftRightDistance, TopDownMovement);

        transform.position = Vector3.Lerp(transform.position, pos, smoothSpeed);
    }
    private void MoveByKeyboard()
    {
        Vector2 input = playerInputAction.BuilderController.Movement.ReadValue<Vector2>();
        if (input.x == 0 && input.y == 0)
            return;

        Vector3 pos = transform.position;

        Vector3 moveDir = transform.forward * input.y + transform.right * input.x;
        pos += (moveDir * panSpeed);


        //Camera bounds
        pos.x = Mathf.Clamp(pos.x, LeftRightDistance, TopDownMovement);
        pos.z = Mathf.Clamp(pos.z, LeftRightDistance, TopDownMovement);

        transform.position = Vector3.Lerp(transform.position, pos , Time.deltaTime * smoothSpeed);
    }

    public static float Normalize(float val, float min, float max)
    {
        return (val - min) / (max - min);
    }


    private void Zoom()
    {
        float input = playerInputAction.BuilderController.Zoom.ReadValue<float>();
        //mouse has scroll has different value than keyboard
        //cant clamp(-1, 1) because holding button is easier than scrolling

        if (input > 1)
            input = -zoomSpeed;
        else if (input < -1)
            input = zoomSpeed;
        else if (input == 0)
            return;


        Vector3 movement = transform.position;
        movement.y = movement.y + input;
        //Camera bounds
        movement.y = Mathf.Clamp(movement.y, minDepth, maxDepth);

        transform.position = movement; 
        

    }


    public void Rotate(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        Debug.Log("Rotate" + input);

        if(transform.eulerAngles.y > 360)
        {
            transform.eulerAngles -= new Vector3(0, 360, 0);
        }
        else if(transform.eulerAngles.y < -360)
        {
            transform.eulerAngles += new Vector3(0, 360, 0);
        }

        transform.eulerAngles += new Vector3(0, input * rotateSpeed, 0);

    }
}