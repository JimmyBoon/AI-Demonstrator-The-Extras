using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras.Objects;
using System;

namespace Extras.Character
{
    public class CharacterNeeds : MonoBehaviour
    {
        [SerializeField] float food = 100f;
        [SerializeField] float water = 100f;
        [SerializeField] float social = 100f;
        [SerializeField] float rest = 100f;
        [SerializeField] float foodConsumptionRate = 1f;
        [SerializeField] float waterConsumptionRate = 1f;
        [SerializeField] float socialConsumptionRate = 0.25f;
        [SerializeField] float restConsumptionRate = 0.1f;
        [SerializeField] bool randomValues = true;

        ObjectOfInterest currentThingOfInterest;

        CharacterMotion motion;
        Animator animator;
        CharacterSearching searching;

        private void Awake()
        {
            motion = GetComponent<CharacterMotion>();
            animator = GetComponent<Animator>();
            searching = GetComponent<CharacterSearching>();

            if(randomValues)
            {
            food = UnityEngine.Random.Range(50f, 100f);
            water = UnityEngine.Random.Range(50f, 100f);
            social = UnityEngine.Random.Range(50f, 100f);
            rest = UnityEngine.Random.Range(50f, 100f);


            foodConsumptionRate = UnityEngine.Random.Range(0.2f, 0.5f);
            waterConsumptionRate = UnityEngine.Random.Range(0.2f, 0.5f);
            socialConsumptionRate = UnityEngine.Random.Range(0.2f, 0.5f);
            restConsumptionRate = UnityEngine.Random.Range(0.2f, 0.5f);
            }

        }

        void Update()
        {
            food = Mathf.Clamp(food - foodConsumptionRate * Time.deltaTime, -10, 100);
            water = Mathf.Clamp(water - waterConsumptionRate * Time.deltaTime, -10, 100);
            social = Mathf.Clamp(social - socialConsumptionRate * Time.deltaTime, 0, 100);
            rest = Mathf.Clamp(rest - restConsumptionRate * Time.deltaTime, 0, 100);

        }

        public void SetNeed(ObjectType type, float value)
        {
            if (type == ObjectType.Food)
            {
                food = value;
            }
            if (type == ObjectType.Water)
            {
                water = value;
            }
            if (type == ObjectType.Character)
            {
                social = value;
            }
            if (type == ObjectType.Home)
            {
                rest = value;
            }
        }

        public ObjectOfInterest GetCurrentThingOfInterest()
        {
            return currentThingOfInterest;
        }


        public float GetFoodPriority()
        {
            return 100 - (food / 100) * 100;
        }

        public float GetWaterPriority()
        {
            return 100 - (water / 100) * 100;
        }

        public float GetSocialPriority()
        {
            return 100 - (social / 100) * 100;
        }

        public float GetRestPriority()
        {
            return 100 - (rest / 100) * 100;
        }

        public bool NeedsProcessing()
        {

            float foodPriority = GetFoodPriority();
            float waterPriority = GetWaterPriority();
            ObjectType currentNeed = new ObjectType();

            if (waterPriority > foodPriority)
            {
                currentNeed = ObjectType.Water;
            }
            else
            {
                currentNeed = ObjectType.Food;
            }


            List<ObjectOfInterest> possibleNeedsProviders = new List<ObjectOfInterest>();
            float distanceToNeedsProviders = Mathf.Infinity;
            ObjectOfInterest closestProvider = null;
            currentThingOfInterest = null;
            motion.CancelDestination();



            foreach (KeyValuePair<ObjectOfInterest, ObjectType> interest in searching.GetThingsSeen())
            {
                if (interest.Value == currentNeed)
                {
                    possibleNeedsProviders.Add(interest.Key);
                }
            }

            foreach (ObjectOfInterest provider in possibleNeedsProviders)
            {
                if (distanceToNeedsProviders > CheckDistance(provider.GetGoToPoint().position))
                {
                    distanceToNeedsProviders = CheckDistance(provider.GetGoToPoint().position);
                    closestProvider = provider;
                }
            }

            if (closestProvider != null)
            {
                currentThingOfInterest = closestProvider;
                motion.SetDestination(currentThingOfInterest.GetGoToPoint().position);
                //currentActionType = ActionType.Needs;
                return true;
            }
            return false;
        }

        public void CompleteNeedsGathering()
        {

            SetNeed(currentThingOfInterest.GetObjectType(), 100f);

            if (currentThingOfInterest.GetObjectType() == ObjectType.Food)
            {
                animator.SetTrigger("pickUp");
            }
            if (currentThingOfInterest.GetObjectType() == ObjectType.Water)
            {
                animator.SetTrigger("drink");
            }
            // ResetAll();
            // StartCoroutine(Delay(endDelay));
        }

        private float CheckDistance(Vector3 targetDestination)
        {
            return Vector3.Distance(transform.position, targetDestination);
        }
    }

}
