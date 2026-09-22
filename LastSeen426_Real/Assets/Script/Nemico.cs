using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("PLAYER")]
    public Transform player;

    [Header("VISIONE")]
    public float detectionRange = 15f;

    [Header("MOVIMENTO")]
    public float speed = 3f;

    [Header("ATTACCO")]
    public float attackRange = 2f;
    public float damage = 20f;
    public float attackCooldown = 1f;

    [Header("ANIMAZIONE")]
    public Animator animator;

    private Rigidbody rb;
    private PlayerHealth playerHealth;
    private float nextAttackTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError(
                "ERRORE: il nemico non ha un Rigidbody!"
            );
            return;
        }

        if (player == null)
        {
            Debug.LogError(
                "ERRORE: trascina il Player nel campo Player dell'EnemyController!"
            );
            return;
        }

        playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            playerHealth = player.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
            playerHealth = player.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null)
            Debug.LogError(
                "ERRORE: PlayerHealth non trovato!"
            );

        // Cerca automaticamente l'Animator
        if (animator == null)
        {
            animator = GetComponent<Animator>();

            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError(
                "ERRORE: Animator non trovato nel nemico!"
            );
        }
    }

    private void FixedUpdate()
    {
        if (player == null || playerHealth == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // PLAYER TROPPO LONTANO
        if (distance > detectionRange)
        {
            SetAnimationSpeed(0f);
            return;
        }

        // PLAYER VICINO: ATTACCO
        if (distance <= attackRange)
        {
            SetAnimationSpeed(0f);
            AttackPlayer();
            return;
        }

        // PLAYER NELLA DETECTION RANGE: INSEGUIMENTO
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            SetAnimationSpeed(0f);
            return;
        }

        direction.Normalize();

        Vector3 movement =
            direction *
            speed *
            Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position + movement
        );

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        rb.MoveRotation(
            Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                10f * Time.fixedDeltaTime
            )
        );

        // Il nemico si sta muovendo → CORSA
        SetAnimationSpeed(1f);
    }

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime =
            Time.time + attackCooldown;

        Debug.Log(
            "NEMICO ATTACCA! Danno: " +
            damage
        );

        playerHealth.TakeDamage(damage);
    }

    private void SetAnimationSpeed(float value)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", value);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}