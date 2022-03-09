using UnityEngine;

public enum MovementState { Idle, Walk, Run, Crouch}
public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    [Header("Camera")]
    private Vector2 Rotation;
    [HideInInspector] public float rotY;

    public Transform cameraTransform;
    [Range(1, 20)]
    [SerializeField] private float cameraSensibility;
    [SerializeField] private Vector2 minMaxVerticalValues;

    [Header("Movement")]
    public MovementState MovementState;
    public bool IsRunning = false;
    Vector3 velocity;
    public float SpeedWalk = 5f, SpeedRunFactor = 1.5f, SpeedCrounchFactor = 0.5f;

    [Header("Jump")]
    public bool isGrounded;
    public LayerMask LayerGround;
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
        OnTheGround();
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
    }


    private void FixedUpdate()
    {
        //Debug.Log(InputSystem.onDeviceChange);
        Gravity();
    }

    private void OnTheGround()
    {
        isGrounded = Physics.CheckSphere(transform.position - Vector3.up, 0.3f, LayerGround);
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
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            MovementState = MovementState.Idle;
            return;
        }
        Vector3 _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        float _facteurStateMovement = 1f;
        if (Input.GetKey(KeyCode.LeftShift) ) //add bool if can run !
        {
            _facteurStateMovement = 1.5f;
            MovementState = MovementState.Run;
        }
        else
        {
            MovementState = MovementState.Walk;
        }
        _moveDirection = transform.TransformDirection(_moveDirection);
        controller.Move(_moveDirection * SpeedWalk * _facteurStateMovement * Time.deltaTime);
    }

    private void Jump()
    {
        if (!isGrounded)
        {
            return;
        }
        if (Input.GetAxisRaw("Jump") > 0)
        {
            //velocity = new Vector3(controller.velocity.x, Mathf.Sqrt(JumpHeight * -2f * gravity), controller.velocity.z);
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
            //Debug.Log(velocity);
            controller.Move(velocity * Time.deltaTime);
        }
    }

}
