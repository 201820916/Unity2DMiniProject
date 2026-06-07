using UnityEngine;

public class EnemyVisionTrigger : MonoBehaviour
{
    [SerializeField] private EnemyAi2 enemyAI;
    [SerializeField] private float detectTime = 50f;
    [SerializeField] private float detectIncreaseSpeed = 25f;
    [SerializeField] private GameObject mark;
    [SerializeField] private GameObject Vision;
    [SerializeField] private Sprite guardQ;
    [SerializeField] private Sprite guardA;
    [SerializeField] private bool stopChaseWhenVisionLost = true;

    private bool isPlayerInVision;
    private bool isDetected;
    private float countTimer;
    private SpriteRenderer markSpriteRenderer;

    private void Start()
    {
        if (mark != null)
        {
            markSpriteRenderer = mark.GetComponentInChildren<SpriteRenderer>(true);
            mark.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPlayerInVision)
        {
            countTimer = 0f;
            isDetected = false;
            HideMark();
            return;
        }

        countTimer += detectIncreaseSpeed * Time.deltaTime;
        UpdateMark();

        if (!isDetected && countTimer >= detectTime)
        {
            isDetected = true;

            if (enemyAI != null)
            {
                enemyAI.StartChase();
            }
        }
    }

    private void UpdateMark()
    {
        if (mark == null || markSpriteRenderer == null) return;

        if (countTimer >= 50f)
        {
            mark.SetActive(true);
            markSpriteRenderer.sprite = guardA;
        }
        else if (countTimer >= 25f)
        {
            mark.SetActive(true);
            markSpriteRenderer.sprite = guardQ;
        }
        else
        {
            HideMark();
        }
    }

    private void HideMark()
    {
        if (mark == null) return;

        mark.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Enemy vision enter: " + other.name);
        isPlayerInVision = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isPlayerInVision = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Enemy vision exit: " + other.name);
        if (stopChaseWhenVisionLost && isDetected && enemyAI != null)
        {
            enemyAI.StopChase();
        }

        isPlayerInVision = false;
        isDetected = false;
        countTimer = 0f;
        HideMark();
    }
}
