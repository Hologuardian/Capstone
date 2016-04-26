using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Item
{
    private Texture2D icon;
    public Texture2D Icon
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

    private uint id;
    public uint ID
    {
        get{return id;}
        set {id = value;}
    }

    private uint stackSize;
    public uint StackSize
    {
        get {return stackSize;}
        set {stackSize = value;}
    }

    private uint maxStack;
    public uint MaxStack
    {
        get { return maxStack; }
        set { maxStack = value; }
    }

    public void onInteract()
    {
        throw new NotImplementedException();
    }
}
