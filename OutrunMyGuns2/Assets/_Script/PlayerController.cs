using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    [Header("Camera")]
    private Vector2 Rotation;
    [HideInInspector] public float rotY;

    [SerializeField] private Transform cameraTransform, weaponTransform;
    [Range(1, 20)]
    [SerializeField] private float cameraSensibility;
    [SerializeField] private Vector2 minMaxVerticalValues;
    [Range(1, 140)]
    [SerializeField] private float defaultCameraFov;
    [Range(1, 100)]
    [SerializeField] private float fovChangeSpeed;
    [Range(0, 10)]
    [SerializeField] private float fovChangeMarge;


    [Header("Movement")]
    Vector3 velocity;
    private Vector2 movementInput;
    public float Speed = 5f;

    [Header("Jump")]
    [SerializeField] float gravity;
    public float JumpHeight = 2f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraLook();
        Movement();
        Jump();
    }

    private void CameraLook()
    {
        Rotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotY += Rotation.y * -cameraSensibility;
        transform.localEulerAngles += new Vector3(0, Rotation.x * cameraSensibility, 0);

        rotY = Mathf.Clamp(rotY, minMaxVerticalValues.x, minMaxVerticalValues.y);
        cameraTransform.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
        weaponTransform.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
    }


    private void FixedUpdate()
    {
        //Debug.Log(InputSystem.onDeviceChange);
        Gravity();
    }

    private void Gravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Movement()
    {
        Vector3 _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _moveDirection = transform.TransformDirection(_moveDirection);
        controller.Move(_moveDirection * Speed * Time.deltaTime);
    }

    private void Jump()
    {
        if (!controller.isGrounded)
        {
            return;
        }
        if (Input.GetAxisRaw("Jump") > 0)
        {
            //velocity = new Vector3(controller.velocity.x, Mathf.Sqrt(JumpHeight * -2f * gravity), controller.velocity.z);
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
            controller.Move(velocity * Time.deltaTime);
        }
    }

}
