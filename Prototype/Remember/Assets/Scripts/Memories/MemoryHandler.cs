using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MemoryHandler : MonoBehaviour
{
    public RawImage memoryUI;

	void Start()
    {
        StartCoroutine(DrawMemory(memoryFactory.makeMemory(memoryID.spawnMemory)));
    }

    void Update()
    {
        
    }

    IEnumerator DrawMemory(Memory spawnMemory)
    {
        memoryUI.texture = spawnMemory.Memories;
        memoryUI.enabled = true;

        yield return new WaitForSeconds(spawnMemory.Length);

        memoryUI.enabled = false;
    }
    IEnumerator DrawLogMemory(Memory acquireLogs)
    {
        memoryUI.texture = acquireLogs.Memories;
        memoryUI.enabled = true;

        yield return new WaitForSeconds(acquireLogs.Length);

        memoryUI.enabled = false;
    }
}
