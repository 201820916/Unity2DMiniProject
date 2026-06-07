using UnityEngine;

public class GameClaer : MonoBehaviour
{
    [SerializeField] private HackMiniGameManager CoolingNode;
    [SerializeField] private Power_MiniGame_Manager powerNode;
    [SerializeField] private Memory_node MemoryNode;
    [SerializeField] private GameObject clearPanel;

    private bool isAllNodeHack;
    private bool isNear;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAllNodeHack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            CheckAllHack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isNear = true;
    }

    private void CheckAllHack()
    {
        if (powerNode.isHack && CoolingNode.isHack && MemoryNode.isHack)
        {
            isAllNodeHack = true;
            clearPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        else
        {
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isNear = false;
    }
}
