using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Character.Conversations
{
    public class ConversationBuilder : MonoBehaviour
    {
        [SerializeField] List<string> characterOneConverstation = new List<string>();
        [SerializeField] List<string> characterTwoConverstation = new List<string>();
        [SerializeField] List<ConversationLine> conversationLines = new List<ConversationLine>();
        [SerializeField] StandardQuestions questions;
        CharacterTalking otherCharacter;

        public void SetOtherCharacter(CharacterTalking other)
        {
            otherCharacter = other;
        }

        public List<string> GetCharacterOneConverstation()
        {
            return characterOneConverstation;
        }

        public List<string> GetCharacterTwoConverstation()
        {
            return characterTwoConverstation;
        }

        public List<ConversationLine> GetConversationLines()
        {
            return conversationLines;
        }

        public float GetConversationLength()
        {
            return conversationLines.Count * 2f + 1f;
        }

        public void BuildConversation(CharacterTalking characterOne, CharacterTalking characterTwo)
        {
            conversationLines.Clear();

            // for (int i = 0; i < characterOneConverstation.Count; i++)
            // {
            //     ConversationLine lineOne = new ConversationLine(characterOne, characterOneConverstation[i]);
            //     ConversationLine lineTwo = new ConversationLine(characterTwo, characterTwoConverstation[i]);
            //     conversationLines.Add(lineOne);
            //     conversationLines.Add(lineTwo);
            // }
        }
    }
}