using UnityEngine;
using System;
using TMPro;

public class HackMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject miniGamePanel;

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


    // 이 배열에 있는 문장 중 하나가 랜덤으로 선택됩니다. Inspector에서 수정할 수 있습니다.
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

    private Action onSuccess;
    private Action onFail;
    private bool isPlaying;
    private string currentSentence;
    private float remainingTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        miniGamePanel.SetActive(false);

        if (inputField != null)
        {
            // 입력칸의 내용이 바뀔 때마다 정답과 같은지 검사합니다.
            inputField.onValueChanged.AddListener(CheckInput);
        }
    }

    public void StartMiniGame(Action successCallback, Action failCallback)
    {
        onSuccess = successCallback;
        onFail = failCallback;

        isPlaying = true;
        remainingTime = timeLimit;
        currentSentence = GetRandomSentence();

        miniGamePanel.SetActive(true);

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

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        // 미니게임이 진행 중일 때만 제한시간을 줄입니다.
        remainingTime -= Time.deltaTime;
        UpdateTimerText();

        if (remainingTime <= 0f)
        {
            // 시간이 끝나면 실패 처리합니다.
            Fail();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Esc를 누르면 미니게임을 취소/실패 처리합니다.
            Fail();
        }
    }

    private string GetRandomSentence()
    {
        if (hackSentences == null || hackSentences.Length == 0)
        {
            return "ACCESS NODE";
        }
        // 문제 발생시 ACCESS NODE 출력되도록 세팅


        int index = UnityEngine.Random.Range(0, hackSentences.Length);
        // System에도 Random이 있고, UnityEngine에도 랜덤이 있다.
        // 어디의 Random을 사용해야할지 명시

        return hackSentences[index];
    }

    private void CheckInput(string input)
    {
        if (!isPlaying) return;

        if (string.Equals(input, currentSentence))
        {
            Success();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        // Mathf.CeilToInt = 소수점 올림
    }

    private void Success()
    {
        isPlaying = false;
        miniGamePanel.SetActive(false);

        // Hack_Node의 OnMiniGameSuccess가 여기서 실행됩니다.
        onSuccess?.Invoke();
    }

    private void Fail()
    {
        isPlaying = false;

        if (resultText != null)
        {
            resultText.text = "FAILED";
        }

        miniGamePanel.SetActive(false);
        onFail?.Invoke();
    }
}
