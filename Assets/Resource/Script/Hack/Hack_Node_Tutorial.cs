using UnityEngine;
using System.Collections;
public class Hack_Node_Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject offmark;
    [SerializeField] private HackMiniGame miniGame;
    [SerializeField] private GameObject Tutorial5;

    public bool isHack;
    public bool isNear;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isHack = false;
        isNear = false;
        offmark.SetActive(false);
        Tutorial5.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && !isHack)
            // 가까이 있음 + 해킹 안했음 + 태그 확인
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                isHack = true;
                miniGame.StartMiniGame(OnMiniGameSuccess, OnMiniGameFail);
            }
        }
    }

    private void OnMiniGameSuccess()
    {
        Tutorial5.SetActive(true);
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

            if (isNear)
            {
                offmark.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            offmark.SetActive(false);
        }
    }
}
