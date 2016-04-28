using UnityEngine;
using System.Collections;

public class WorldItem : MonoBehaviour 
{
    public ID itemType;
    Color orig;
    void OnMouseEnter()
    {
        orig = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        if(orig != null)
            GetComponent<Renderer>().material.color = orig;
    }
}
