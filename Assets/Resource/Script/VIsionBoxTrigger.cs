using UnityEngine;

public class VIsionBoxTrigger : MonoBehaviour
{
    [SerializeField] private Guard_Vision guardVision;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guardVision.SetPlayerInVision(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            guardVision.SetPlayerInVision(false);
        }
    }
}