using UnityEngine;

public class LiftTeleport : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform destination;

    // 플레이어가 이동한 지역 번호를 EnemyAI에 알려주기 위한 참조입니다.
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyAi2 enemyAi2;

    // 이 텔레포트가 도착시키는 지역 번호입니다. 3x3 지역에 맞춰 Inspector에서 설정합니다.
    [SerializeField] private int destinationRegionIndex;


    private GameObject playerInRange;

    private void Update()
    {
        if (HackMiniGame.IsPlaying) return;

        if (playerInRange != null && Input.GetKeyDown(KeyCode.E))
        {
            playerInRange.transform.position = destination.position;

            if (enemyAI != null)
            {
                // 플레이어 지역 번호를 갱신해야 추적자가 다른 줄의 같은 X좌표를 같은 지역으로 착각하지 않습니다.
                enemyAI.SetPlayerRegion(destinationRegionIndex);
            }

            if (enemyAi2 != null)
            {
                enemyAi2.SetPlayerRegion(destinationRegionIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject == playerInRange)
        {
            playerInRange = null;
        }
    }
}

