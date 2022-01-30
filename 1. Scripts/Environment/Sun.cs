using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Environment
{
    public class Sun : MonoBehaviour
    {
        Light directionalLight;
        NightDay nightDay;
        [SerializeField] float dayIntensity = 1f;
        [SerializeField] float nightIntensity = 0f;
        [SerializeField] float skyDayIntensity = 1f;
        [SerializeField] float skyNightIntesity = 0.1f;
        [SerializeField] float tranistionTime = 10f;



        private void Awake()
        {
            directionalLight = GetComponent<Light>();
            nightDay = FindObjectOfType<NightDay>();
            nightDay.NightDayUpdated += HandleNightDayUpdated;
        }

        private void OnDestroy()
        {
            RenderSettings.skybox.SetFloat("_Exposure", 1f);
        }


        private void HandleNightDayUpdated(bool isNight)
        {
            if (isNight)
            {
                StartCoroutine(SunSetRise(nightIntensity, skyNightIntesity, tranistionTime));
            }
            else
            {
                StartCoroutine(SunSetRise(dayIntensity, skyDayIntensity, tranistionTime));
            }

        }

        private IEnumerator SunSetRise(float sunTarget, float skyTarget, float fadeTime)
        {
            while (!Mathf.Approximately(directionalLight.intensity, sunTarget))
            {
                directionalLight.intensity = Mathf.MoveTowards(directionalLight.intensity, sunTarget, Time.deltaTime / fadeTime);
                RenderSettings.skybox.SetFloat("_Exposure", Mathf.MoveTowards(directionalLight.intensity, skyTarget, Time.deltaTime / fadeTime));

                yield return null;

            }
        }
    }
}