using UnityEngine;
using System.Collections;
using System;

public class Memory
{
    private ID id;
    public ID Id
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
}
