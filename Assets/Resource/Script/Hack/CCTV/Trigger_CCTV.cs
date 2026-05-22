using UnityEngine;

public class Trigger_CCTV : MonoBehaviour
{

    [SerializeField] private Judge_CCTV CCTV_Judge;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("CCTV 들어옴: " + other.name + " / " + other.tag);

        if (other.CompareTag("Player"))
        {
            CCTV_Judge.SetPlayerInVision(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("CCTV 들어옴: " + other.name + " / " + other.tag);

        if (other.CompareTag("Player"))
        {
            CCTV_Judge.SetPlayerInVision(false);
        }
    }

}
