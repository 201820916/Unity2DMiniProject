using UnityEngine;
using System;
using TMPro;

public class HackMiniGame : MonoBehaviour
{
    public static bool IsPlaying { get; private set; }

    [SerializeField] private GameObject miniGamePanel;

    [SerializeField] private Character_move playerMovement;

    // 플레이어가 따라 입력해야 할 문장을 보여줌.
    [SerializeField] private TMP_Text QuestionText;

    // 플레이어가 문장을 입력할 필드.
    [SerializeField] private TMP_InputField inputField;

    // 남은 시간
    [SerializeField] private TMP_Text timerText;

    // 결과 출력
    [SerializeField] private TMP_Text resultText;

    // 제한시간 설정
    [SerializeField] private float timeLimit = 8f;

    // 이 배열에 있는 문장 중 하나가 랜덤으로 선택
    [TextArea]
    [SerializeField] private string[] hackSentences =
    {
        "ACCESS NODE",
        "DISABLE LIGHT",
        "BYPASS SECURITY",
        "OPEN CIRCUIT",
        "SYSTEM OVERRIDE",
        "TRACE SIGNAL",
        "UNLOCK PANEL",
        "CUT POWER",
        "RESET CAMERA",
        "UPLOAD KEY"
    };


    private Action onSuccess; // 성공 콜백
    private Action onFail; // 실패 콜백
    private string currentSentence; // 제시된 문구
    private float remainingTime; // 제한 시간
    

    void Start()
    {
        if (miniGamePanel != null)
        {
            miniGamePanel.SetActive(false);
        }
        IsPlaying = false;

        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<Character_move>();
        }

        if (inputField != null)
        {
            inputField.onValueChanged.AddListener(CheckInput);
        }
    }

    public void StartMiniGame(Action successCallback, Action failCallback)
    {
        if (IsPlaying) return;

        onSuccess = successCallback;
        onFail = failCallback;

        IsPlaying = true;
        remainingTime = timeLimit;
        currentSentence = GetRandomSentence();

        SetPlayerControl(false);
        if (miniGamePanel != null)
        {
            miniGamePanel.SetActive(true);
        }

        if (QuestionText != null)
        {
            QuestionText.text = currentSentence;
        }

        if (inputField != null)
        {
            // 이전 입력을 지우고 바로 입력할 수 있게 포커스를 줍니다.
            inputField.text = string.Empty;
            inputField.ActivateInputField();
            inputField.Select();
        }

        if (resultText != null)
        {
            resultText.text = string.Empty;
        }

        UpdateTimerText();
    }

    void Update()
    {
        if (!IsPlaying) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerText();

        if (remainingTime <= 0f)
        {
            Fail();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Fail();
        }
    }

    private string GetRandomSentence()
    {
        if (hackSentences == null || hackSentences.Length == 0)
        {
            return "ACCESS NODE";
        }

        int index = UnityEngine.Random.Range(0, hackSentences.Length);
        return hackSentences[index];
    }

    private void CheckInput(string input)
    {
        if (!IsPlaying) return;

        if (string.Equals(input, currentSentence))
        {
            Success();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        timerText.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    private void Success()
    {
        IsPlaying = false;
        SetPlayerControl(true);
        if (miniGamePanel != null)
        {
            miniGamePanel.SetActive(false);
        }
        onSuccess?.Invoke();
    }

    private void Fail()
    {
        IsPlaying = false;
        SetPlayerControl(true);

        if (resultText != null)
        {
            resultText.text = "FAILED";
        }

        if (miniGamePanel != null)
        {
            miniGamePanel.SetActive(false);
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
