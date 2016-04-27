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
        CraftingTree.LoadCrafting();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventory.SetActive(false);
        SetTierActive(false, tier1, ID.UIRoot);
        SetTierActive(false, tier2, ID.Tools);
        SetTierActive(false, tier3, ID.UIRoot);
        entryPointParent.GetComponentInChildren<Button>().onClick.AddListener(delegate { ToggleInventory(); });
        Button[] buttons = tier1.GetComponentsInChildren<Button>();
        int iter = 0;
        foreach(Button b in buttons)
        {
            b.onClick.AddListener(delegate { OnClick(b, 1, iter); });
        }
        iter = 0;
        buttons = tier2.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(delegate { OnClick(b, 2, iter); });
        }
        iter = 0;
        buttons = tier3.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(delegate { OnClick(b, 1, iter); });
        }
	}

    public GameObject inventory;
    public GameObject tier1;
    public GameObject tier2;
    public GameObject tier3;
    public uint layer = 0;
    public ID tier1Item;
    public ID tier2Item;
    public EventSystem uiEventSystem;
    public GameObject entryPointParent;
    public GameObject lastSelected = null;
	// Update is called once per frame
	void Update () 
    {
        if(inventoryOpen)
        {
            if (uiEventSystem.currentSelectedGameObject != null)
            {
                if (uiEventSystem.currentSelectedGameObject.gameObject == entryPointParent.GetComponentInChildren<Button>().gameObject)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    inventoryOpen = false;
                    inventory.SetActive(false);
                    SetTierActive(false, tier1, ID.UIRoot);
                    SetTierActive(false, tier2, ID.UIRoot);
                    SetTierActive(false, tier3, ID.UIRoot);
                    GetComponentInChildren<RigidbodyFirstPersonController>().enabled = true;
                    uiEventSystem.SetSelectedGameObject(null);
                }
                else if (uiEventSystem.currentSelectedGameObject.transform.parent.gameObject == tier1 && layer != 1)
                {
                    SetTierActive(true, tier1, ID.UIRoot);
                    SetTierActive(false, tier2, ID.UIRoot);
                    layer = 1;
                }
                else if (uiEventSystem.currentSelectedGameObject.transform.parent.gameObject == tier2 && layer != 2)
                {
                    if(layer == 1)
                    {
                        Debug.Log("Tier 1 ID: " + int.Parse(lastSelected.name));
                        tier1Item = CraftingTree.craftingUI[ID.UIRoot][int.Parse(lastSelected.name)];
                    }
                    SetTierActive(true, tier2, tier1Item);
                    SetTierActive(false, tier3, tier1Item);
                    layer = 2;
                }
                else if (uiEventSystem.currentSelectedGameObject.transform.parent.gameObject == tier3 && layer != 3)
                {
                    if (layer == 2)
                    {
                        Debug.Log("Tier 1: " + tier1Item + "Tier 2 ID: " + int.Parse(lastSelected.name));
                        tier2Item = CraftingTree.craftingUI[tier1Item][int.Parse(lastSelected.name)];
                    }
                    SetTierActive(true, tier3, tier2Item);
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
        if(uiEventSystem.currentSelectedGameObject != lastSelected)
            lastSelected = uiEventSystem.currentSelectedGameObject;
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
            SetTierActive(true, tier1, ID.UIRoot);
            layer = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            buttonUp = false;
            inventoryOpen = false;
            inventory.SetActive(false);
            SetTierActive(false, tier1, ID.UIRoot);
            SetTierActive(false, tier2, ID.UIRoot);
            SetTierActive(false, tier3, ID.UIRoot);
            GetComponentInChildren<RigidbodyFirstPersonController>().enabled = true;
            uiEventSystem.SetSelectedGameObject(entryPointParent.GetComponentInChildren<Button>().gameObject);
        }
    }

    public void SetTierActive(bool active, GameObject tier, ID parent)
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
            int iter = 0;
            ID[] items = new ID[] { };
            if(CraftingTree.craftingUI.ContainsKey(parent))
                items = CraftingTree.craftingUI[parent];
            string debug = "";
            foreach(ID id in items)
                debug += id;
            Debug.Log(debug);
            RawImage[] rimgs = b.GetComponentsInChildren<RawImage>();
            foreach (RawImage img in rimgs)
            {
                if (iter < items.Length)
                {
                    img.texture = ItemFactory.makeItem(items[iter], 0).Icon;
                    img.enabled = active;
                    iter++;
                }
            }
            uiEventSystem.SetSelectedGameObject(b);
        }
        uiEventSystem.UpdateModules();
        uiEventSystem.SetSelectedGameObject(obj);
    }

    public void OnClick(Button b, int tier, int index)
    {
        if(tier == 1)
        {

        }
        if (tier == 2)
        {

        }
        if (tier == 3)
        {
           InventoryManager inventory = gameObject.GetComponent<InventoryManager>();
           ID[] req = CraftingTree.Required[tier2Item];
        }
    }
}
