using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Header("Day Cycle")]
    public float dayLength = 5.0f;
    private float dayLengthCurrent = 0.0f;
    public float dayCurrent = 0.0f;

    [Header("Weather")]
    public EnvironmentWeatherSystem[] weather;

    [Header("References")]
    new public Light light;
    public Material[] clouds;

    public bool isDay;

    void Start()
    {
        foreach(EnvironmentWeatherSystem system in weather)
        {
            system.Start(this);
        }
    }

    void Update()
    {
        dayLengthCurrent += Time.deltaTime / 60.0f;

        if(dayLengthCurrent <= dayLength)
        {
            dayCurrent = dayLengthCurrent / dayLength;

            //todo day counting implementation

        }
        else if(dayLengthCurrent <= dayLength * 2)
        {
            dayCurrent = 1 - (dayLengthCurrent - dayLength) / dayLength;

        }
        else
        {
            dayLengthCurrent -= dayLength * 2;
        }

        weather[0].Update();
    }
}
