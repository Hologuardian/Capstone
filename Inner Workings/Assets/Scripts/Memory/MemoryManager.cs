using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MemoryManager : MonoBehaviour
{
    public List<Memory> Memories;

    public RawImage show;

    public void Start()
    {
        ShowMemory("test");
    }

    public void ShowMemory(string memory)
    {
        Texture mem = null;
        foreach (Memory m in Memories)
        {
            if (m.name == memory)
                mem = m.texture;
        }
        if(mem != null)
        {
            show.texture = mem;
            ((RectTransform)(show.transform)).sizeDelta = new Vector2(mem.width, mem.height);
            show.enabled = true;
            StartCoroutine("FadeIn", show);
        }
    }

    public IEnumerator FadeIn(RawImage r)
    {
        float fade = 0.0f;
        while (fade < 1.0f)
        {
            Color c = r.color;
            c.a = fade;
            r.color = c;
            fade += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine("FadeOut", r);
    }

    public IEnumerator FadeOut(RawImage r)
    {
        yield return new WaitForSeconds(2.0f);
        float fade = 1.0f;
        while (fade > 0.0f)
        {
            Color c = r.color;
            c.a = fade;
            r.color = c;
            fade -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine("FadeOut", r);
        r.enabled = false;
    }
}
