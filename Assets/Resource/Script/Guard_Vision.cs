using UnityEngine;
using System.Collections; // 코루틴 사용

public class Guard_Vision : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private Transform visionBox;
    public float visionDistance = 8f;
    public float visionHeight = 2f;
    public int facing = -1;
    bool isLightOFF = false; // 암전 상태 아님 -> true : 암전

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

        visionBox.localPosition = new Vector3(xOffset, 0.5f, 0f);
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