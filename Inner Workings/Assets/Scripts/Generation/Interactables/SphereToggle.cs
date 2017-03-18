using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SphereToggle : MonoBehaviour
{
    public Renderer toggle;
    //public ParticleSystem particles;
    public ParticleSystem souls;
    public ParticleSystem rain;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            toggle.enabled = true;
            //particles.Play();
            souls.Stop();
            souls.Clear();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            toggle.enabled = false;
            //particles.Stop();
            //particles.Clear();
            souls.Play();
        }
    }
}
