using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class MemoryManager : MonoBehaviour
{
    public List<Memory> Memories;

    public RawImage show;
    public Text textBox;
    public Image textBackground;
    public Image background;
    public Image imageMask;

    public void Start()
    {
        ShowMemory("test");
        show.enabled = false;
    }

    public void ShowMemory(string memory)
    {
        Memory mem = null;
        foreach (Memory m in Memories)
        {
            if (m.name == memory)
                mem = m;
        }
        if(mem != null)
        {
            StopAllCoroutines();
            show.texture = mem.texture;
            ((RectTransform)(show.transform)).sizeDelta = new Vector2(Mathf.Clamp(mem.texture.width / 2, 0, Screen.width), Mathf.Clamp(mem.texture.height / 2, 0, Screen.height));
            textBox.enabled = true;
            textBackground.enabled = true;
            textBox.text = mem.text;
            StartCoroutine(FadeIn(mem.time));
            StartCoroutine(FadeOutText(mem.textTime));
        }
    }

    public IEnumerator FadeIn(float time)
    {
        float fade = 0.0f;
        while (fade <= 1.1f)
        {
            Color c = show.color;
            c.a = fade;
            show.color = c;
            c = textBox.color;
            c.a = fade;
            textBox.color = c;
            c = textBackground.color;
            c.a = fade;
            textBackground.color = c;
            c = background.color;
            c.a = fade;
            background.color = c;
            fade += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        show.enabled = true;
        while (fade >= 0.0f)
        {
            Color c = imageMask.color;
            c.a = fade;
            imageMask.color = c;
            c = textBox.color;
            fade -= 0.2f;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(FadeOut(time));
    }

    public IEnumerator FadeOut(float time)
    {
        yield return new WaitForSeconds(time);
        float fade = 0.0f;
        while (fade <= 1.1f)
        {
            Color c = imageMask.color;
            c.a = fade;
            imageMask.color = c;
            c = textBox.color;
            fade += 0.2f;
            yield return new WaitForSeconds(0.05f);
        }
        show.enabled = false;
        Color maskC = imageMask.color;
        maskC.a = 0;
        imageMask.color = maskC;
        fade = 1.0f;
        while (fade >= -0.1f)
        {
            Color c = show.color;
            c.a = fade;
            show.color = c;
            c = background.color;
            c.a = fade;
            background.color = c;
            fade -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator FadeOutText(float time)
    {
        yield return new WaitForSeconds(time + 1.0f);
        float fade = 1.0f;
        while (fade >= 0.0f)
        {
            Color c = textBox.color;
            c.a = fade;
            textBox.color = c;
            c = textBackground.color;
            c.a = fade;
            textBackground.color = c;
            fade -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        textBox.enabled = false;
        textBackground.enabled = false;
        textBox.text = "";
    }
}
