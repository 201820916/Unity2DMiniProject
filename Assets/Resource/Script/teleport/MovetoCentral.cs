using UnityEngine;

public class MovetoCentral : MonoBehaviour
{
    [SerializeField] GameObject StartDoor;
    [SerializeField] GameObject StartDestination;

    [SerializeField] GameObject ReturnDoor;
    [SerializeField] GameObject ReturnDestination;

    [SerializeField] GameObject Main_Character;
    [SerializeField] float teleportRange = 1.5f;

    private void UseTeleport()
    {
        float distanceToStartDoor = Vector2.Distance(
            Main_Character.transform.position,
            StartDoor.transform.position
        );

        float distanceToReturnDoor = Vector2.Distance(
            Main_Character.transform.position,
            ReturnDoor.transform.position
        );

        Debug.Log("StartDoor 거리: " + distanceToStartDoor);
        Debug.Log("ReturnDoor 거리: " + distanceToReturnDoor);

        if (distanceToStartDoor <= teleportRange)
        {
            Main_Character.transform.position = StartDestination.transform.position;
            Debug.Log("중앙통제실로 이동");
        }
        else if (distanceToReturnDoor <= teleportRange)
        {
            Main_Character.transform.position = ReturnDestination.transform.position;
            Debug.Log("원래 장소로 이동");
        }
        else
        {
            Debug.Log("텔레포트 범위 밖임");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseTeleport();
        }
    }
}