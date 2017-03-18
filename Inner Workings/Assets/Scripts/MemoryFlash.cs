using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryFlash : MonoBehaviour {
    public MemoryManager manager;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(FlashMemories());
	}

    IEnumerator FlashMemories()
    {
        yield return new WaitForSeconds(2.0f);
        for (int i = 1; i <= 5; i++)
        {
            manager.ShowMemory("test" + i);
            yield return new WaitForSeconds(1.0f);
        }
        manager.ShowMemory("test6");
        yield return new WaitForSeconds(2.0f);
        manager.ShowMemory("test7");
        yield return new WaitForSeconds(4.0f);
    }
}
