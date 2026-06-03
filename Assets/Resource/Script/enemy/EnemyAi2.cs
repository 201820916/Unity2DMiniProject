using UnityEngine;

public class EnemyAi2 : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private Animator enemyAnimator;

    [Header("State")]
    [SerializeField] private EnemyState currentState = EnemyState.Patrol;

    // 3x3 배치에서는 X좌표만으로 지역을 구분할 수 없어 지역 번호를 따로 저장합니다.
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
    private int pendingTeleportDestinationRegionIndex = -1;
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
        // PickConnectedMovePoint();
    }

}
    /*
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
    
    private void PatrolManage()
    {
        if ( currentMovePoint == null )
        {
            PickConnectedMovePoint();
            SetMoving(false);
            return;
        }

        if (isWatingAtMovePoint)
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
    }

    private void UpdateChase()
    {

    }

    private void UpdateMoveToTeleport()
    {

    }

    private void UpdateMoveToPatrolPoint()
    {

    }

    



    private void PickConnectedMovePoint()
    {

    }


}
    */