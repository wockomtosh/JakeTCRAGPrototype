using System.Collections;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.InputSystem;
using JakeTCRAGPPrototype.Controller.Guitar;
using System.Collections.Generic;

public enum PowerUp
{
    None,
    Strength,
    Speed
}

public class CharacterControls : MonoBehaviour
{
    static CharacterControls instance;

    CharacterController controller;
    Guitar_Controller guitarController;
    InputAction moveAction;
    InputAction attack1Action;
    InputAction dodgeAction;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float powerSpeed = 10f;
    private float curSpeed = 5.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    [SerializeField]
    private float dodgeSpeed = 20;
    [SerializeField]
    private float dodgeLength = .15f;
    private bool dodgeProtection = false;

    [SerializeField]
    private float playerStrength = 1f;
    [SerializeField]
    private float powerStrength = 2f;
    private float curStrength = 1f;

    private bool attacking = false;

    [SerializeField]
    private float knockbackDuration = .2f;
    private bool hasKnockback = false;

    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material speedMaterial;
    [SerializeField]
    private Material strengthMaterial;

    void Start()
    {
        instance = this;

        curSpeed = playerSpeed;
        curStrength = playerStrength;

        controller = GetComponent<CharacterController>();
        guitarController = GetComponentInChildren<Guitar_Controller>();

        moveAction = GetComponent<PlayerInput>().actions.FindAction("Move");
        moveAction.Enable();
        attack1Action = GetComponent<PlayerInput>().actions.FindAction("Attack1");
        attack1Action.Enable();
        dodgeAction = GetComponent<PlayerInput>().actions.FindAction("Dodge");
        dodgeAction.Enable();

        GetComponent<Health>().canTakeDamage = true;
    }

    void Update()
    {
        attacking = guitarController.GetIsSwinging;
        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        if (attacking)
        {
            return;
        }
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputDirection.x, 0, inputDirection.y);
        controller.Move(move * Time.deltaTime * curSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void HandleAttack()
    {
        if (!attacking)
        {
            return;
        }

        //Filter nearby enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Enemy"));

        foreach (Collider enemy in enemies)
        {
            Vector3 enemyVector = enemy.transform.position - transform.position;
            float distToEnemy = enemyVector.magnitude;
            guitarController.GetSwingRadious(out float attackRadius);

            if (distToEnemy < attackRadius)
            {
                if(guitarController.GetSwingTrailEdges(out SwingLine beginLine, out SwingLine endLine))
                {
                    //using player position as start for now
                    Vector3 beginVector = beginLine.P2 - transform.position;
                    Vector3 endVector = endLine.P2 - transform.position;
                    float attackAngle = Vector3.Angle(beginVector, endVector);
                    float beginAngle = Vector3.Angle(beginVector, enemyVector);
                    float endAngle = Vector3.Angle(endVector, enemyVector);
                    if (beginAngle <= attackAngle && endAngle <= attackAngle)
                    {
                        //Apply damage and knockback
                        enemy.GetComponent<Health>().TakeDamage((int)curStrength);

                        //Apply knockback
                        Vector3 knockbackDir = new Vector3(enemyVector.x, 0, enemyVector.z);
                        enemy.GetComponent<EnemyController>().ApplyKnockback(knockbackDir.normalized, 10);
                    }
                }
            }
            
        }
    }

    void OnDodge(InputValue value)
    {
        if (attacking || hasKnockback)
        {
            return;
        }
        playerVelocity += gameObject.transform.forward * dodgeSpeed;
        GetComponent<Health>().canTakeDamage = false;
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(DodgeTimer());
    }

    IEnumerator DodgeTimer()
    {
        yield return new WaitForSeconds(dodgeLength / 2);
        GetComponent<Health>().canTakeDamage = true;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(dodgeLength / 2);
        playerVelocity = Vector3.zero;
    }
    
    void OnAttack1(InputValue value)
    {
        if (guitarController.TriggerSwinging())
        {
            MusicManager.GetInstance().IncreaseGuitar();
        }
    }

    void OnAttack2(InputValue value)
    {
        if (guitarController.TriggerSwinging())
        {
            MusicManager.GetInstance().IncreaseKeyboard();
        }
    }

    public void ApplyKnockback(Vector3 direction, float power)
    {
        if (hasKnockback)
        {
            return;
        }
        playerVelocity += direction * power;
        hasKnockback = true;
        moveAction.Disable();
        attack1Action.Disable();
        dodgeAction.Disable();
        StartCoroutine(KnockbackTimer());
    }

    IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(knockbackDuration);
        playerVelocity = Vector3.zero;
        hasKnockback = false;
        moveAction.Enable();
        attack1Action.Enable();
        dodgeAction.Enable();
    }

    public void SetPowerUp(PowerUp powerUp)
    {
        switch (powerUp)
        {
            case PowerUp.Speed:
                setSpeedMaterial();
                curSpeed = powerSpeed;
                break;
            case PowerUp.Strength:
                setStrengthMaterial();
                curStrength = powerStrength;
                break;
        }
    }

    public void RemovePowerUp(PowerUp powerUp)
    {
        switch (powerUp)
        {
            case PowerUp.Speed:
                setDefaultMaterial();
                curSpeed = playerSpeed;
                break;
            case PowerUp.Strength:
                setDefaultMaterial();
                curStrength = playerStrength;
                break;
        }
    }

    public void setSpeedMaterial()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = speedMaterial;
    }

    public void setStrengthMaterial()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = strengthMaterial;
    }

    public void setDefaultMaterial()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = defaultMaterial;
    }

    public static CharacterControls GetInstance()
    {
        return instance;
    }
}
