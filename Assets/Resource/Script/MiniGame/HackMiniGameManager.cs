using UnityEngine;

public class HackMiniGameManager : MonoBehaviour
{
    [SerializeField] private HackMiniGame miniGame;
    [SerializeField] private GameObject mark;

    private bool isNear;
    public bool isHack { get; private set; }

    private void Start()
    {
        if (mark != null)
        {
            mark.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isNear) return;
        if (isHack) return;
        if (HackMiniGame.IsPlaying) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (mark != null)
            {
                mark.SetActive(false);
            }

            if (miniGame != null)
            {
                miniGame.StartMiniGame(OnMiniGameSuccess, OnMiniGameFail);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (isHack) return;

        isNear = true;

        if (mark != null)
        {
            mark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isNear = false;

        if (mark != null)
        {
            mark.SetActive(false);
        }
    }

    private void OnMiniGameSuccess()
    {
        isHack = true;

        if (mark != null)
        {
            mark.SetActive(false);
        }
    }

    private void OnMiniGameFail()
    {
        isHack = false;

        if (isNear && mark != null)
        {
            mark.SetActive(true);
        }
    }
}
