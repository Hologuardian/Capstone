using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Effects;

public class PlayerHealth : MonoBehaviour
{
    public float healSpeed;

    public hpStats health;

    public BlurOptimized blurPingPong;
    public Vortex vortexOn;

    private void Awake()
    {
        health.Initialize();
    }
 
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            health.CurrVal -= 10;
        }

        if (health.CurrVal < health.MaxVal)
        {
            healSpeed = 2;
            health.CurrVal += healSpeed * Time.deltaTime;
        }

        if (health.CurrVal >= health.MaxVal)
        {
            healSpeed = 0;            
        }

        if (health.CurrVal <= 60)
        {
            blurPingPong.enabled = true;
            float offset = Mathf.PingPong(Time.time * 2, 2);
            blurPingPong.blurSize = offset + 1;
        }

        if (health.CurrVal <= 30)
        {
            vortexOn.enabled = true;
        }

        if (health.CurrVal > 30)
        {
            vortexOn.enabled = false;
        }

        if (health.CurrVal > 60)
        {
            blurPingPong.enabled = false;
        }
    }
}
