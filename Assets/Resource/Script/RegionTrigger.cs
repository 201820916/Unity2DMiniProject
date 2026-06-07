using UnityEngine;

public class RegionTrigger : MonoBehaviour
{
    [SerializeField] private EnemyAi2 enemyAI;
    [SerializeField] private int regionIndex;
    [SerializeField] private GameObject enemyObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        UpdateRegion(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        UpdateRegion(other);
    }

    private void UpdateRegion(Collider2D other)
    {
        if (enemyAI == null) return;

        if (other.CompareTag("Player"))
        {
            enemyAI.SetPlayerRegion(regionIndex);
            return;
        }

        if (enemyObject != null && IsEnemyObject(other.gameObject))
        {
            enemyAI.SetEnemyRegion(regionIndex);
        }
    }

    private bool IsEnemyObject(GameObject otherObject)
    {
        return otherObject == enemyObject || otherObject.transform.IsChildOf(enemyObject.transform);
    }
}
