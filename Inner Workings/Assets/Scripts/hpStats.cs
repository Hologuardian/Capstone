using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class hpStats
{
    [SerializeField]
    private BarScript bar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currVal;

    public float CurrVal
    {
        get
        {
            return currVal;
        }

        set
        {
            this.currVal = value;
            bar.value = currVal;           
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            this.maxVal = value;
            bar.maxValue = maxVal;
        }
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
    }
}
