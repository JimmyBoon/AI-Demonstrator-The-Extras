using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras.Environment;
using System;

namespace Extras.Objects
{
    public class Lights : MonoBehaviour
    {
        Light[] pointLights;
        NightDay nightDay;

        [SerializeField] GameObject globe = null;
        [SerializeField] float minChangeTime = 3f;
        [SerializeField] float maxChangeTime = 10f;

        private void Awake()
        {
            pointLights = GetComponentsInChildren<Light>();
            nightDay = FindObjectOfType<NightDay>();

            nightDay.NightDayUpdated += HandleNightDayUpdated;

        }

        private void HandleNightDayUpdated(bool isNight)
        {
            StartCoroutine (ActivateLights(isNight));
        }

        private IEnumerator ActivateLights(bool isNight)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minChangeTime, maxChangeTime));

            if (isNight)
            {
                foreach (Light light in pointLights)
                {
                    light.enabled = true;
                    if(globe != null)
                    {
                        globe.SetActive(true);
                    }
                }
            }
            else
            {
                foreach (Light light in pointLights)
                {
                    light.enabled = false;
                    if (globe != null)
                    {
                        globe.SetActive(false);
                    }
                }
            }
        }
    }
}