using UnityEngine;

public class VIsionBoxTrigger : MonoBehaviour
{
    [SerializeField] private Guard_Vision guardVision;
    [SerializeField] private EnemyAI enemyAI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guardVision.SetPlayerInVision(true);

            if(enemyAI != null)
            {
                enemyAI.StartChase(other.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guardVision.SetPlayerInVision(false);

            if (enemyAI != null)
            {
                enemyAI.StopChase();
            }
        }
    }
}