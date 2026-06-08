using TMPro;
using UnityEngine;
using System;

public class PowerRoom_MiniGame : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject PowerNode;
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
    [SerializeField] private int Sucesscount = 0;
    [SerializeField] private float QuestionNumber = 0;
    [SerializeField] private int requiredSuccessCount = 3;
    [SerializeField] private float answerTolerance = 5f;
    [SerializeField] private int minQuestionNumber = 10;
    [SerializeField] private int maxQuestionNumber = 90;

    private float pressure = 0f;
    private bool isPlaying;
    private float score;
    private Action onSuccess;
    private Action onFail;
    private float answer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        pressure = 0.5f;
        score = 0f;
        UpdateNeedleUI();


        isPlaying = false;

        if (MiniGamePanel != null)
        {
            MiniGamePanel.SetActive(false);
        }

        if (PowerGameGroup != null )
        {
            PowerGameGroup.SetActive(false);
        }

        score = 0f;

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
        UpdateQuestionUI();

        if (Input.GetKeyDown(KeyCode.F))
        {
            SubmitPressure();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Fail();
        }
    }

    public void StartMiniGame(Action successCallback, Action failCallback)
    {
        if (isPlaying) return;

        onSuccess = successCallback;
        onFail = failCallback;

        SetPlayerControl(false);
        pressure = 0.5f;
        Sucesscount = 0;
        answer = 0f;
        SetNewQuestion();
        UpdateNeedleUI();
        UpdateQuestionUI();

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

    private void SetNewQuestion()
    {
        QuestionNumber = UnityEngine.Random.Range(minQuestionNumber, maxQuestionNumber + 1);
    }

    private void UpdateQuestionUI()
    {
        if (Question != null)
        {
            Question.text = "TARGET : " + QuestionNumber.ToString("0");
        }

        if (Count != null)
        {
            Count.text = Sucesscount + " / " + requiredSuccessCount;
        }

        if (submit != null)
        {
            submit.text = "your answer : " + answer.ToString("0");
        }
    }

    private void SubmitPressure()
    {
        float currentPressure = pressure * 100f;
        answer = currentPressure;
        UpdateQuestionUI();

        float difference = Mathf.Abs(currentPressure - QuestionNumber);

        if (difference <= answerTolerance)
        {
            Sucesscount++;

            if (Sucesscount >= requiredSuccessCount)
            {
                Success();
                return;
            }

            SetNewQuestion();
            UpdateQuestionUI();
        }
        else
        {
            UpdateQuestionUI();
        }
    }

    private void Success()
    {
        isPlaying = false;
        SetPlayerControl(true);
        if (MiniGamePanel != null)
        {
            MiniGamePanel.SetActive(false);
        }
        PowerGameGroup.SetActive(false);
        onSuccess?.Invoke();
    }

    private void Fail()
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

        onFail?.Invoke();
    }

    private void SetPlayerControl(bool canControl)
    {
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(canControl);
        }
    }

}
