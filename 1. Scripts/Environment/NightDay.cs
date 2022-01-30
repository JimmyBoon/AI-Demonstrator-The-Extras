using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Environment
{
    public class NightDay : MonoBehaviour
    {
        [SerializeField] bool isNight = false;
        [SerializeField] GameObject fireFlies = null;
        [SerializeField] float dayNightDuration = 300f;

        bool currentNightStatus = false;

        public event Action<bool> NightDayUpdated;

        private void Start()
        {
            StartCoroutine(DayNightCycle());
        }

        void Update()
        {
            if (currentNightStatus == isNight)
            {
                return;
            }
            currentNightStatus = isNight;
            NightDayUpdated?.Invoke(currentNightStatus);
        }

        public void ToggleNight()
        {
            if (isNight == true)
            {
                isNight = false;
            }
            else if (isNight == false)
            {
                isNight = true;
            }
        }

        private IEnumerator DayNightCycle()
        {   
            yield return new WaitForSeconds(dayNightDuration);
            ToggleNight();
            StartCoroutine(DayNightCycle());
        }
    }
}