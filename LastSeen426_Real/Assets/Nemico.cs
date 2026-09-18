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

    [Tooltip("Danno inflitto dal nemico ad ogni attacco")]
    public float damage = 20f;

    [Tooltip("Tempo minimo tra un attacco e l'altro")]
    public float attackCooldown = 1f;

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

        playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            playerHealth =
                player.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            playerHealth =
                player.GetComponentInChildren<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            Debug.LogError(
                "ERRORE: PlayerHealth non trovato!"
            );
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (playerHealth == null)
        {
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // FUORI DAL RAGGIO
        if (distance > detectionRange)
        {
            return;
        }

        // ATTACCO
        if (distance <= attackRange)
        {
            AttackPlayer();
            return;
        }

        // INSEGUIMENTO
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
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
                10f *
                Time.fixedDeltaTime
            )
        );
    }

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime =
            Time.time + attackCooldown;

        Debug.Log(
            "NEMICO ATTACCA! Danno: " +
            damage
        );

        playerHealth.TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        // Raggio di rilevamento
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        // Raggio di attacco
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}