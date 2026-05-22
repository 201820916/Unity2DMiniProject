using UnityEngine;
using System.Collections; // 코루틴 사용

public class Guard_Vision : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private Transform visionBox;
    public float visionDistance = 12f;
    public float visionHeight = 12f;
    public int facing = 1; // 1 = 오른쪽, -1 = 왼쪽
    bool isLightOFF = false; // 암전 상태 아님 -> true : 암전
    [SerializeField] private SpriteRenderer SR;

    [Header("Detection")]
    public float detection = 0f;
    public float detectionIncrease = 60f;
    public float detectionDecrease = 40f;

    public string state = "normal";

    private bool isPlayerInVision;

    void Update()
    {
        if (isLightOFF)
        {
            return;
        }
        UpdateVisionBox();
        UpdateFacingVisual();
        UpdateDetection();
        UpdateState();
    }

    private void UpdateVisionBox()
    {
        if (visionBox == null)
        {
            Debug.Log("VisionBox가 없습니다.");
            return;
        }

        visionBox.localScale = new Vector3(visionDistance, visionHeight, 1f);

        float xOffset;

        if (facing == 1)
        {
            xOffset = visionDistance / 2f;
        }
        else
        {
            xOffset = -visionDistance / 2f;
        }

        visionBox.localPosition = new Vector3(xOffset, 1.5f, 0f);
        
    }
    private void UpdateFacingVisual()
    {
        if (SR == null) return;

        SR.flipX = facing == -1;
    }

    public void SetPlayerInVision(bool value)
    {
        isPlayerInVision = value;
    }

    private void UpdateDetection()
    {
        if (isPlayerInVision)
        {
            detection += detectionIncrease * Time.deltaTime;
        }
        else
        {
            detection -= detectionDecrease * Time.deltaTime;
        }

        detection = Mathf.Clamp(detection, 0f, 100f);
    }

    private void UpdateState()
    {
        if (detection >= 80f)
        {
            state = "alert";
        }
        else if (detection >= 30f)
        {
            state = "warning";
        }
        else
        {
            state = "normal";
        }
    }

    // 여기서부터는 노드해킹 - 암전의 상호작용
    public void LightOFF(float duration)
    {
        StartCoroutine(LightOFFRoutine(duration));
    }

    private IEnumerator LightOFFRoutine(float duration)
    {
        isLightOFF = true;
        state = "blind";
        detection = 0f;

        if(visionBox != null)
        {
            visionBox.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(duration);

        isLightOFF = false;
        state = "normal";

        if (visionBox != null)
        {
            visionBox.gameObject.SetActive(true);
        }
    }
}