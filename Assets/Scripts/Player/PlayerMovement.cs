using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Fields")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turnSpeed = 720f;
    [SerializeField] private float gravity = -20f;
    private float vertVelocity;

    [Header("Ground Check Fields")]
    [SerializeField] private float groundCheckRadius = 0.65f;
    [SerializeField] private float groundCheckVertOffset = -0.45f;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 groundCheckPos;

    public bool IsGrounded { get; private set; }

    public Vector3 MoveDirection { get; private set; }

    private CharacterController cc;

    private Transform mainCameraTransform;

    private InputManager im;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        mainCameraTransform = Camera.main.transform;
        im = InputManager.Instance;
    }
    private void Update()
    {
        UpdateGroundCheckPosition();
        IsGrounded = Physics.CheckSphere(groundCheckPos, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

        vertVelocity += gravity * Time.deltaTime;

        if (IsGrounded && vertVelocity < 0)
        {
            vertVelocity = -5f;
        }

        Vector3 mainCameraForwardVec = mainCameraTransform.forward;
        mainCameraForwardVec.y = 0;
        mainCameraForwardVec.Normalize();

        Vector3 mainCameraRightVec = mainCameraTransform.right;
        mainCameraRightVec.y = 0;
        mainCameraRightVec.Normalize();

        MoveDirection = mainCameraForwardVec * im.PlayerMoveInput.y + mainCameraRightVec * im.PlayerMoveInput.x;

        Vector3 velocity = MoveDirection * moveSpeed;
        velocity.y = vertVelocity;

        cc.Move(velocity * Time.deltaTime);

        if (MoveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(MoveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        UpdateGroundCheckPosition();
        Gizmos.DrawWireSphere(groundCheckPos, groundCheckRadius);
    }

    private void UpdateGroundCheckPosition()
    {
        groundCheckPos = new Vector3(transform.position.x, transform.position.y + groundCheckVertOffset, transform.position.z);
    }
}
