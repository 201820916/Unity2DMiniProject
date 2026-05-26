using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase
    }

    [Header("State")]
    [SerializeField] private EnemyState currentState = EnemyState.Patrol;

    [Header("Patrol")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 4f;

    private Rigidbody2D rb;
    private Transform patrolTarget;
    private Transform chaseTarget;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolTarget = pointB;
    }

    private void FixedUpdate()
    {
        if (currentState == EnemyState.Patrol)
        {
            Patrol();
        }
        else if (currentState == EnemyState.Chase)
        {
            Chase();
        }
    }

    private void Patrol()
    {
        if (pointA == null || pointB == null) return;

        float direction = Mathf.Sign(patrolTarget.position.x - transform.position.x);


        rb.linearVelocity = new Vector2(direction * patrolSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(transform.position.x - patrolTarget.position.x) < 0.2f)
        {
            patrolTarget = patrolTarget == pointA ? pointB : pointA;
        }
    }

    private void Chase()
    {
        if (chaseTarget == null)
        {
            currentState = EnemyState.Patrol;
            return;
        }

        float direction = Mathf.Sign(chaseTarget.position.x - transform.position.x);

        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
    }

    public void StartChase(Transform target)
    {
        chaseTarget = target;
        currentState = EnemyState.Chase;
    }

    public void StopChase()
    {
        chaseTarget = null;
        currentState = EnemyState.Patrol;
    }
}