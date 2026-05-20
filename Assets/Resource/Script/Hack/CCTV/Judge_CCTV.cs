using UnityEngine;

public class Judge_CCTV : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private SpriteRenderer CCTV_Judge;
    [SerializeField] private Sprite Guard_A;
    [SerializeField] private Sprite Guard_Q;

    [Header("Detection")]
    public string state = "normal";
    public float detection = 0f;
    public float detectionIncrease = 60f;
    public float detectionDecrease = 40f;

    private bool isPlayerInVision;

    void Start()
    {
        CCTV_Judge.sprite = null;
        state = "normal";
        isPlayerInVision = false;
    }

    void Update()
    {
        UpdateDetection();
        UpdateState();
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
        if (CCTV_Judge == null) return;

        if (detection >= 80f)
        {
            CCTV_Judge.sprite = Guard_A;
            state = "alert";
        }
        else if (detection >= 30f)
        {
            CCTV_Judge.sprite = Guard_Q;
            state = "warning";
        }
        else
        {
            CCTV_Judge.sprite = null;
            state = "normal";
        }
    }

    public void SetPlayerInVision(bool value)
    {
        isPlayerInVision = value;
    }
}