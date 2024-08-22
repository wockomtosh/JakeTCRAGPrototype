using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControls : MonoBehaviour
{
    CharacterController controller;
    InputAction moveAction;
    InputAction attack1;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        moveAction = GetComponent<PlayerInput>().actions.FindAction("Move");
        moveAction.Enable();
        attack1 = GetComponent<PlayerInput>().actions.FindAction("Attack1");
        attack1.Enable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputDirection.x, 0, inputDirection.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
