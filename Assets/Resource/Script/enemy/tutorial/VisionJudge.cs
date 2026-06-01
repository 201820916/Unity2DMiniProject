using UnityEngine;

public class VisionJudge : MonoBehaviour
{
    [SerializeField] private SpriteRenderer visionJudgeRenderer;
    [SerializeField] private Guard_Vision GV;
    [SerializeField] private Sprite Guard_A;
    [SerializeField] private Sprite Guard_Q;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (visionJudgeRenderer != null)
        {
            visionJudgeRenderer.sprite = null;
        }
    }

    private void UpdateState()
    {
        if ( GV.state == "alert")
        {
            visionJudgeRenderer.sprite = Guard_A;
        }

        else if ( GV.state == "warning")
        {
            visionJudgeRenderer.sprite = Guard_Q;
        }

        else
        {
            visionJudgeRenderer.sprite = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
