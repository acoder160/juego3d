using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxSunrise;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Texture2D skyboxSunset;

    [SerializeField] private Gradient graddientNightToSunrise;
    [SerializeField] private Gradient graddientSunriseToDay;
    [SerializeField] private Gradient graddientDayToSunset;
    [SerializeField] private Gradient graddientSunsetToNight;

    [SerializeField] private Light globalLight;

    private int minutes;

    public int Minutes
    { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }

    private int hours = 5;

    public int Hours
    { get { return hours; } set { hours = value; OnHoursChange(value); } }

    private int days;

    public int Days
    { get { return days; } set { days = value; } }

    private float tempSecond;

    public void Update()
    {
        tempSecond += Time.deltaTime * 10f;

        if (tempSecond >= 1)
        {
            Minutes += 1;
            tempSecond = 0;
        }
    }

    private void OnMinutesChange(int value)
    {
        // Rota 0.25 grados por minuto alrededor del eje X (simulando amanecer/atardecer)
        globalLight.transform.Rotate(Vector3.right, 0.25f, Space.World);
        if (value >= 60)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }

    private void OnHoursChange(int value)
    {
        if (value == 6)
        {
            // Transición de Noche a Amanecer (dura 2h = 120 min)
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 120f));
            StartCoroutine(LerpLight(graddientNightToSunrise, 120f));
        }
        else if (value == 8)
        {
            // Transición de Amanecer a Día (dura 10h = 600 min)
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 600f));
            StartCoroutine(LerpLight(graddientSunriseToDay, 600f));
        }
        else if (value == 18)
        {
            // Transición de Día a Atardecer (dura 4h = 240 min)
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 240f));
            StartCoroutine(LerpLight(graddientDayToSunset, 240f));
        }
        else if (value == 22)
        {
            // Transición de Atardecer a Noche (dura 8h = 480 min)
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 480f));
            StartCoroutine(LerpLight(graddientSunsetToNight, 480f));
        }
    }

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}