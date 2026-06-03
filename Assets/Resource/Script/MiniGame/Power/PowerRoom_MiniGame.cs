using TMPro;
using UnityEngine;
using System;

public class PowerRoom_MiniGame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject CoolNode;
    [SerializeField] private GameObject MiniGamePanel;
    [SerializeField] private GameObject PowerGameGroup;
    [SerializeField] private TMP_Text Count;
    [SerializeField] private TMP_Text Question;
    [SerializeField] private TMP_Text submit;
    [SerializeField] private RectTransform needle;
    [SerializeField] private float minNeedleAngle = 90f;
    [SerializeField] private float maxNeedleAngle = -90f;
    [SerializeField] private Character_move playerMovement;

    [Header("Game Rule")]
    [SerializeField] private float pressureUpSpeed = 0.8f;
    [SerializeField] private float pressureDownSpeed = 0.6f;
    [SerializeField] private float stableMinPressure = 45f;
    [SerializeField] private float stableMaxPressure = 60f;
    [SerializeField] private float requiredStableTime = 3f;
    [SerializeField] private float unstableDrainSpeed = 1f;

    private float pressure = 0f;
    private float stableTimer = 0f;
    private bool isPlaying;
    private Action onSuccess;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        pressure = 0.5f;
        stableTimer = 0f;
        UpdateNeedleUI();
        UpdatePressureUI();


        isPlaying = false;

        if ( MiniGamePanel != null)
        {
            MiniGamePanel.SetActive(false);
        }

        if (PowerGameGroup != null )
        {
            PowerGameGroup.SetActive(false);
        }

        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<Character_move>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        UpdatePressure();
        UpdateNeedleUI();
        UpdateStableTimer();
        UpdatePressureUI();
    }

    public void StartMiniGame(Action successCallback, Action failCallback)
    {
        if (isPlaying) return;

        onSuccess = successCallback;

        SetPlayerControl(false);
        pressure = 0.5f;
        stableTimer = 0f;
        UpdateNeedleUI();
        UpdatePressureUI();

        isPlaying = true;

        if (MiniGamePanel != null)
        {
            MiniGamePanel.SetActive(true);
        }

        if (PowerGameGroup != null)
        {
            PowerGameGroup.SetActive(true);
        }

    }

    private void UpdatePressure()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            pressure += pressureUpSpeed * Time.deltaTime;
        }

        else
        {
            pressure -= pressureDownSpeed * Time.deltaTime;
        }

        pressure = Mathf.Clamp01(pressure);
    }

    private void UpdateNeedleUI()
    {
        if (needle == null) return;

        float angle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, pressure);

        needle.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void UpdateStableTimer()
    {
        float currentPressure = GetCurrentPressure();
        bool isStable = currentPressure >= stableMinPressure && currentPressure <= stableMaxPressure;

        if (isStable)
        {
            stableTimer += Time.deltaTime;

            if (stableTimer >= requiredStableTime)
            {
                Success();
            }

            return;
        }

        stableTimer = Mathf.Max(0f, stableTimer - unstableDrainSpeed * Time.deltaTime);
    }

    private void UpdatePressureUI()
    {
        float currentPressure = GetCurrentPressure();

        if (Question != null)
        {
            Question.text = "COOLANT PRESSURE";
        }

        if (Count != null)
        {
            Count.text = "STABLE " + stableTimer.ToString("0.0") + " / " + requiredStableTime.ToString("0.0");
        }

        if (submit != null)
        {
            submit.text = currentPressure.ToString("0") + " PSI";
        }
    }

    private float GetCurrentPressure()
    {
        return pressure * 100f;
    }

    private void Success()
    {
        isPlaying = false;
        SetPlayerControl(true);
        if (MiniGamePanel != null)
        {
            MiniGamePanel.SetActive(false);
        }
        if (PowerGameGroup != null)
        {
            PowerGameGroup.SetActive(false);
        }
        onSuccess?.Invoke();
    }

    private void SetPlayerControl(bool canControl)
    {
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(canControl);
        }
    }

}
