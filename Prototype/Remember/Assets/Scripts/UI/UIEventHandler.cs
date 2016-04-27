using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class UIEventHandler : MonoBehaviour 
{
    bool inventoryOpen = false;
    bool buttonUp = true;
	// Use this for initialization
	void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventory.SetActive(false);
        SetTierActive(false, tier1, null);
        SetTierActive(false, tier2, null);
        SetTierActive(false, tier3, null);
        entryPoint.GetComponentInChildren<Button>().onClick.AddListener(delegate { ToggleInventory(); });
	}

    public GameObject inventory;
    public GameObject tier1;
    public GameObject tier2;
    public GameObject tier3;
    public uint layer = 0;
    public EventSystem uiEventSystem;
    public GameObject entryPoint;
	// Update is called once per frame
	void Update () 
    {
        if(inventoryOpen)
        {
            if (uiEventSystem.currentSelectedGameObject != null)
            {
                if (uiEventSystem.currentSelectedGameObject.gameObject == entryPoint.GetComponentInChildren<Button>().gameObject)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    inventoryOpen = false;
                    inventory.SetActive(false);
                    SetTierActive(false, tier1, null);
                    SetTierActive(false, tier2, null);
                    SetTierActive(false, tier3, null);
                    GetComponentInChildren<RigidbodyFirstPersonController>().enabled = true;
                    uiEventSystem.SetSelectedGameObject(null);
                }
                else if (uiEventSystem.currentSelectedGameObject.transform.parent.gameObject == tier1 && layer != 1)
                {
                    SetTierActive(true, tier1, uiEventSystem.currentSelectedGameObject);
                    SetTierActive(false, tier2, uiEventSystem.currentSelectedGameObject);
                    layer = 1;
                }
                else if (uiEventSystem.currentSelectedGameObject.transform.parent.gameObject == tier2 && layer != 2)
                {
                    SetTierActive(true, tier2, uiEventSystem.currentSelectedGameObject);
                    SetTierActive(false, tier3, uiEventSystem.currentSelectedGameObject);
                    layer = 2;
                }
                else if (uiEventSystem.currentSelectedGameObject.transform.parent.gameObject == tier3 && layer != 3)
                {
                    SetTierActive(true, tier3, uiEventSystem.currentSelectedGameObject);
                    layer = 3;
                }
            }
        }
	    if(Input.GetKeyDown(KeyCode.I))
        {
            if(buttonUp)
            {
                ToggleInventory();
            }
        }
        else if(Input.GetKeyUp(KeyCode.I))
        {
            buttonUp = true;
        }
	}

    public void ToggleInventory()
    {
        if (inventoryOpen == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            buttonUp = false;
            inventoryOpen = true;
            inventory.SetActive(true);
            GetComponentInChildren<RigidbodyFirstPersonController>().enabled = false;
            uiEventSystem.SetSelectedGameObject(tier1.transform.GetChild(0).gameObject);
            SetTierActive(true, tier1, uiEventSystem.currentSelectedGameObject);
            layer = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            buttonUp = false;
            inventoryOpen = false;
            inventory.SetActive(false);
            SetTierActive(false, tier1, null);
            SetTierActive(false, tier2, null);
            SetTierActive(false, tier3, null);
            GetComponentInChildren<RigidbodyFirstPersonController>().enabled = true;
            uiEventSystem.SetSelectedGameObject(entryPoint.GetComponentInChildren<Button>().gameObject);
        }
    }

    public void SetTierActive(bool active, GameObject tier, GameObject parent)
    {
        GameObject obj = uiEventSystem.currentSelectedGameObject;
        uiEventSystem.UpdateModules();
        for(int i = 0; i < tier.transform.childCount; i++)
        {
            GameObject b = tier.transform.GetChild(i).gameObject;
            //b.GetComponent<Button>();
            Image[] imgs = b.GetComponentsInChildren<Image>();
            foreach(Image img in imgs)
            {
                img.enabled = active;
            }
            RawImage[] rimgs = b.GetComponentsInChildren<RawImage>();
            foreach (RawImage img in rimgs)
            {
                img.enabled = active;
            }
            uiEventSystem.SetSelectedGameObject(b);
        }
        uiEventSystem.UpdateModules();
        uiEventSystem.SetSelectedGameObject(obj);
    }
}
