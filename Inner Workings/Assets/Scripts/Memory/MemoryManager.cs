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
    public AudioSource source;
    public AudioSource music;

    public void Start()
    {
        ShowMemory("test");
        show.enabled = false;
    }

    public void SwitchToIsolationScene(float delay)
    {
        StartCoroutine(WaitThenSwitchScene(delay, 4));
    }

    public IEnumerator WaitThenSwitchScene(float time, int scene)
    {
        yield return new WaitForSeconds(time);
        LoadingSceneManager.LoadScene(scene);
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
            show.gameObject.SetActive(true);
            //((RectTransform)(show.transform)).sizeDelta = new Vector2(Mathf.Clamp(mem.texture.width, 0, Screen.width), Mathf.Clamp(mem.texture.height, 0, Screen.height));
            textBox.enabled = true;
            textBackground.enabled = true;
            textBox.text = mem.text;
            StartCoroutine(FadeIn(mem.time));
            StartCoroutine(PlayAudio(mem.audio));
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
            c.a = fade * 0.6f;
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
        while (fade >= -0.1f)
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

    public IEnumerator PlayAudio(AudioClip clip)
    {
        yield return new WaitForSeconds(0.8f);
        music.Pause();
        //Audio
        source.loop = false;
        source.clip = clip;
        source.Play();
        yield return new WaitForSeconds(clip.length);
        music.UnPause();
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
        float fade = 0.6f;
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
