using UnityEngine;

public class VIsionBoxTrigger : MonoBehaviour
{
    [SerializeField] private Guard_Vision guardVision;
    [SerializeField] private EnemyAI enemyAI;

    private void OnTriggerEnter2D(Collider2D other)
    { 

    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
}