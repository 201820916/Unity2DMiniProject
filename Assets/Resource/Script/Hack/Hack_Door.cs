using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Hack_Door : MonoBehaviour
{
    [SerializeField] private GameObject lockblocker;
    public bool isHack;
    public bool isNear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isHack = false;
        isNear = false;
        lockblocker.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (isNear && !isHack)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                lockblocker.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }
}

