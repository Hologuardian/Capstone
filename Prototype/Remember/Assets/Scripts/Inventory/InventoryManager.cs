using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour 
{
    private List<Button> slots = new List<Button>();
    private Item[] inventory;
    public RectTransform panel;
    public Button template = null;
	// Use this for initialization
	void Start () 
    {
        Rect bounds = panel.rect;
        for (int j = 55; j < bounds.size.y; j += 50)
        {
            for (int i = 5; i < bounds.size.x - 40; i += 50)
            {
                Button b = Instantiate<Button>(template);
                RectTransform t = b.GetComponent<RectTransform>();
                t.SetParent(panel);
                t.localPosition = new Vector3(i - bounds.size.x / 2, j - bounds.size.y / 2, 0);
                //b.enabled = false;
                b.GetComponentInChildren<RawImage>().enabled = false;
                b.name = "Slot" + slots.Count;
                b.onClick.AddListener(delegate { ButtonClicked(b); });
                slots.Add(b);
            }
        }
        inventory = new Item[slots.Count];
	}

    public void ButtonClicked(Button b)
    {
        int index = slots.IndexOf(b, 0, slots.Count);
        b.GetComponentInChildren<RawImage>().enabled = true;
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
                    inventory[i].StackSize += item.StackSize;
                    return;
                }
            }
        }
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
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
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
