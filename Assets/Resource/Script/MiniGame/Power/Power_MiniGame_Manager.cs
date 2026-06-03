using UnityEngine;

public class Power_MiniGame_Manager : MonoBehaviour
{
    [SerializeField] private GameObject PowerNode;
    [SerializeField] private PowerRoom_MiniGame C_MiniGame;
    [SerializeField] private GameObject mark;

    bool isHack;
    bool isNear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isHack = false;
        isNear = false;
        mark.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isNear && !isHack && Input.GetKeyDown(KeyCode.E))
        {
            mark.SetActive(false);
            isHack = true;

            C_MiniGame.StartMiniGame(OnMiniGameSuccess, OnminiGameFail);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (isNear) return;
        if (isHack) return;

        isNear = true;
        mark.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isNear = false;
        mark.SetActive(false);
    }

    private void OnMiniGameSuccess()
    {
        Debug.Log("해킹에 성공했습니다.");
        isHack = true;
        mark.SetActive(false);
    }

    private void OnminiGameFail()
    {
        isHack = false;

        if (isNear)
        {
            mark.SetActive(true);
        }
    }
}
