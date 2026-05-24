using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueData
{
    public DialogueEntry[] dialogues;
}

[System.Serializable]
public class DialogueEntry
{
    public string id;
    public string[] lines;
}

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Data")]
    [SerializeField] private TextAsset dialogueJson;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBubble;
    [SerializeField] private TextMeshPro dialogueText;

    private DialogueData dialogueData;
    private string[] currentLines;
    private int currentIndex;
    private bool isDialogueActive;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (dialogueBubble != null)
        {
            dialogueBubble.SetActive(false);
        }

        LoadDialogueJson();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDialogueActive) return;

        if(Input.GetKeyDown(KeyCode.E))
        {
            ShowNextLine();
        }
    }



    private void LoadDialogueJson()
    {
        if(dialogueJson == null)
        {
            Debug.Log("Dialogue JSON 파일이 연결되지 않았습니다.");
            return;
        }

        dialogueData = JsonUtility.FromJson<DialogueData>(dialogueJson.text);
    }

    private DialogueEntry FindDialogue(string dialogueId)
    {
        if (dialogueData == null || dialogueData.dialogues == null)
        {
            return null;
        }

        foreach (DialogueEntry entry in dialogueData.dialogues)
        {
            if (entry.id == dialogueId)
            {
                return entry;
            }
        }

        return null;
    }

    public void StartDialogue(string dialogueId)
    {
        DialogueEntry entry = FindDialogue(dialogueId);

        if(entry == null)
        {
            Debug.Log("대화 ID를 찾을 수 없습니다: " + dialogueId);
            return;
        }

        currentLines = entry.lines;
        currentIndex = 0;
        isDialogueActive = true;

        dialogueBubble.SetActive(true);
        dialogueText.text = currentLines[currentIndex].ToString();

    }

    private void ShowNextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            EndDialogue();

            return;
        }

        dialogueText.text = currentLines[currentIndex];
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueBubble.SetActive(false);
    }
}
