using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Environment
{
    public class NightDayEffects : MonoBehaviour
    {
        ParticleSystem effects;
        NightDay nightDay;
        [SerializeField] bool playDuringNight = true;

        private void Awake()
        {   
            effects = GetComponent<ParticleSystem>();
            nightDay = FindObjectOfType<NightDay>();
        }

        void Start()
        {
            nightDay.NightDayUpdated += HandleNightDayUpdated;

        }

        private void HandleNightDayUpdated(bool isNight)
        {
            if(isNight && playDuringNight)
            {
                effects.Play();
            }
            else if(isNight && !playDuringNight)
            {
                effects.Stop();
            }
            else if(!isNight && playDuringNight)
            {
                effects.Stop();
            }
            else if(!isNight && !playDuringNight)
            {
                effects.Play();
            }

        }
    }
}