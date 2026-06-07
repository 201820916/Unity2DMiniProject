using UnityEngine;

public class Memory_node : MonoBehaviour
{
    [SerializeField] private GameObject mark;
    [SerializeField] private DataPacketMiniGame miniGame;

    public bool isHack { get; private set; }
    private bool isPlay;
    private bool isNear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isNear = false;
        mark.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && !isHack && Input.GetKeyDown(KeyCode.E))
        {
            mark.SetActive(false);
            isHack = true;
            miniGame.StartMiniGame(OnMiniGameSuccess, OnMiniGameFail);
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
        isNear = false;
        mark.SetActive(false);
    }

    private void OnMiniGameSuccess()
    {
        Debug.Log("해킹에 성공했습니다!");

        isHack = true;
        
        mark.SetActive(false);
    }

    private void OnMiniGameFail()
    {
        isHack = false;
    }
}
