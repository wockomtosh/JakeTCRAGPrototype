using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControls : MonoBehaviour
{
    CharacterController controller;
    InputAction moveAction;
    InputAction attack1Action;
    InputAction dodgeAction;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private float dodgeSpeed = 20;
    [SerializeField]
    private float dodgeLength = .15f;
    private bool dodgeProtection = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        moveAction = GetComponent<PlayerInput>().actions.FindAction("Move");
        moveAction.Enable();
        attack1Action = GetComponent<PlayerInput>().actions.FindAction("Attack1");
        attack1Action.Enable();
        dodgeAction = GetComponent<PlayerInput>().actions.FindAction("Dodge");
        dodgeAction.Enable();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
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

    void OnDodge(InputValue value)
    {
        playerVelocity += gameObject.transform.forward * dodgeSpeed;
        dodgeProtection = true;
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(DodgeTimer());
    }

    IEnumerator DodgeTimer()
    {
        yield return new WaitForSeconds(dodgeLength / 2);
        dodgeProtection = false;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(dodgeLength / 2);
        playerVelocity = Vector3.zero;
    }
}
