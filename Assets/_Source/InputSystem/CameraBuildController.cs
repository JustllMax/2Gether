using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBuildController : MonoBehaviour
{

    [SerializeField] PlayerInputAction playerInputAction;
    [SerializeField] float rotateSpeed = 90;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float zoomSpeed = 2;
    [SerializeField] float smoothSpeed = 0.5f;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.BuilderController.Enable();
        playerInputAction.BuilderController.Rotate.performed += Rotate;
    }


    private void LateUpdate()
    {

        Move();
        Zoom();
        
    }

    private void Move()
    {
        Vector2 input = playerInputAction.BuilderController.Movement.ReadValue<Vector2>();
        if (input.x == 0 && input.y == 0)
            return;

        Vector3 moveDir = transform.forward * input.y + transform.right * input.x;
        Vector3 pos = transform.position;
        transform.position = Vector3.Lerp(transform.position, pos + (moveDir * moveSpeed), Time.deltaTime * smoothSpeed);
    }


    private void Zoom()
    {
        float input = playerInputAction.BuilderController.Zoom.ReadValue<float>();
        //mouse has scroll has different value than keyboard
        //cant clamp(-1, 1) because holding button is easier than scrolling
        if (input > 1)
            input = 10;
        if(input < -1)
            input = -10;
        
        if (input == 0) 
            return;

        Vector3 movement = new Vector3(transform.position.x, transform.position.y + (input * zoomSpeed), transform.position.z);


        transform.position = Vector3.Lerp(transform.position, movement, Time.deltaTime * smoothSpeed);

    }


    public void Rotate(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        Debug.Log("Rotate" + input);


        transform.eulerAngles += new Vector3(0, input * rotateSpeed, 0);

    }
}
