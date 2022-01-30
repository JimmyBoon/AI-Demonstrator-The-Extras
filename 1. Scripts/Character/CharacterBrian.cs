using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras.Objects;
using Extras.Environment;


namespace Extras.Character
{

    public class CharacterBrian : MonoBehaviour
    {

        [SerializeField] ActionType recommendedActionType = ActionType.None;
        [SerializeField] ActionType currentActionType = ActionType.None;

        [SerializeField] float foodPri;
        [SerializeField] float waterPri;
        [SerializeField] float socialPri;
        [SerializeField] float culturePri;
        [SerializeField] float searchPri;
        [SerializeField] float restPri;


        [SerializeField] string uniqueIdentifier = "";
        [SerializeField] float inspectionRange = 3f;
        [SerializeField] float dwellTime = 2f;
        [SerializeField] float delayTime = 2f;

        CharacterBrian otherCharacter = null;

        [SerializeField] private bool itsRaining = false;
        [SerializeField] private bool delay = true;
        [SerializeField] private float converstationDistance = 2f;

        [SerializeField] private ObjectOfInterest currentThingOfInterest = null;
        [SerializeField] private ObjectOfInterest culturalThingOfInterest = null;


        CharacterCulture culture;
        CharacterEyeSight eyeSight;
        CharacterMotion motion;
        CharacterNeeds needs;
        CharacterSearching searching;
        CharacterTalking talking;
        Weather weather;
        Animator animator;

        private void Awake()
        {
            uniqueIdentifier = System.Guid.NewGuid().ToString();

            culture = GetComponent<CharacterCulture>();
            eyeSight = GetComponent<CharacterEyeSight>();
            motion = GetComponent<CharacterMotion>();
            needs = GetComponent<CharacterNeeds>();
            searching = GetComponent<CharacterSearching>();
            talking = GetComponent<CharacterTalking>();
            weather = FindObjectOfType<Weather>();
            animator = GetComponent<Animator>();

            weather.WeatherUpdated += HandleWeatherUpdated;
            talking.TalkingStatusUpdated += HandleTalkingStatusUpdated;
        }

        private void OnDestroy()
        {
            weather.WeatherUpdated -= HandleWeatherUpdated;
            talking.TalkingStatusUpdated -= HandleTalkingStatusUpdated;
        }

        private void Start()
        {
            StartCoroutine(Bow());
        }

        private IEnumerator Bow()
        {
            motion.CancelDestination();
            transform.LookAt(Camera.main.transform.position);
            animator.SetTrigger("bow");
            yield return new WaitForSeconds(3f);
            delay = false;
        }

        private void Update()
        {
            CalculateActionPriority();

            if (delay)
            {
                return;
            }

            //Talking
            if (currentActionType == ActionType.Conversation)
            {
                StartCoroutine(talking.InConversation());
                StartCoroutine(Delay(talking.GetConversationLength() + 2f));
                ResetAll();
                return;
            }

            if (currentActionType == ActionType.Social)
            {

                if (WithinDistance(talking.GetOtherCharacter().transform.position, converstationDistance))
                {
                    talking.BeginConversation();
                    return;
                }
                motion.SetDestination(talking.GetOtherCharacter().GetComponent<ObjectOfInterest>().GetGoToPoint().position);
                return;
            }

            if (talking.EvaluateTalking())
            {
                currentActionType = ActionType.Social;
                talking.InitiateConversation();
                return;
            }



            //Needs

            if (currentActionType == ActionType.Needs)
            {
                if (WithinDistance(currentThingOfInterest.GetGoToPoint().position, inspectionRange))
                {
                    needs.CompleteNeedsGathering();
                    ResetAll();
                    StartCoroutine(Delay(3f));
                }
                return;
            }

            if (recommendedActionType == ActionType.Needs)
            {
                if (needs.NeedsProcessing())
                {
                    currentThingOfInterest = needs.GetCurrentThingOfInterest();
                    currentActionType = ActionType.Needs;
                }
                return;
            }

            //Rain

            if (itsRaining && searching.GetMyHome() != null)
            {
                motion.SetDestination(searching.GetMyHome().GetComponent<ObjectOfInterest>().GetGoToPoint().position);
                return;
            }

            //Investigating and Culture

            if (currentActionType == ActionType.Investigating && !searching.InvestigationInProgress(currentThingOfInterest))
            {
                currentActionType = ActionType.None;

                StartCoroutine(Delay(2f));

                return;
            }

            if (currentActionType == ActionType.Culture)
            {
                motion.SetDestination(culturalThingOfInterest.GetGoToPoint().position);
                if (WithinDistance(culturalThingOfInterest.GetGoToPoint().position, 1f))
                {
                    StartCoroutine(Delay(culture.EnjoyTheCulture() + 3f));
                    ResetAll();
                }
                return;
            }

            if (recommendedActionType == ActionType.Culture && culture.GetKnownCultureCount() > 0)
            {
                culture.ChooseCultureObject();
                culturalThingOfInterest = culture.GetCulturalObject();
                currentActionType = ActionType.Culture;
                return;
            }
            else if (recommendedActionType == ActionType.Culture && culture.GetKnownCultureCount() == 0)
            {
                culture.ResetCulture();
            }

            //Searching

            //Begin Investigation
            if (currentActionType == ActionType.Search
            && eyeSight.Look() != null
            && !searching.GetThingsSeen().ContainsKey(eyeSight.Look())
            && eyeSight.Look().GetObjectType() != ObjectType.Character)
            {
                currentThingOfInterest = eyeSight.Look();
                currentActionType = ActionType.Investigating;
                motion.SetDestination(currentThingOfInterest.GetGoToPoint().position);
            }

            if (currentActionType == ActionType.Investigating) { return; }

            //Complete Investigation
            if (WithinDistance(searching.GetSearchDestination(), 3f))
            {
                currentActionType = ActionType.None;
                searching.SearchCompleted();
                motion.CancelDestination();
                StartCoroutine(Delay(1.5f));
            }

            if (currentActionType == ActionType.Search) { return; }

            if (recommendedActionType == ActionType.Search)
            {
                motion.SetDestination(searching.SearchNewPoint());
                currentActionType = ActionType.Search;
                return;
            }

            if (searching.GetMyHome() != null && recommendedActionType == ActionType.Rest)
            {
                motion.SetDestination(searching.GetMyHome().GetSleepPosition().position);
                if (WithinDistance(searching.GetMyHome().GetSleepPosition().position, 0.5f))
                {
                    needs.SetNeed(ObjectType.Home, 100f);
                    StartCoroutine(culture.Sleep(20f));
                    StartCoroutine(Delay(23f));
                }
                return;
            }

            motion.SetDestination(searching.SearchNewPoint());
            return;
        }

