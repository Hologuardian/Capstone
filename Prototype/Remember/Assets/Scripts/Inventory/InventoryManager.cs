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
                b.enabled = true;
                slots.Add(b);
            }
        }
        inventory = new Item[slots.Count];
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
