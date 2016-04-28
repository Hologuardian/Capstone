using UnityEngine;
using System.Collections;
using System;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetButtonDown("Fire1"))
        {
            Interact();
        }
	}

    void Interact()
    {
        RaycastHit hit;
        Transform t = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Physics.Raycast(new Ray(t.position, t.forward), out hit);
        if(hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "Collectable")
            {
                Item i = ItemFactory.makeItem(hit.collider.gameObject.GetComponent<WorldItem>().itemType, 1);
                gameObject.GetComponent<InventoryManager>().AddItem(i);
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
