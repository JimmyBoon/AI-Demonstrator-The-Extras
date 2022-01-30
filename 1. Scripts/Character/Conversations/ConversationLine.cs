using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Character.Conversations
{
    public class ConversationLine
    {
        public CharacterTalking characterTalking;
        public string text;

        public ConversationLine(CharacterTalking character, string line)
        {
            characterTalking = character;
            text = line;
        }
    }
}