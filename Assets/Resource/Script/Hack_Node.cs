using UnityEngine;
using System.Collections;
public class Hack_Node : MonoBehaviour
{
    [SerializeField] Guard_Vision GV;
    [SerializeField] private float hackDuration = 4f;
    [SerializeField] private GameObject LightNode;
    public bool isHack;
    public bool isNear;
    [SerializeField] private GameObject offmark;
    [SerializeField] private GameObject LightPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isHack = false;
        isNear = false;
        offmark.SetActive(false);
        LightPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear && !isHack && LightNode.CompareTag("LightOFF"))
            // 가까이 있음 + 해킹 안했음 + 태그 확인
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(HackRoutine());
            }
        }
    }

    private IEnumerator HackRoutine()
    {
        isHack = true;

        LightPanel.SetActive(true);

        GV.LightOFF(hackDuration);

        yield return new WaitForSeconds(hackDuration);

        LightPanel.SetActive(false);

        isHack = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;

            if (isNear)
            {
                offmark.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            offmark.SetActive(false);
        }
    }
}