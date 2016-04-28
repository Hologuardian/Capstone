using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour 
{
    private List<Button> slots = new List<Button>();
    private List<Button> hotBar = new List<Button>();
    int yHeight = 1;
    private Item[] inventory;
    public RectTransform panel;
    public RectTransform hotBarPanel;
    public Button template = null;
	// Use this for initialization
	void Start () 
    {
        Rect bounds = panel.rect;
        for (int i = 5; i < (int)bounds.size.x - 45; i += 50)
        {
            for (int j = (int)bounds.size.y - 5; j > 0; j -= 50)
            {
                Button b = Instantiate<Button>(template);
                RectTransform t = b.GetComponent<RectTransform>();
                t.SetParent(panel);
                t.localPosition = new Vector3(i - bounds.size.x / 2, j - bounds.size.y / 2, 0);
                Text tex = b.GetComponentInChildren<Text>(true);
                tex.transform.position = new Vector3(tex.transform.position.x, tex.transform.position.y, 255 - slots.Count);
                //b.enabled = false;
                b.GetComponentInChildren<RawImage>().enabled = false;
                b.name = "Slot" + slots.Count;
                b.onClick.AddListener(delegate { ButtonClicked(b); });
                slots.Add(b);
                b.gameObject.transform.SetAsLastSibling();
                if (i == 5)
                {
                    Button but = Instantiate<Button>(template);
                    t = but.GetComponent<RectTransform>();
                    t.SetParent(hotBarPanel);
                    t.localPosition = new Vector3(-25, j - hotBarPanel.rect.size.y / 2, 0);
                    tex = but.GetComponentInChildren<Text>(true);
                    tex.transform.position = new Vector3(tex.transform.position.x, tex.transform.position.y, 255 - hotBar.Count);
                    but.GetComponentInChildren<RawImage>().enabled = false;
                    but.name = "Hotbar" + hotBar.Count;
                    hotBar.Add(but);
                    but.gameObject.transform.SetAsLastSibling();
                }
            }
        }
        inventory = new Item[slots.Count];
        AddItem(ItemFactory.makeItem(ID.Stone, 99));
        AddItem(ItemFactory.makeItem(ID.Iron, 99));
        AddItem(ItemFactory.makeItem(ID.Log, 99));
        AddItem(ItemFactory.makeItem(ID.Stick, 99));
	}

    public void NewStack(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null)
            {
                setSlotValue(i, item);
                return;
            }
        }
    }

    public void ButtonClicked(Button b)
    {
        int index = slots.IndexOf(b, 0, slots.Count);
        b.GetComponentInChildren<RawImage>().enabled = true;
        b.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture>("One");
        b.GetComponentInChildren<Text>().text = "" + Random.Range(0, 100);
    }

    void setSlotValue(int index, Item item)
    {
        inventory[index] = item;
        slots[index].GetComponentInChildren<RawImage>().enabled = true;
        slots[index].GetComponentInChildren<RawImage>().texture = item.Icon;
    }

    public void IncrementItem(Item item)
    {
        for(int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] != null)
            {
                if(inventory[i].ID == item.ID)
                {
                    inventory[i].StackSize++;
                    return;
                }
            }
        }
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                slots[i].GetComponentInChildren<RawImage>().enabled = true;
                slots[i].GetComponentInChildren<RawImage>().texture = item.Icon;
                return;
            }
        }
    }

    public void DecrementItem(Item item)
    {

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].ID == item.ID)
                {
                    inventory[i].StackSize--;
                    return;
                }
            }
        }
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].ID == item.ID)
                {
                    Item inv = inventory[i];
                    int diff = (inv.MaxStack - inv.StackSize);
                    if(diff - item.StackSize < 0)
                    {

                        inv.StackSize = inv.MaxStack;
                        item.StackSize = item.StackSize - diff;
                        NewStack(item);
                        return;
                    }
                    inv.StackSize += item.StackSize;
                    return;
                }
            }
        }
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                slots[i].GetComponentInChildren<RawImage>().enabled = true;
                slots[i].GetComponentInChildren<RawImage>().texture = item.Icon;
                return;
            }
        }
    }

    public void ReduceItem(Item item)
    {

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].ID == item.ID)
                {
                    inventory[i].StackSize -= item.StackSize;
                    return;
                }
            }
        }
    }

    public void DropItem(int slot)
    {
        inventory[slot] = null;
    }

    public bool HasItem(Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                if (inventory[i].ID == item.ID)
                {
                    if (inventory[i].StackSize > 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () 
    {
        int iter = 0;
        foreach(Item i in inventory)
        {
            if(i != null)
            {
                slots[iter].GetComponentInChildren<Text>().text = "" + i.StackSize;
            }
            iter++;
        }
        iter = 0;
        foreach(Button b in hotBar)
        {
            b.GetComponentInChildren<RawImage>(true).texture = slots[iter].GetComponentInChildren<RawImage>(true).texture;
            b.GetComponentInChildren<RawImage>(true).enabled = slots[iter].GetComponentInChildren<RawImage>(true).enabled;
            b.GetComponentInChildren<Text>(true).text = slots[iter].GetComponentInChildren<Text>(true).text;
            iter++;
        }
	}
}
