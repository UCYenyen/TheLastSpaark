using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    [Header("Boss Settings")]
    public bool canLookAtPlayer = true;
    public bool canMove;
    public bool shouldAttack;
    public bool shouldChasePlayer;
    public bool shouldChargeAttackPlayer;
    public bool isAttacking = false;
    public bool isPlayerInRange = false;

    [Header("Stun Timer")]
    public float startStunnedTime;
    public float stunTime;
    public bool isStunned = false;

    [Header("Attack Settings")]
    public float startTimeBetweenStates = 5f;
    public float timeBetweenStates = 5f;
    public Vector2 playerLastPosition;

    [Header("Attack State")]
    public bool isChargeAttack = false;
    public bool isSnowThrow = false; // peluru snowbal yang naikin freeze meter
    public bool isBlizzardAttack = false; // peluru blizzard freeze player when hit
    [Header("Blizzard Attack")]
    public Transform[] blizzardAttackSpawnPoint;

    [Header("Refrences")]
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject yetiDamageOnContactArea;
    public GameObject[] gamobjectToDeactivateOnDeath;
    public GameObject dashFX;

    [Header("Boss Health")]
    public BossHealthController bossHealthController;

    [Header("Movement Settings")]
    private Vector2 moveDirection;
    public float moveSpeed = 3f;
    public float chargeSpeed = 5f;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            if (startStunnedTime > 0)
            {
                stunTime -= Time.deltaTime;
                if (stunTime <= 0)
                {
                    isStunned = false;
                    canLookAtPlayer = true;
                }
            }
        }
        if (timeBetweenStates > 0 && canMove && isStunned == false)
        {
            timeBetweenStates -= Time.deltaTime;
            if (timeBetweenStates <= 0)
            {
                // Trigger the next attack state
                TriggerNextAttackState();
            }
        }
         LookAtPlayer();
    }
    private void TriggerNextAttackState()
    {
        // Reset the time between states
        timeBetweenStates = startTimeBetweenStates;

        // Randomly choose the next attack state
        int nextState = Random.Range(0,2);
        switch (nextState)
        {
            case 0:
                ResetAttackStates();
                isChargeAttack = true;
                break;
            case 1:
                ResetAttackStates();
                isBlizzardAttack = true;
                anim.SetTrigger("blizzardAttack");
                break;
        }
    }
    private void ResetAttackStates()
    {
        yetiDamageOnContactArea.SetActive(false);
        isStunned = true;
        stunTime = startStunnedTime;
        anim.SetBool("isMoving", false);
        if (dashFX != null)
        {
            dashFX.SetActive(false);
        }
        isChargeAttack = false;
        isSnowThrow = false;
        isBlizzardAttack = false;
    }
    void FixedUpdate()
    {
        ChasePlayer();
        ChargeAttackPlayer();
    }
    public void LookAtPlayer()
    {
        if (PlayerController.instance != null && canLookAtPlayer)
        {
            moveDirection = (PlayerController.instance.transform.position - transform.position).normalized;
            anim.SetFloat("moveDirX", moveDirection.x);
            anim.SetFloat("moveDirY", moveDirection.y);
        }
    }
    public void GetPlayerLastPosition()
    {
        if (PlayerController.instance != null & canLookAtPlayer)
        {
            playerLastPosition = PlayerController.instance.transform.position;
        }
    }
    public void ChasePlayer()
    {
        if (canMove)
        {
            Debug.Log("isChasing");
            if (PlayerController.instance != null && !isChargeAttack && !isStunned)
            {
                Vector2 playerPosition = PlayerController.instance.transform.position;
                Vector2 npcPosition = transform.position;

                if (Vector2.Distance(playerPosition, npcPosition) > 2f)
                {
                    if (Vector2.Distance(playerPosition, npcPosition) < 10f)
                    {
                        transform.position = Vector2.MoveTowards(npcPosition, playerPosition, 2.25f * Time.fixedDeltaTime);
                        anim.SetBool("isMoving", true);
                    }
                    else
                    {
                        anim.SetBool("isMoving", false);
                    }
                }
                else
                {
                    anim.SetBool("isMoving", false);
                }
            }
        }
    }
    public void ChargeAttackPlayer()
    {
        GetPlayerLastPosition();
        Vector2 npcPosition = transform.position;

        float distance = Vector2.Distance(transform.position, playerLastPosition);

        if (PlayerController.instance != null && canMove && isChargeAttack)
        {
            canLookAtPlayer = false;
            yetiDamageOnContactArea.SetActive(true);
            if (dashFX != null)
            {
                dashFX.SetActive(true);
            }

            if (distance > 0.1f)
            {
                transform.position = Vector2.MoveTowards(npcPosition, playerLastPosition, chargeSpeed * Time.fixedDeltaTime);
                anim.SetBool("isMoving", true);
            }
            else
            {
                ResetAttackStates();
                anim.SetBool("isMoving", false);
            }
        }
    }
    public void BlizzardAttack()
    {
        if (isBlizzardAttack)
        {
            foreach (Transform spawnPoint in blizzardAttackSpawnPoint)
            {
                BlizzardObjectPooler.instance.SpawnPooledObject(spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
