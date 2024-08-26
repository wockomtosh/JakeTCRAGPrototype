using JetBrains.Annotations;
using System;
using System.Collections;
using UnityEngine;

[Serializable]
enum EnemyMovementAction
{
    DashToPlayer,
    DashFromPlayer,
    StayStill
}

[Serializable]
struct EnemyBeat
{
    public Color beatColor;
    public bool attacking;
    public bool shielding;
    public EnemyMovementAction movement;

    public EnemyBeat(Color beatColor, bool attacking, bool shielding, EnemyMovementAction movement)
    {
        this.beatColor = beatColor;
        this.attacking = attacking;
        this.shielding = shielding;
        this.movement = movement;
    }
}

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    GameObject attackZone;
    [SerializeField] 
    BoxCollider attackCollider;
    [SerializeField]
    GameObject shield;
    SpriteRenderer spriteRenderer;
    CharacterController controller;

    [SerializeField]
    private float bpm = 76;
    private float beatLength;
    private float beatTimer = 0;
    private int timeSignature = 4;
    private int curBeat = 0;

    [SerializeField]
    private float beatMoveSpeed = 15;
    [SerializeField]
    private float beatMoveDuration = .1f;

    [SerializeField]
    private EnemyBeat beat1 = new EnemyBeat(Color.white, false, false, EnemyMovementAction.DashToPlayer);
    [SerializeField]
    private EnemyBeat beat2 = new EnemyBeat(new Color(1, 0, 0, .3f), false, true, EnemyMovementAction.StayStill);
    [SerializeField]
    private EnemyBeat beat3 = new EnemyBeat(new Color(1, 0, 0, .6f), false, false, EnemyMovementAction.DashToPlayer);
    [SerializeField]
    private EnemyBeat beat4 = new EnemyBeat(Color.red, true, true, EnemyMovementAction.StayStill);

    private Vector3 attackPos = new Vector3(0, .9f, 0);
    private Vector3 restPos = Vector3.zero;

    private bool attacking = false;

    [SerializeField]
    private bool followPlayer = false;
    [SerializeField]
    private float followDistance = 10;
    [SerializeField]
    private float enemySpeed = 2.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    private Vector3 enemyVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float knockbackDuration = .2f;
    private bool hasKnockback = false;


    void Start()
    {
        beatLength = 60 / bpm;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateBeats();
        UpdateMovement();
    }

    void UpdateBeats()
    {
        beatTimer += Time.deltaTime;
        if (beatTimer > beatLength)
        {
            //We'll need to be very precise with beat timing or we'll need a way to sync up with the music every measure
            beatTimer -= beatLength;

            IncrementBeat();

            switch (curBeat)
            {
                case 0:
                    ApplyEnemyBeat(beat1);
                    break;
                case 1:
                    ApplyEnemyBeat(beat2);
                    break;
                case 2:
                    ApplyEnemyBeat(beat3);
                    break;
                case 3:
                    ApplyEnemyBeat(beat4);
                    break;
            }
        }
    }

    void IncrementBeat()
    {
        curBeat++;
        if (curBeat >= timeSignature)
        {
            curBeat = 0;
        }
    }

    void ApplyEnemyBeat(EnemyBeat beat) 
    {
        shield.SetActive(beat.shielding);
        GetComponent<Health>().canTakeDamage = !beat.shielding;
        spriteRenderer.color = beat.beatColor;
        if (beat.attacking)
        {
            attacking = true;
            attackZone.transform.position = transform.position + attackPos;
            attackCollider.enabled = true;
        }
        else
        {
            attacking = false;
            attackZone.transform.position = transform.position + restPos;
            attackCollider.enabled = false;
        }

        if (!hasKnockback)
        {
            if (beat.movement == EnemyMovementAction.DashToPlayer)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector3 toPlayerDirection = player.transform.position - transform.position;
                enemyVelocity += toPlayerDirection.normalized * beatMoveSpeed;
                StartCoroutine(BeatMoveTimer());
            }
            if (beat.movement == EnemyMovementAction.DashFromPlayer)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Vector3 toPlayerDirection = player.transform.position - transform.position;
                enemyVelocity += -toPlayerDirection.normalized * beatMoveSpeed;
                StartCoroutine(BeatMoveTimer());
            }
        }
    }

    IEnumerator BeatMoveTimer()
    {
        yield return new WaitForSeconds(beatMoveDuration);
        enemyVelocity = Vector3.zero;
    }

    void UpdateMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && enemyVelocity.y < 0)
        {
            enemyVelocity.y = 0f;
        }

        enemyVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(enemyVelocity * Time.deltaTime);

        if (followPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 toPlayerDirection = player.transform.position - transform.position;
            float toPlayerDistance = toPlayerDirection.magnitude;
            toPlayerDirection = toPlayerDirection.normalized;
            if (toPlayerDistance < followDistance)
            {
                Vector3 move = new Vector3(toPlayerDirection.x, 0, toPlayerDirection.z);
                Debug.Log(move);
                controller.Move(move * Time.deltaTime * enemySpeed);

                if (move != Vector3.zero)
                {
                    gameObject.transform.forward = move;
                }
            }
        }
    }

    public void ApplyKnockback(Vector3 direction, float power)
    {
        if (hasKnockback)
        {
            return;
        }
        hasKnockback = true;
        enemyVelocity += direction * power;
        StartCoroutine(KnockbackTimer());
    }

    IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(knockbackDuration);
        enemyVelocity = Vector3.zero;
        hasKnockback = false;
    }
}
