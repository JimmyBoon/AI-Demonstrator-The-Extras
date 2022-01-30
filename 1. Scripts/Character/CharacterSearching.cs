using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Extras.Objects;

namespace Extras.Character
{
    public class CharacterSearching : MonoBehaviour
    {
        Vector3 searchDestination;
        CharacterCulture culture;
        CharacterMotion motion;
        CharacterEyeSight eyeSight;
        NavMeshAgent agent;

        [SerializeField] float searchPriorty;
        [SerializeField] bool searchInProgress = false;
        [SerializeField] bool investigationInProgress = false;

        [SerializeField] float notGoingBackWithinRange = 10f;
        [SerializeField] float searchPointRange = 50f;
        [SerializeField] float inspectionRange = 2f;

        [SerializeField] List<Transform> placesToSearch = new List<Transform>();
        [SerializeField] int thingsToFind = 3;
        [SerializeField] List<Vector3> placesSearched = new List<Vector3>();

        [SerializeField] ObjectOfInterest basicFoodToAdd;
        [SerializeField] ObjectOfInterest basicWaterToAdd;

        Dictionary<ObjectOfInterest, ObjectType> thingsSeen = new Dictionary<ObjectOfInterest, ObjectType>();

        [SerializeField] Home myHome = null;

        private void Awake()
        {
            culture = GetComponent<CharacterCulture>();
            motion = GetComponent<CharacterMotion>();
            eyeSight = GetComponent<CharacterEyeSight>();
            agent = GetComponent<NavMeshAgent>();

            thingsToFind = FindObjectsOfType<ObjectOfInterest>().Length;

            thingsSeen.Add(basicFoodToAdd, basicFoodToAdd.GetObjectType());
            thingsSeen.Add(basicWaterToAdd, basicWaterToAdd.GetObjectType());
        }

        public Dictionary<ObjectOfInterest, ObjectType> GetThingsSeen()
        {
            return thingsSeen;
        }

        public Vector3 GetSearchDestination()
        {
            return searchDestination;
        }

        public bool GetSearchInProgress()
        {
            return searchInProgress;
        }

        public void SetSearchInProgress(bool searching)
        {
            searchInProgress = searching;
        }

        public bool GetInvestigationInProgress()
        {
            return investigationInProgress;
        }

        public void SetInvestigationInProgress(bool investigation)
        {
            investigationInProgress = investigation;
        }

        public Home GetMyHome()
        {
            return myHome;
        }

        #region Searching

        public Vector3 SearchNewPoint()
        {
            searchDestination = GenerateSearchPoint();
            int loopcounter = 0;
            while (!ValidateSearchPoint(searchDestination))
            {
                searchDestination = GenerateSearchPoint();
                loopcounter++;
                if (loopcounter > 50)
                {
                    Debug.Log("loop break");
                    break;
                }
            }
            searchInProgress = true;
            return searchDestination;
        }

        private Vector3 GenerateSearchPoint()
        {

            Vector3 searchPoint = UnityEngine.Random.insideUnitSphere * searchPointRange;
            if (placesSearched.Count > 5 && placesToSearch.Count > 0)
            {
                searchPoint = placesSearched[0];
                placesSearched.Clear();
                placesToSearch.RemoveAt(0);
            }

            searchPoint.y = transform.position.y;

            return searchPoint;
        }

        private bool ValidateSearchPoint(Vector3 destination)
        {
            NavMeshHit navMeshHit;
            NavMeshPath path = new NavMeshPath();
            bool samplePos = NavMesh.SamplePosition(destination, out navMeshHit, 1f, NavMesh.AllAreas);
            bool checkPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (samplePos == false)
            {
                Debug.Log("NavMesh sample is false");
                return false;
            }
            if (checkPath == false)
            {
                Debug.Log("NavMesh path is false");
                return false;
            }

            foreach (Vector3 place in placesSearched)
            {
                if (Vector3.Distance(destination, place) < notGoingBackWithinRange) //todo remove Distance
                {
                    return false;
                }
            }
            return true;
        }

        public void SearchCompleted()
        {
            searchInProgress = false;
            placesSearched.Add(searchDestination);
        }
        #endregion

        #region Investigating

        public bool InvestigationInProgress(ObjectOfInterest currentThingOfInterest)
        {
            if (WithinDistance(currentThingOfInterest.GetGoToPoint().position, inspectionRange))
            {

                thingsSeen.Add(currentThingOfInterest, currentThingOfInterest.GetObjectType());
                motion.CancelDestination();
                

                if (currentThingOfInterest.GetObjectType() == ObjectType.Home && myHome == null)
                {
                    Home home = currentThingOfInterest.GetComponent<Home>();
                    if (home.GetOwnedBy() == null)
                    {
                        myHome = home;
                        home.SetOwnedBy(gameObject);
                    }
                }

                if (currentThingOfInterest.GetObjectType() == ObjectType.Culture)
                {
                    culture.AddToCultureList(currentThingOfInterest);
                }

                currentThingOfInterest = null;
                return false;

            }
            return true;

        }

        public void InvestigateThingOfInterest(ObjectOfInterest currentThingOfInterest)
        {
            currentThingOfInterest = eyeSight.Look();
            SetInvestigationInProgress(true);
            SetSearchInProgress(false);
        }

        #endregion

        private bool WithinDistance(Vector3 targetDestination, float distance)
        {
            return (targetDestination - transform.position).sqrMagnitude <= distance * distance;
        }

        public float CalculateSearchPriority()
        {
            searchPriorty = (60f - ((float)thingsSeen.Count / (float)thingsToFind) * 100f);
            if(myHome == null) 
            {
                searchPriorty += 50f;
            }
            if(culture.GetKnownCultureCount() == 0)
            {
                searchPriorty += 20f;
            }

            return searchPriorty;
        }
    }
}