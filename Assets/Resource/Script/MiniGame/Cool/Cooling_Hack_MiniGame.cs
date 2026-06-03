using TMPro;
using UnityEngine;
using System;

public class Cooling_Hack_MiniGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject CoolNode;
    [SerializeField] private GameObject MiniGamePanel;
    [SerializeField] private GameObject coolingGameGroup;
    [SerializeField] private TMP_Text Count;
    [SerializeField] private RectTransform needle;
    [SerializeField] private float minNeedleAngle = 90f;
    [SerializeField] private float maxNeedleAngle = -90f;
    [SerializeField] private Character_move playerMovement;

    [SerializeField] private float pressureUpSpeed = 0.8f;
    [SerializeField] private float pressureDownSpeed = 0.6f;

    private float pressure = 0f;
    private bool isPlaying;
    private Action onSuccess;


    [SerializeField] private float stableMinPressure = 45f;
    [SerializeField] private float stableMaxPressure = 60f;
    [SerializeField] private float requiredStableTime = 3f;
    [SerializeField] private float unstableDrainSpeed = 1f;

    private float stableTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        pressure = 0.5f;
        stableTimer = 0f;
        UpdateNeedleUI();
        UpdatePressureUI();


        isPlaying = false;

        if (MiniGamePanel != null)
        {
            MiniGamePanel.SetActive(false);
        }

        if (coolingGameGroup != null)
        {
            coolingGameGroup.SetActive(false);
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
        UpdateStableCheck();
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

        if (coolingGameGroup != null)
        {
            coolingGameGroup.SetActive(true);
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

    private void UpdateStableCheck()
    {
        float currentPressure = pressure * 100f;
        bool isStable = currentPressure >= stableMinPressure &&
                        currentPressure <= stableMaxPressure;

        if (isStable)
        {
            stableTimer += Time.deltaTime;

            if (stableTimer >= requiredStableTime)
            {
                Success();
            }
        }
        else
        {
            stableTimer = Mathf.Max(0f, stableTimer - unstableDrainSpeed * Time.deltaTime);
        }
    }

    private void UpdatePressureUI()
    {
        if (Count == null) return;

        float currentPressure = pressure * 100f;
        Count.text =
            "COOLANT PRESSURE\n" +
            "LOW    STABLE    DANGER\n" +
            currentPressure.ToString("0") + " PSI\n" +
            "STABLE " + stableTimer.ToString("0.0") + " / " + requiredStableTime.ToString("0.0");
    }

    private void Success()
    {
        isPlaying = false;
        SetPlayerControl(true);
        if (MiniGamePanel != null)
        {
            MiniGamePanel.SetActive(false);
        }
        if (coolingGameGroup != null)
        {
            coolingGameGroup.SetActive(false);
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

