using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Hack_Door : MonoBehaviour
{
    [SerializeField] private GameObject lockblocker;
    [SerializeField] private HackMiniGame miniGame;
    [SerializeField] private GameObject EnterCenter;
    public bool isHack;
    public bool isNear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isHack = false;
        isNear = false;
        lockblocker.SetActive(true);
        EnterCenter.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (HackMiniGame.IsPlaying) return;

        if (isNear && !isHack)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isHack = true;
                miniGame.StartMiniGame(OnMiniGameSuccess, OnMiniGameFail);
            }
        }
    }
    private void OnMiniGameSuccess()
    {
        lockblocker.SetActive(false);
        EnterCenter.SetActive(true);
    }

    private void OnMiniGameFail()
    {
        isHack = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }
}

