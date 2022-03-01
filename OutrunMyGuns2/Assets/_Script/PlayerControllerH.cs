using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerH : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float PlayerGravity = 10;

    [Header("Camera")]
    private Vector2 Rotation;
    [HideInInspector] public float rotY;

    [SerializeField] private Transform cameraTransform;
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
    private Vector2 movementInput;
    public float Speed = 5f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 400f;
    public bool onGround;
    [SerializeField] Vector3 posCheckGround;
    public LayerMask LayerGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0, -PlayerGravity, 0);
    }

    void Update()
    {

        CameraLook();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }

    private bool CheckIsOnGround()
    {
        return true;
    }

    private void Jump()
    {
        onGround = Physics.CheckSphere(transform.position - posCheckGround, 0.1f, LayerGround);
    }

    private void Movement()
    {
        Vector3 _moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        _moveDirection = transform.TransformDirection(_moveDirection);
        rb.velocity = _moveDirection * Speed * Time.deltaTime;
    }

    private void CameraLook()
    {
        rotY += Rotation.y * -cameraSensibility * Time.deltaTime;
        transform.localEulerAngles += new Vector3(0, Rotation.x * cameraSensibility * Time.deltaTime, 0);

        rotY = Mathf.Clamp(rotY, minMaxVerticalValues.x, minMaxVerticalValues.y);
        cameraTransform.transform.localRotation = Quaternion.Euler(rotY, 0, 0);
    }


    #region Input Manager
    public void OnMove(InputAction.CallbackContext ctx) 
    { 
        movementInput = ctx.ReadValue<Vector2>(); 
    }

    public void GetRotation(InputAction.CallbackContext ctx)
    {
        Rotation = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    #endregion
}
