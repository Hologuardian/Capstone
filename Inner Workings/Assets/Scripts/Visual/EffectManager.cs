using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class EffectManager : MonoBehaviour
{
    public ColorCorrectionCurves Color;
    public EdgeDetection edgeDetect;
    public SunShafts shafts;
    public Camera cam;
    public AudioSource sfx;
    public AudioClip darkAmbient;
    public AudioClip lightAmbient;

    public float interp = 0.0f;
    public AnimationCurve colorCorrect;
    public float transitionScale = 1.0f;
    public bool colorState = true;
    // Update is called once per frame
    void Update ()
    {
        if (colorState) 
            ToColor();
        else
            ToBlack();

        if(interp > 0.0f)
        {
            shafts.enabled = false;
        }
        else
        {
            shafts.enabled = true;
        }

        colorCorrect.MoveKey(0, new Keyframe(0, interp));
        colorCorrect.MoveKey(1, new Keyframe(1, 1.0f - interp));
        colorCorrect.keys[0].tangentMode = 2;
        colorCorrect.keys[1].tangentMode = 2;

        edgeDetect.edgesOnly = Mathf.Clamp(interp * 2.0f, 0.0f, 1.0f);
        edgeDetect.edgeExp = 1.0f - (interp * 0.3f);
        
        Color.blueChannel = colorCorrect;
        Color.redChannel = colorCorrect;
        Color.greenChannel = colorCorrect;
        Color.saturation = 0.25f + (interp * 0.75f);
        Color.UpdateParameters();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Memory")
        {
            sfx.clip = darkAmbient;
            sfx.PlayDelayed(1.0f);
            colorState = false;
            StartCoroutine(DisableParticles());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Memory")
        {
            sfx.clip = lightAmbient;
            sfx.PlayDelayed(1.0f);
            colorState = true;
            StartCoroutine(EnableParticles());
        }
    }

    IEnumerator DisableParticles()
    {
        yield return new WaitForSeconds(0.5f);
        cam.cullingMask &= (~(1 << LayerMask.NameToLayer("TransparentFX")));
    }

    IEnumerator EnableParticles()
    {
        yield return new WaitForSeconds(0.5f);
        cam.cullingMask |= (1 << LayerMask.NameToLayer("TransparentFX"));
    }

    void ToColor()
    {
        if (interp > 0.0f)
            interp -= Time.deltaTime * transitionScale;
        else
            interp = 0;
    }

    void ToBlack()
    {
        if (interp < 1.0f)
            interp += Time.deltaTime * transitionScale;
        else
            interp = 1;
    }
}
