using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEditor;

public class EffectManager : MonoBehaviour
{
    public ColorCorrectionCurves Color;
    public EdgeDetection edgeDetect;
    public SunShafts shafts;

	// Use this for initialization
	void Start ()
    {
	    	
	}

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
        AnimationUtility.SetKeyLeftTangentMode(colorCorrect, 0, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(colorCorrect, 0, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyLeftTangentMode(colorCorrect, 1, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(colorCorrect, 1, AnimationUtility.TangentMode.Linear);

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
        if(collider.gameObject.tag == "Memory")
            colorState = false;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Memory")
            colorState = true;
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
