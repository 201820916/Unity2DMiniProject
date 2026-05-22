using UnityEngine;
using UnityEngine.UIElements;

public class HackDroneShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint; // 발사 위치(플레이어 손목?)
    [SerializeField] private float range = 50f; // 유효범위
    [SerializeField] private LayerMask cctvLayer; // RayCast는 cctvLayer만 맞춤.
    private Vector2 aimDirection = Vector2.right; // RayCast 발사 방향

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAimDirection();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShootHackDrone();
        }
    }


    private void UpdateAimDirection()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector2 direction = mouseWorldPos - firePoint.position;
        aimDirection = direction.normalized;
    }

    private void ShootHackDrone()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            firePoint.position,
            aimDirection,
            range,
            cctvLayer
        );

        Debug.DrawRay(firePoint.position, aimDirection * range, Color.cyan, 0.5f);

        if (hit.collider != null) // RayCast 충돌
        {
            Judge_CCTV cctv = hit.collider.GetComponentInParent<Judge_CCTV>();
            // 피격받은 오브젝트에 Judge_CCTV 컴포넌트가 있는가?

            if (cctv != null) // 있다면
            {
                Debug.Log("CCTV 해킹 드론 명중");
                cctv.DisableCCTV(4f);
            }
        }
        else
        {
            Debug.Log("해킹 드론 빗나감");
        }
    }
}