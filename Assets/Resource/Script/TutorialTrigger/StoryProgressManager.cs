using UnityEngine;

public class StoryProgressManager : MonoBehaviour
{
    [SerializeField] private GameObject alertTrigger;

    private bool backdoorInstalled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(alertTrigger != null)
        {
            alertTrigger.SetActive(false);
        }
    }

    public void OnBackdoorInstalled()
    {
        backdoorInstalled = true;

        Debug.Log("스토리 진행: 백도어 설치 완료");
        Debug.Log("대화 출력: 침입 흔적이 감지되었습니다.");

        if (alertTrigger != null)
        {
            alertTrigger.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
