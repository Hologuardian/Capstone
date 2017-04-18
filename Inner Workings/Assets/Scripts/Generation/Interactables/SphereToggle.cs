using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class SphereToggle : MonoBehaviour
{
    public Renderer toggle;
    //public ParticleSystem particles;
    public ParticleSystem rain;
    public bool Exiting = false;
    public bool Active = false;

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.5f);
        toggle.enabled = true;
        for (float i = 0; i < 1.0f; i += 0.1f)
        {
            toggle.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, i));
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeOut()
    {
        for (float i = 1.0f; i > 0.0f; i -= 0.1f)
        {
            toggle.material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, i));
            yield return new WaitForSeconds(0.05f);
        }
        if (Exiting)
        {
            Destroy(gameObject);
            yield break;
        }

        toggle.enabled = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Active = true;
            StartCoroutine(FadeIn());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Active = false;
            StartCoroutine(FadeOut());
        }
    }
}
