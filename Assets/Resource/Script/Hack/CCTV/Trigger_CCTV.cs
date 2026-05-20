using UnityEngine;

public class Trigger_CCTV : MonoBehaviour
{

    [SerializeField] private Judge_CCTV JC;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("CCTV 들어옴: " + other.name + " / " + other.tag);

        if (other.CompareTag("Player"))
        {
            JC.SetPlayerInVision(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("CCTV 들어옴: " + other.name + " / " + other.tag);

        if (other.CompareTag("Player"))
        {
            JC.SetPlayerInVision(false);
        }
    }

}
