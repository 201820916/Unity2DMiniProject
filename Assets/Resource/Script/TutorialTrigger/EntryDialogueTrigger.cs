using UnityEngine;

public class EntryDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string dialogueId = "entry";
    [SerializeField] private GameObject Bubble;


    private bool hasPlayed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;

            dialogueManager.StartDialogue(dialogueId);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
       if (!hasPlayed) return;

       if (other.CompareTag("Player"))
       {
            Bubble.SetActive(false);
       }
    }
}
