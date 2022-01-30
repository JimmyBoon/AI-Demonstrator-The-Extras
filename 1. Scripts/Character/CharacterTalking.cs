using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras.Objects;


namespace Extras.Character
{
    public class CharacterTalking : MonoBehaviour
    {
        [SerializeField] float wantsToTalkTriggerValue = 60f;
        [SerializeField] float conversationLength = 8f;
        [SerializeField] bool social = false;
        [SerializeField] bool inConversation = false;

        CharacterTalking otherCharacter = null;

        CharacterNeeds needs;
        CharacterMotion motion;
        CharacterEyeSight eyeSight;
        Animator animator;

        public event Action<ActionType> TalkingStatusUpdated;

        private void Awake()
        {
            needs = GetComponent<CharacterNeeds>();
            motion = GetComponent<CharacterMotion>();
            eyeSight = GetComponent<CharacterEyeSight>();
            animator = GetComponent<Animator>();
        }

        public float GetConversationLength()
        {
            return conversationLength;
        }

        public void SetCoversationLength(float newConversationLength)
        {
            conversationLength = newConversationLength;
        }

        public CharacterTalking GetOtherCharacter()
        {
            return otherCharacter;
        }

        public bool EvaluateTalking()
        {
            ObjectOfInterest possibleCharacter = eyeSight.Look();

            if (possibleCharacter != null && possibleCharacter.GetObjectType() == ObjectType.Character
            && possibleCharacter.gameObject != this.gameObject
            && needs.GetSocialPriority() > wantsToTalkTriggerValue
            && possibleCharacter.GetComponent<CharacterTalking>().WantsToTalk())
            {
                otherCharacter = possibleCharacter.GetComponent<CharacterTalking>();
                return true;
            }
            else
            {
                otherCharacter = null;
                return false;
            }

        }

        public void InitiateConversation()
        {
            if (otherCharacter == null) { return; }

            social = true;
            TalkingStatusUpdated?.Invoke(ActionType.Social);

            motion.CancelDestination();

            otherCharacter.SetCoversationLength(conversationLength);
            otherCharacter.motion.CancelDestination();
            otherCharacter.SetOtherCharacter(this.gameObject.GetComponent<CharacterTalking>());
            otherCharacter.SetTalking(ActionType.Social);

            otherCharacter.transform.LookAt(transform.position); //todo check if this is required, might result in stuttering

        }


        public void BeginConversation()
        {
            needs.SetNeed(ObjectType.Character, 100f);
            motion.CancelDestination();
            transform.LookAt(otherCharacter.transform.position);

            TalkingStatusUpdated?.Invoke(ActionType.Conversation);
            inConversation = true;
            social = false;

            animator.SetBool("talking", true);
            animator.SetFloat("talkValue", UnityEngine.Random.Range(0.0f, 1f));
        }



        public IEnumerator InConversation()
        {
            yield return new WaitForSeconds(conversationLength);
            animator.SetBool("talking", false);
            inConversation = false;
            otherCharacter = null;
            conversationLength = UnityEngine.Random.Range(3f, 15f);

        }

        public bool WantsToTalk()
        {
            return needs.GetSocialPriority() > wantsToTalkTriggerValue && social == false && inConversation == false;
        }

        public void SetTalking(ActionType actionType)
        {
            social = true;
            this.TalkingStatusUpdated?.Invoke(actionType);
        }

        public void SetOtherCharacter(CharacterTalking character)
        {
            otherCharacter = character;
        }
    }
}