using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarScript : MonoBehaviour
{
    public float fillAmount;
    public Image content;

    public float maxValue { get; set; }

    [SerializeField]
    private float lerpSpeed;

    public float value
    {
        set
        {
            fillAmount = Map(value, 0, maxValue, 0, 1);
        }
    }

	void Start ()
    {
        
	}

	void Update ()
    {
        handleBar();
	}

    public void handleBar()
    {
        content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
    }

    public float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