        private void HandleWeatherUpdated()
        {
            if (weather.GetRaining())
            {
                searching.SetSearchInProgress(false);
                searching.SetInvestigationInProgress(false);
                itsRaining = true;
                motion.SetRun();
            }
            else
            {
                itsRaining = false;
                motion.SetWalk();
                ResetAll();
            }
        }

        private void HandleTalkingStatusUpdated(ActionType action)
        {
            currentActionType = action;
        }

        private bool WithinDistance(Vector3 targetDestination, float distance)
        {
            return (targetDestination - transform.position).sqrMagnitude <= distance * distance;
        }

        private IEnumerator Delay(float seconds)
        {

            delay = true;
            yield return new WaitForSeconds(seconds);
            delay = false;

        }

        private void ResetAll()
        {
            currentActionType = ActionType.None;
            searching.SetSearchInProgress(false);
            searching.SetInvestigationInProgress(false);
            motion.CancelDestination();
        }

        void Hit()
        {

        }

        private void CalculateActionPriority()
        {

            ActionPriority food = new ActionPriority();
            ActionPriority water = new ActionPriority();
            ActionPriority social = new ActionPriority();
            ActionPriority rest = new ActionPriority();
            ActionPriority cultural = new ActionPriority();
            ActionPriority search = new ActionPriority();

            food.actionType = ActionType.Needs;
            food.priorityValue = needs.GetFoodPriority();
            foodPri = needs.GetFoodPriority();

            water.actionType = ActionType.Needs;
            water.priorityValue = needs.GetWaterPriority();
            waterPri = needs.GetWaterPriority();

            social.actionType = ActionType.Social;
            social.priorityValue = needs.GetSocialPriority();
            socialPri = needs.GetSocialPriority();

            rest.actionType = ActionType.Rest;
            rest.priorityValue = needs.GetRestPriority();
            restPri = needs.GetRestPriority();

            cultural.actionType = ActionType.Culture;
            cultural.priorityValue = culture.GetCulturePriority();
            culturePri = culture.GetCulturePriority();

            search.actionType = ActionType.Search;
            search.priorityValue = searching.CalculateSearchPriority();
            searchPri = search.priorityValue;

            List<ActionPriority> priorities = new List<ActionPriority>() { food, water, cultural, search, rest };

            ActionPriority highestPriority = new ActionPriority();
            float highestValue = Mathf.NegativeInfinity;

            foreach (ActionPriority priority in priorities)
            {
                if (priority.priorityValue > highestValue)
                {
                    highestPriority = priority;
                    highestValue = priority.priorityValue;
                }
            }

            recommendedActionType = highestPriority.actionType;

        }

        struct ActionPriority
        {
            public ActionType actionType { get; set; }
            public float priorityValue { get; set; }
        }

    }
}