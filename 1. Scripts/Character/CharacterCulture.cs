using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras.Objects;

namespace Extras.Character
{
    public class CharacterCulture : MonoBehaviour
    {
        [SerializeField] float culture = 100f;
        [SerializeField] float cultureConsumptionRate = 0.1f;
        bool hasBeenInvoked = false;

        [SerializeField] private List<ObjectOfInterest> cultureObjectsList = new List<ObjectOfInterest>();

        ObjectOfInterest culturalThingOfInterest = null;

        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            culture = UnityEngine.Random.Range(50f, 100f);
            cultureConsumptionRate = UnityEngine.Random.Range(0.2f, 0.5f);
            
        }

        void Update()
        {
            culture = Mathf.Clamp(culture - cultureConsumptionRate * Time.deltaTime, 20, 100);
        }

        public void AddToCultureList(ObjectOfInterest objectOfInterest)
        {
            cultureObjectsList.Add(objectOfInterest);
        }

        public void ResetCulture()
        {
            hasBeenInvoked = false;
            culture = 100f;
        }

        public float GetCulturePriority()
        {
            return 100 - (culture / 100) * 100; ;
        }

        public int GetKnownCultureCount()
        {
            return cultureObjectsList.Count;
        }

        public void ChooseCultureObject()
        {
            culturalThingOfInterest = cultureObjectsList[UnityEngine.Random.Range(0, cultureObjectsList.Count)];
        }

        public ObjectOfInterest GetCulturalObject()
        {
            return culturalThingOfInterest;
        }

        public float EnjoyTheCulture()
        {
            float duration;
            ResetCulture();
            if (culturalThingOfInterest.GetCultureType() == CultureType.Art)
            {
                transform.LookAt(culturalThingOfInterest.transform.position);
                animator.SetTrigger("yes");
                duration = 3f;
                //StartCoroutine(Delay(duration));
                return duration;
            }
            if (culturalThingOfInterest.GetCultureType() == CultureType.Sit)
            {
                duration = 10f;
                StartCoroutine(Sit(10f));
                return duration;
            }
            if (culturalThingOfInterest.GetCultureType() == CultureType.Laydown)
            {
                duration = 10f;
                StartCoroutine(Sleep(10f));
                return duration;

            }
            culturalThingOfInterest = null;
            return 0f;
            //ResetAll();
        }

        public IEnumerator Sleep(float duration)
        {
            //StartCoroutine(Delay(duration + 3f));
            transform.LookAt(Camera.main.transform);
            animator.SetBool("sleep", true);
            yield return new WaitForSeconds(duration);
            animator.SetBool("sleep", false);
        }

        private IEnumerator Sit(float duration)
        {
            //StartCoroutine(Delay(duration + 3f));
            transform.LookAt(Camera.main.transform);
            animator.SetBool("sitting", true);
            yield return new WaitForSeconds(duration);
            animator.SetBool("sitting", false);
        }
    }
}