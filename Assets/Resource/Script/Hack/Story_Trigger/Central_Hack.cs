using UnityEngine;

public class Central_Hack : MonoBehaviour
{
    [SerializeField] private GameObject interactMark;
    //[SerializeField] private StoryProgressManager Story;

    private bool isNear;
    private bool isInstalled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (interactMark != null)
        {
            interactMark.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isNear) return;

        if (isInstalled) return;

        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCentralHack();
        }
    }

    private void StartCentralHack()
    {
        Debug.Log("백도어 해킹 미니게임 시작");

        CompleteBackDoorHack();
    }

    private void CompleteBackDoorHack()
    {
        isInstalled = true;

        if (interactMark != null)
        {
            interactMark.SetActive(false);
        }

        Debug.Log("백도어 설치 완료");

        /*
        if(storyManager != null)
        {
            storyManager.OnBackdoorInstalled();
        }
        
        스토리 관련

        */
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isNear = false;

            if (interactMark != null)
            {
                interactMark.SetActive(false);
            }
        }
    }
}

