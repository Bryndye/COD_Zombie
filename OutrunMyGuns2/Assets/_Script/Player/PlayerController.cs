using UnityEngine;
using TMPro;

public enum MovementState { Idle, Walk, Run}
public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    PlayerWeapon playerW;
    PlayerCut playerCut;
    [SerializeField] TextMeshProUGUI textVelocity;

    [Header("Camera")]
    private Vector2 Rotation;
    [HideInInspector] public float rotY;

    public Transform cameraTransform;
    [Range(1, 20)]
    [SerializeField] private float cameraSensibility;
    [SerializeField] private Vector2 minMaxVerticalValues;

    [Header("Movement")]
    public MovementState PlayerMvmtState;
    Vector3 velocity;
    public float SpeedWalk = 5f, SpeedRunFactor = 1.5f;
    float facteurStateMovement = 1;

    [Header("Jump")]
    public bool isGrounded;
    public LayerMask LayerGround;
    [SerializeField] float gravity;
    public float JumpHeight = 2f;

    [Header("Crouch")]
    public bool IsCrouching = false;
    [SerializeField] Vector3 posCamCrouch, posCamDefault;
    [SerializeField] float scaleCollider = 1, SpeedCrounchFactor = 0.5f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerW = GetComponent<PlayerWeapon>();
        playerCut = GetComponent<PlayerCut>();
        posCamDefault = cameraTransform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraLook();
        OnTheGround();
        Crouch();
        Movement();
        Jump();

        textVelocity.text = controller.velocity.ToString();
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

    private float FacteurMovementManager()
    {
        if (PlayerMvmtState == MovementState.Walk)
        {
            return facteurStateMovement = 1;
        }
        else if (PlayerMvmtState == MovementState.Run)
        {
            return facteurStateMovement = 1.5f;
        }
        else
        {
            return facteurStateMovement = 1f;
        }
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
            PlayerMvmtState = MovementState.Idle;
            return;
        }
        Vector3 _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        if (Input.GetKey(KeyCode.LeftShift) && !IsCrouching && Input.GetAxis("Vertical") > 0 && !playerCut.IsCutting)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && playerW.IsReloading)
            {
                //Debug.Log("cancel anim reload from run");
                playerW.CancelAnimReload();
            }
            else if (!playerW.IsReloading)
            {
                PlayerMvmtState = MovementState.Run;
            }
        }
        else
        {
            PlayerMvmtState = MovementState.Walk;
        }
        _moveDirection = transform.TransformDirection(_moveDirection);
        controller.Move(_moveDirection * SpeedWalk * FacteurMovementManager() * (IsCrouching ? 0.5f : 1) * (playerW.IsAiming ? 0.5f : 1) * Time.deltaTime);
    }

    private void Jump()
    {
        if (!isGrounded)
        {
            return;
        }
        if (Input.GetAxisRaw("Jump") > 0)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
            //Debug.Log(velocity);
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            IsCrouching = true;
            controller.height = scaleCollider;
            cameraTransform.localPosition = posCamCrouch;

        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            IsCrouching = false;
            PlayerMvmtState = MovementState.Idle;
            controller.height = 2;
            cameraTransform.localPosition = posCamDefault;
        }
    }
}
