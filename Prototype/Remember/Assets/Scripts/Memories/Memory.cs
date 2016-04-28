using UnityEngine;
using System.Collections;
using System;

public class Memory
{
    private memoryID id;
    public memoryID Id
    {
        get { return id; }
        set { id = value; }
    }

    private Texture memories;
    public Texture Memories
    {
        get { return memories; }
        set { memories = value; }
    }
    private AudioClip sound;
    public AudioClip Sound
    {
        get { return sound; }
        set { sound = value; }
    }
    private float length;
    public float Length
    {
        get { return length; }
        set { length = value; }
    }
}
