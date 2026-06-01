using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private Vector3 teleportOffset = new Vector3(-5f, 0f, 0f);
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private Animator enemyAnimator;

    public Vector3 PlayerPosition { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

    private void Start()
    {
        
    }

    private void Update()
    {

        if (player == null || enemy == null) return;


        PlayerPosition = player.transform.position;
        
        EnemyPosition = enemy.transform.position;

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        float distance = Mathf.Abs(playerPosition.x - enemyPosition.x);

        if (distance <= stopDistance)
        {
            return;
        }

        float direction = Mathf.Sign(playerPosition.x - enemyPosition.x);

        enemy.transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);
    }
}