using UnityEngine;

[System.Serializable]
public class RegionMovePoints
{
    // Inspector에서 지역을 구분하기 위한 이름입니다.
    public string regionName;

    // 해당 지역에서 추적자가 도착하거나 대기할 기준 위치입니다.
    public Transform point;

    // 이 지역에서 이동 가능한 다음 지역 번호입니다.
    // Unity 배열 기준이라 1번 지역은 0, 2번 지역은 1처럼 0부터 입력합니다.
    public int[] connectedRegions;
}

public class EnemyAI : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private Animator enemyAnimator;

    [Header("State")]
    [SerializeField] private EnemyState currentState = EnemyState.Patrol;

    // 3x3 배치에서는 X좌표만으로 지역을 구분할 수 없어서 지역 번호를 따로 저장합니다.
    [SerializeField] private int playerRegionIndex;
    [SerializeField] private int enemyRegionIndex;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float patrolSpeed = 1.2f;
    [SerializeField] private float searchWaitTime = 1.5f;
    [SerializeField] private float movePointArriveDistance = 0.15f;

    [Header("MovePoint")]
    // 9개 지역의 이동 기준점과 연결 정보를 넣습니다.
    // regions[0]은 1번 지역, regions[8]은 9번 지역입니다.
    [SerializeField] private RegionMovePoints[] regions;

    private Transform currentMovePoint;
    private int currentMovePointRegionIndex;
    private float waitTimer;
    private bool isWaitingAtMovePoint;

    public Vector3 PlayerPosition { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

    public enum EnemyState
    {
        Patrol,
        Chase
    }

    private void Start()
    {
        currentState = EnemyState.Patrol;
        PickConnectedMovePoint();
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
        }
    }

    private Transform GetRegionPoint(int regionIndex)
    {
        // 잘못된 지역 번호가 들어오면 포인트를 찾지 않습니다.
        if (regions == null || regions.Length == 0) return null;
        if (regionIndex < 0 || regionIndex >= regions.Length) return null;

        return regions[regionIndex].point;
    }

    private int[] GetConnectedRegions(int regionIndex)
    {
        // 현재 지역에서 이동 가능한 지역 목록을 가져옵니다.
        if (regions == null || regions.Length == 0) return null;
        if (regionIndex < 0 || regionIndex >= regions.Length) return null;

        return regions[regionIndex].connectedRegions;
    }

    private void PickConnectedMovePoint()
    {
        // 현재 추적자 지역에서 실제로 연결된 지역 중 하나를 순찰 목표로 고릅니다.
        int[] connectedRegions = GetConnectedRegions(enemyRegionIndex);

        if (connectedRegions == null || connectedRegions.Length == 0)
        {
            currentMovePoint = null;
            return;
        }

        for (int i = 0; i < 20; i++)
        {
            int randomIndex = Random.Range(0, connectedRegions.Length);
            int randomRegionIndex = connectedRegions[randomIndex];
            Transform randomPoint = GetRegionPoint(randomRegionIndex);

            if (randomPoint == null) continue;

            currentMovePoint = randomPoint;
            currentMovePointRegionIndex = randomRegionIndex;
            return;
        }

        currentMovePoint = null;
    }

    public void PatrolManage()
    {
        // 목표 포인트가 없으면 현재 지역의 연결 목록에서 다시 목표를 고릅니다.
        if (currentMovePoint == null)
        {
            PickConnectedMovePoint();
            SetMoving(false);
            return;
        }

        // 포인트에 도착한 뒤 잠깐 대기하는 상태입니다.
        if (isWaitingAtMovePoint)
        {
            waitTimer -= Time.deltaTime;
            SetMoving(false);

            if (waitTimer <= 0f)
            {
                isWaitingAtMovePoint = false;
                PickConnectedMovePoint();
            }

            return;
        }

        // 목표 지역이 현재 지역과 다르면 연결된 지역으로 이동합니다.
        // 지금 구조에서는 지역 사이 이동을 텔레포트로 처리합니다.
        if (currentMovePointRegionIndex != enemyRegionIndex)
        {
            UseTeleport(currentMovePointRegionIndex);
            return;
        }

        // 같은 지역 안에서는 Y축 이동 없이 X축만 보고 목표 포인트까지 이동합니다.
        float distance = Mathf.Abs(EnemyPosition.x - currentMovePoint.position.x);

        if (distance <= movePointArriveDistance)
        {
            waitTimer = searchWaitTime;
            isWaitingAtMovePoint = true;
            SetMoving(false);
            return;
        }

        float direction = Mathf.Sign(currentMovePoint.position.x - EnemyPosition.x);

        SetDirection(direction);
        SetMoving(true);

        enemy.transform.position += new Vector3(direction * patrolSpeed * Time.deltaTime, 0f, 0f);
    }

    private void UpdateChase()
    {
        // 플레이어와 다른 지역에 있으면 플레이어가 있는 지역 방향으로 한 칸씩 이동합니다.
        if (enemyRegionIndex != playerRegionIndex)
        {
            int nextRegionIndex = FindNextRegionTowardPlayer();

            if (nextRegionIndex != -1)
            {
                UseTeleport(nextRegionIndex);
            }
            else
            {
                SetMoving(false);
            }

            return;
        }

        // 같은 지역 안에 있을 때만 X축으로 플레이어를 추적합니다.
        float distance = Mathf.Abs(PlayerPosition.x - EnemyPosition.x);

        if (distance <= stopDistance)
        {
            SetMoving(false);
            return;
        }

        float direction = Mathf.Sign(PlayerPosition.x - EnemyPosition.x);

        SetDirection(direction);
        SetMoving(true);

        enemy.transform.position += new Vector3(direction * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    private int FindNextRegionTowardPlayer()
    {
        // 지금 연결 구조에서는 모든 경로가 2, 5, 8을 거치는 십자형입니다.
        // 그래서 간단한 BFS로 플레이어 지역까지 가는 다음 한 칸을 찾습니다.
        if (regions == null || regions.Length == 0) return -1;
        if (enemyRegionIndex < 0 || enemyRegionIndex >= regions.Length) return -1;
        if (playerRegionIndex < 0 || playerRegionIndex >= regions.Length) return -1;

        int[] queue = new int[regions.Length];
        int[] previous = new int[regions.Length];
        bool[] visited = new bool[regions.Length];

        for (int i = 0; i < previous.Length; i++)
        {
            previous[i] = -1;
        }

        int head = 0;
        int tail = 0;

        queue[tail] = enemyRegionIndex;
        tail++;
        visited[enemyRegionIndex] = true;

        while (head < tail)
        {
            int currentRegionIndex = queue[head];
            head++;

            if (currentRegionIndex == playerRegionIndex)
            {
                break;
            }

            int[] connectedRegions = GetConnectedRegions(currentRegionIndex);
            if (connectedRegions == null) continue;

            for (int i = 0; i < connectedRegions.Length; i++)
            {
                int nextRegionIndex = connectedRegions[i];

                if (nextRegionIndex < 0 || nextRegionIndex >= regions.Length) continue;
                if (visited[nextRegionIndex]) continue;

                visited[nextRegionIndex] = true;
                previous[nextRegionIndex] = currentRegionIndex;
                queue[tail] = nextRegionIndex;
                tail++;
            }
        }

        if (!visited[playerRegionIndex])
        {
            return -1;
        }

        int nextStep = playerRegionIndex;

        while (previous[nextStep] != enemyRegionIndex)
        {
            nextStep = previous[nextStep];

            if (nextStep == -1)
            {
                return -1;
            }
        }

        return nextStep;
    }

    public void StartChase()
    {
        // CCTV 감지, 시야 감지, 해킹 실패 등에서 호출해서 추적 상태로 전환합니다.
        currentState = EnemyState.Chase;
        isWaitingAtMovePoint = false;
    }

    public void StopChase()
    {
        // 추적을 멈추면 다시 연결된 지역을 따라 무작위 순찰을 시작합니다.
        currentState = EnemyState.Patrol;
        isWaitingAtMovePoint = false;
        PickConnectedMovePoint();
    }

    public void SetPlayerRegion(int regionIndex)
    {
        // 플레이어가 텔레포트로 지역 이동했을 때 LiftTeleport에서 호출합니다.
        playerRegionIndex = regionIndex;
    }

    public void SetEnemyRegion(int regionIndex)
    {
        // 필요할 때 추적자의 현재 지역을 직접 맞춰주기 위한 함수입니다.
        enemyRegionIndex = regionIndex;
        isWaitingAtMovePoint = false;
        PickConnectedMovePoint();
    }

    public void UseTeleport(int destinationRegionIndex)
    {
        // 추적자가 연결된 다른 지역으로 이동해야 할 때 해당 지역의 기준 포인트로 텔레포트합니다.
        if (enemy == null) return;
        if (regions == null || regions.Length == 0) return;
        if (destinationRegionIndex < 0 || destinationRegionIndex >= regions.Length) return;

        Transform destinationPoint = GetRegionPoint(destinationRegionIndex);
        if (destinationPoint == null) return;

        enemyRegionIndex = destinationRegionIndex;
        currentMovePoint = destinationPoint;
        currentMovePointRegionIndex = destinationRegionIndex;

        enemy.transform.position = destinationPoint.position;
        waitTimer = searchWaitTime;
        isWaitingAtMovePoint = true;
        SetMoving(false);
    }

    private void SetDirection(float direction)
    {
        if (enemySprite != null)
        {
            // 스프라이트 기본 방향이 반대라면 direction < 0f를 direction > 0f로 바꾸면 됩니다.
            enemySprite.flipX = direction < 0f;
        }
    }

    private void SetMoving(bool isMoving)
    {
        // Animator에 isMoving bool 파라미터가 있을 때 이동 애니메이션을 제어합니다.
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("isMoving", isMoving);
        }
    }
}
