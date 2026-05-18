using UnityEngine;
using System.Collections;
public class Hack_Node : MonoBehaviour
{
    [SerializeField] Guard_Vision GV;
    [SerializeField] private float hackDuration = 4f;
    [SerializeField] private GameObject LightNode;
    public bool isHack;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isHack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isHack && LightNode.CompareTag("LightOFF"))
        {
            StartCoroutine(HackRoutine());
        }
    }

    private IEnumerator HackRoutine()
    {
        isHack = true;

        GV.LightOFF(hackDuration);

        yield return new WaitForSeconds(hackDuration);

        isHack = false;
    }
}