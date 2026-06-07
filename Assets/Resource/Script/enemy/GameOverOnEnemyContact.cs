using UnityEngine;

public class GameOverOnEnemyContact : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Result")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Character_move playerMovement;
    [SerializeField] private EnemyAi2 enemyAI;
    [SerializeField] private bool pauseGameOnGameOver = true;

    private bool isGameOver;

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<Character_move>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryGameOver(gameObject, other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryGameOver(gameObject, collision.gameObject);
    }

    private void TryGameOver(GameObject firstObject, GameObject secondObject)
    {
        if (isGameOver) return;

        bool playerTouchedEnemy =
            IsPlayerObject(firstObject) && IsEnemyObject(secondObject) ||
            IsEnemyObject(firstObject) && IsPlayerObject(secondObject);

        if (!playerTouchedEnemy) return;

        GameOver();
    }

    private void GameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (playerMovement != null)
        {
            playerMovement.SetCanMove(false);
        }

        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        if (pauseGameOnGameOver)
        {
            Time.timeScale = 0f;
        }

        Debug.Log("Game Over: player touched enemy.");
    }

    private bool IsPlayerObject(GameObject target)
    {
        if (playerObject != null)
        {
            return IsSameOrChild(target, playerObject);
        }

        return target.CompareTag(playerTag);
    }

    private bool IsEnemyObject(GameObject target)
    {
        if (enemyObject != null)
        {
            return target == enemyObject;
        }

        return target.CompareTag(enemyTag);
    }

    private bool IsSameOrChild(GameObject target, GameObject root)
    {
        return target == root || target.transform.IsChildOf(root.transform);
    }
}
