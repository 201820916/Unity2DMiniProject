using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataPacketMiniGame : MonoBehaviour
{
    public static bool IsPlaying { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private GameObject dataPacketGroup;
    [SerializeField] private GameObject[] groupsToHide;

    [Header("Game Rule")]
    [SerializeField] private float timeLimit = 8f;
    [SerializeField] private bool failOnWrongClick = true;

    [Header("Player Control")]
    [SerializeField] private Character_move playerMovement;

    private Button[] packetButtons;
    private TMP_Text[] packetNumberTexts;
    private Action onSuccess;
    private Action onFail;
    private bool isPlaying;
    private float remainingTime;
    private int expectedNumber;
    private int[] packetNumbers;

    private void Start()
    {
        IsPlaying = false;

        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
        }

        if (dataPacketGroup != null)
        {
            dataPacketGroup.SetActive(false);
        }

        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<Character_move>();
        }

        CollectPacketButtons();
        BindPacketButtons();
    }

    private void Update()
    {
        if (!isPlaying) return;

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

    public void StartMiniGame(Action successCallback, Action failCallback)
    {
        if (isPlaying) return;

        onSuccess = successCallback;
        onFail = failCallback;

        isPlaying = true;
        remainingTime = timeLimit;
        expectedNumber = 1;

        SetPlayerControl(false);
        SetupPackets();
        SetOtherGroups(false);

        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
        }

        if (dataPacketGroup != null)
        {
            dataPacketGroup.SetActive(true);
        }

        UpdateTimerText();
    }

    private void CollectPacketButtons()
    {
        if (buttonParent == null)
        {
            packetButtons = Array.Empty<Button>();
            packetNumberTexts = Array.Empty<TMP_Text>();
            return;
        }

        packetButtons = buttonParent.GetComponentsInChildren<Button>(true);
        packetNumberTexts = new TMP_Text[packetButtons.Length];

        for (int i = 0; i < packetButtons.Length; i++)
        {
            packetNumberTexts[i] = packetButtons[i].GetComponentInChildren<TMP_Text>(true);
        }
    }

    private void BindPacketButtons()
    {
        if (packetButtons == null) return;

        for (int i = 0; i < packetButtons.Length; i++)
        {
            int buttonIndex = i;

            if (packetButtons[i] != null)
            {
                packetButtons[i].onClick.RemoveAllListeners();
                packetButtons[i].onClick.AddListener(() => OnPacketClicked(buttonIndex));
            }
        }
    }

    private void SetupPackets()
    {
        int count = packetButtons == null ? 0 : packetButtons.Length;
        packetNumbers = new int[count];

        for (int i = 0; i < count; i++)
        {
            packetNumbers[i] = i + 1;
        }

        Shuffle(packetNumbers);

        for (int i = 0; i < count; i++)
        {
            if (packetButtons[i] != null)
            {
                packetButtons[i].interactable = true;
                packetButtons[i].gameObject.SetActive(true);
            }

            if (i < packetNumberTexts.Length && packetNumberTexts[i] != null)
            {
                packetNumberTexts[i].text = packetNumbers[i].ToString();
            }
        }
    }

    private void OnPacketClicked(int buttonIndex)
    {
        if (!isPlaying) return;
        if (packetNumbers == null || buttonIndex < 0 || buttonIndex >= packetNumbers.Length) return;

        int clickedNumber = packetNumbers[buttonIndex];

        if (clickedNumber == expectedNumber)
        {
            if (packetButtons[buttonIndex] != null)
            {
                packetButtons[buttonIndex].interactable = false;
            }

            expectedNumber++;

            if (expectedNumber > packetNumbers.Length)
            {
                Success();
            }

            return;
        }

        if (failOnWrongClick)
        {
            Fail();
        }
        else
        {
            ResetSelection();
        }
    }

    private void ResetSelection()
    {
        expectedNumber = 1;

        if (packetButtons == null) return;

        foreach (Button packetButton in packetButtons)
        {
            if (packetButton != null)
            {
                packetButton.interactable = true;
            }
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        timerText.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    private void Success()
    {
        EndMiniGame();
        onSuccess?.Invoke();
    }

    private void Fail()
    {
        EndMiniGame();
        onFail?.Invoke();
    }

    private void EndMiniGame()
    {
        isPlaying = false;
        IsPlaying = false;
        SetPlayerControl(true);

        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
        }

        if (dataPacketGroup != null)
        {
            dataPacketGroup.SetActive(false);
        }
    }

    private void SetOtherGroups(bool active)
    {
        if (groupsToHide == null) return;

        foreach (GameObject group in groupsToHide)
        {
            if (group != null)
            {
                group.SetActive(active);
            }
        }
    }

    private void SetPlayerControl(bool canControl)
    {
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(canControl);
        }
    }

    private void Shuffle(int[] numbers)
    {
        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (numbers[i], numbers[randomIndex]) = (numbers[randomIndex], numbers[i]);
        }
    }
}
