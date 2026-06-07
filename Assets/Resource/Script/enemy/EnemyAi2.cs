using UnityEngine;

public class EnemyAi2 : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Transform player; // 추적할 플레이어
    [SerializeField] private GameObject enemy; // 실제로 추적하는 적
    [SerializeField] private SpriteRenderer enemySprite; // 방향 전환에 사용될 렌더러
    [SerializeField] private Animator enemyAnimator; // 애니메이션 제어용

    [Header("State")]
    [SerializeField] private EnemyState currentState; // 현재 상태

    // 3x3 배치에서는 X좌표만으로 지역을 구분할 수 없어 지역 번호를 따로 저장합니다.
    [SerializeField] private int playerRegionIndex; // 플레이어 지역 번호
    [SerializeField] private int enemyRegionIndex; // 적이 있는 지역 번호

    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 1.5f; // 플레이어-적 이 이 거리 이하면 적이 멈춤(임시)
    [SerializeField] private float patrolSpeed = 1.2f; // 패트롤 속도
    [SerializeField] private float searchWaitTime = 1.5f; // 패트롤 도착 후 대기 시간
    [SerializeField] private float movePointArriveDistance = 0.15f; // 

    [Header("MovePoint")]
    // 9개 지역의 이동 기준점과 연결 정보를 넣습니다.
    // regions[0]은 1번 지역, regions[8]은 9번 지역입니다.
    [SerializeField] private RegionMovePoints[] regions;


    [Header("Patrol")]
    private Transform currentMovePoint;
    private int targetRegionIndex = -1;
    private EnemyState stateAfterTeleport = EnemyState.Patrol;
    private float waitTimer;
    private bool isWaitingAtMovePoint;

    public Vector3 PlayerPosition { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

    public enum EnemyState
    {
        Patrol,
        Chase,
        MoveToTeleport,
        MoveToPatrolPoint
    }

    private void Start()
    {
        currentState = EnemyState.Patrol;

    }
    private void Update()
    {
        if (player == null || enemy == null) return;

        PlayerPosition = player.position;
        EnemyPosition = enemy.transform.position;

        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolManage();
                break;

            case EnemyState.Chase:
                UpdateChase();
                break;

            case EnemyState.MoveToTeleport:
                UpdateMoveToTeleport();
                break;

            case EnemyState.MoveToPatrolPoint:
                UpdateMoveToPatrolPoint();
                break;
        }
    }

    private void PickConnectedMovePoint()
    {
        if (!IsValidRegionIndex(enemyRegionIndex)) return;

        currentMovePoint = regions[enemyRegionIndex].patrolPoint;
    }

    private bool IsValidRegionIndex(int regionIndex)
    {
        return regions != null && regionIndex >= 0 && regionIndex < regions.Length;
    }

    private void PickRandomTeleportTarget()
    {
        if (!IsValidRegionIndex(enemyRegionIndex)) return;

        int[] connectedRegions = regions[enemyRegionIndex].connectedRegions;

        if (connectedRegions == null || connectedRegions.Length == 0)
        {
            currentMovePoint = null;
            currentState = EnemyState.Patrol;
            return;
        }

        int randomIndex = Random.Range(0, connectedRegions.Length);
        targetRegionIndex = connectedRegions[randomIndex];

        if (!IsValidRegionIndex(targetRegionIndex))
        {
            targetRegionIndex = -1;
            currentMovePoint = null;
            currentState = EnemyState.Patrol;
            return;
        }

        stateAfterTeleport = EnemyState.Patrol;
        currentState = EnemyState.MoveToTeleport;
    }

    private void MoveTo(Vector3 targetPosition, float speed)
    {
        Vector3 beforePosition = enemy.transform.position;

        float nextX = Mathf.MoveTowards(
            beforePosition.x,
            targetPosition.x,
            speed * Time.deltaTime
        );

        enemy.transform.position = new Vector3(nextX, beforePosition.y, beforePosition.z);

        float directionX = enemy.transform.position.x - beforePosition.x;

        if (enemySprite != null && Mathf.Abs(directionX) > 0.01f)
        {
            enemySprite.flipX = directionX < 0f;
        }
    }

    private float GetHorizontalDistance(Vector3 firstPosition, Vector3 secondPosition)
    {
        return Mathf.Abs(firstPosition.x - secondPosition.x);
    }

    private void PatrolManage()
    {
        if( currentMovePoint == null)
        {
            PickConnectedMovePoint();
            return;
        }

        if (isWaitingAtMovePoint)
        {
            isWaitingAtMovePoint = false;
        }

        MoveTo(currentMovePoint.position, patrolSpeed);

        float distance = GetHorizontalDistance(enemy.transform.position, currentMovePoint.position);

        if (distance <= movePointArriveDistance)
        {
            PickRandomTeleportTarget();
        }

    }

    private void UpdateChase()
    {
        if (playerRegionIndex != enemyRegionIndex)
        {
            targetRegionIndex = playerRegionIndex;
            stateAfterTeleport = EnemyState.Chase;
            currentState = EnemyState.MoveToTeleport;
            return;
        }

        float distance = GetHorizontalDistance(enemy.transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveTo(player.position, moveSpeed);
        }

        currentState = EnemyState.Chase;
    }


    private void UpdateMoveToTeleport()
    {
        if (!IsValidRegionIndex(enemyRegionIndex)) return;
        if (!IsValidRegionIndex(targetRegionIndex))
        {
            currentState = EnemyState.Patrol;
            return;
        }

        Transform targetTeleportPoint = regions[targetRegionIndex].teleportPoint;
        if (targetTeleportPoint == null) return;

        enemy.transform.position = targetTeleportPoint.position;
        enemyRegionIndex = targetRegionIndex;
        targetRegionIndex = -1;
        currentMovePoint = null;
        isWaitingAtMovePoint = false;

        if (stateAfterTeleport == EnemyState.Chase)
        {
            currentState = EnemyState.Chase;
            return;
        }

        currentState = EnemyState.MoveToPatrolPoint;
    }

    private void UpdateMoveToPatrolPoint()
    {
        if (!IsValidRegionIndex(enemyRegionIndex)) return;

        Transform patrolPoint = regions[enemyRegionIndex].patrolPoint;
        if (patrolPoint == null) return;

        MoveTo(patrolPoint.position, patrolSpeed);

        float distance = GetHorizontalDistance(enemy.transform.position, patrolPoint.position);

        if (distance <= movePointArriveDistance)
        {
            currentMovePoint = patrolPoint;
            isWaitingAtMovePoint = false;
            currentState = stateAfterTeleport;
            stateAfterTeleport = EnemyState.Patrol;
        }
    }

    public void StartChase()
    {
        targetRegionIndex = -1;
        currentMovePoint = null;
        isWaitingAtMovePoint = false;
        currentState = EnemyState.Chase;
    }

    public bool IsChasing()
    {
        return currentState == EnemyState.Chase || stateAfterTeleport == EnemyState.Chase;
    }

    public void StopChase()
    {
        targetRegionIndex = -1;
        currentMovePoint = null;
        isWaitingAtMovePoint = false;
        currentState = EnemyState.Patrol;
    }

    public void TeleportToRegionAndPatrol(int regionIndex)
    {
        if (!IsValidRegionIndex(regionIndex)) return;

        Transform teleportPoint = regions[regionIndex].teleportPoint;
        if (teleportPoint == null) return;

        enemy.transform.position = teleportPoint.position;
        enemyRegionIndex = regionIndex;
        targetRegionIndex = -1;
        currentMovePoint = null;
        isWaitingAtMovePoint = false;
        stateAfterTeleport = EnemyState.Patrol;
        currentState = EnemyState.Patrol;
    }

    public void SetPlayerRegion(int regionIndex)
    {
        if (!IsValidRegionIndex(regionIndex)) return;

        playerRegionIndex = regionIndex;
    }

    public void SetEnemyRegion(int regionIndex)
    {
        if (!IsValidRegionIndex(regionIndex)) return;

        enemyRegionIndex = regionIndex;
        targetRegionIndex = -1;
        currentMovePoint = null;
        isWaitingAtMovePoint = false;
    }

}
   
