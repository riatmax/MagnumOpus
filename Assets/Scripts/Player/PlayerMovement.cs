using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {
        Vector3 moveInput = new Vector3(InputManager.Instance.MoveInput.x, 0, InputManager.Instance.MoveInput.y) * moveSpeed * Time.deltaTime;

        cc.Move(moveInput);
    }
}
