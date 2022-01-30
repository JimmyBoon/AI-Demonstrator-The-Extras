using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Environment
{
    
    public class Weather : MonoBehaviour
    {
        [SerializeField] bool raining = false;
        [SerializeField] GameObject rain = null;

        bool currentRainStatus = false;

        public event Action WeatherUpdated;

        private void Update()
        {
            if(currentRainStatus == raining)
            {
                return;
            }
            rain.SetActive(raining);
            currentRainStatus = raining;
            WeatherUpdated?.Invoke();
        }

        public bool GetRaining()
        {
            return currentRainStatus;
        }

        public void ToggleRain()
        {   
            if(raining)
            {
                raining = false;
            }
            else if(!raining)
            {
                raining = true;
            }
        }
        
    }
}