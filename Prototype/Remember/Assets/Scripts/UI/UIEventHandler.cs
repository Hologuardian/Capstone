using UnityEngine;
using System.Collections;

public class UIEventHandler : MonoBehaviour 
{
    bool inventoryOpen = false;
    bool buttonUp = true;
	// Use this for initialization
	void Start () 
    {
        inventory.SetActive(false);
        tier1.SetActive(false);
        tier2.SetActive(false);
        tier3.SetActive(false);
	}

    public GameObject inventory;
    public GameObject tier1;
    public GameObject tier2;
    public GameObject tier3;
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.I))
        {
            if(buttonUp)
            {
                if (inventoryOpen == false)
                {
                    this.gameObject.GetComponent<Controller>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    buttonUp = false;
                    inventoryOpen = true;
                    inventory.SetActive(true);
                    tier1.SetActive(true);
                }
                else
                {
                    this.gameObject.GetComponent<Controller>().enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    buttonUp = false;
                    inventoryOpen = false;
                    inventory.SetActive(false);
                    tier1.SetActive(false);
                    tier2.SetActive(false);
                    tier3.SetActive(false);
                }
            }
        }
        else if(Input.GetKeyUp(KeyCode.I))
        {
            buttonUp = true;
        }
	}
}
