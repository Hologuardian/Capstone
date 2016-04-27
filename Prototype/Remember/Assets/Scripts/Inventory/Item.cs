using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Item
{
    private Texture icon;
    public Texture Icon
    {
        get{return icon;}
        set {icon = value;}
    }

    private GameObject gameObject;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; }
    }

    private ID id;
    public ID ID
    {
        get{return id;}
        set {id = value;}
    }

    private int stackSize;
    public int StackSize
    {
        get {return stackSize;}
        set {stackSize = value;}
    }

    private int maxStack;
    public int MaxStack
    {
        get { return maxStack; }
        set { maxStack = value; }
    }

    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    private string description;
    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public void onInteract()
    {
        throw new NotImplementedException();
    }
}
