using UnityEngine;

public class LiftTeleport : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform destination;


    private GameObject playerInRange;

    private void Update()
    {
        if (HackMiniGame.IsPlaying) return;

        if (playerInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            playerInRange.transform.position = destination.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject == playerInRange)
        {
            playerInRange = null;
        }
    }
}
