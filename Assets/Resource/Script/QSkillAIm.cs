using UnityEngine;

public class QSkillAim : MonoBehaviour
{
    [SerializeField] private GameObject aimMarker;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        aimMarker.transform.position = mouseWorldPos;
    }
}