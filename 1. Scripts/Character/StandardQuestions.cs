using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Character
{
    [CreateAssetMenu(fileName = "StandardQuestions", menuName = "Conversation/Make Standard Questions", order = 0)]
    public class StandardQuestions : ScriptableObject
    {
        [SerializeField] List<string> standardQuestions = new List<string>();

        public List<string> GetStandardQuestions()
        {
            return standardQuestions;
        }
    }
}