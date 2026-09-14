using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Riferimenti")]
    [Tooltip("Trascina qui l'oggetto del Player dall'Inspector")]
    public Transform player;

    [Header("Impostazioni Inseguimento")]
    [Tooltip("Distanza entro cui il nemico inizia a inseguire il giocatore")]
    public float detectionRange = 10f;

    [Header("Impostazioni Movimento")]
    [Tooltip("Velocità di movimento del nemico")]
    public float speed = 3.0f;

    [Tooltip("Punti della piattaforma per il pattugliamento (opzionale)")]
    public Transform[] patrolPoints;

    [Header("DANNO AL PLAYER")]
    [Tooltip("Danno inflitto ogni secondo quando il nemico tocca il Player")]
    public float damagePerSecond = 20f;

    private int currentPatrolIndex = 0;

    void Update()
    {
        // Se il Player non è stato assegnato, prova a cercarlo tramite Tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                return;
            }
        }

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsTarget(player.position);
        }
        else
        {
            PatrolLogic();
        }
    }

    private void PatrolLogic()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }

        Transform targetPoint = patrolPoints[currentPatrolIndex];

        if (targetPoint == null)
        {
            return;
        }

        MoveTowardsTarget(targetPoint.position);

        if (Vector3.Distance(
            transform.position,
            targetPoint.position
        ) < 0.3f)
        {
            currentPatrolIndex =
                (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 targetDestination = new Vector3(
            targetPosition.x,
            transform.position.y,
            targetPosition.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetDestination,
            speed * Time.deltaTime
        );

        Vector3 direction =
            (targetDestination - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * 10f
            );
        }
    }

    // Quando il nemico tocca il Player
    // con un Collider normale
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamagePlayer(collision.gameObject);
        }
    }

    // Quando il nemico tocca il Player
    // con un Trigger
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DamagePlayer(other.gameObject);
        }
    }

    private void DamagePlayer(GameObject playerObj)
    {
        PlayerHealth health =
            playerObj.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(
                damagePerSecond * Time.deltaTime
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );
    }
}