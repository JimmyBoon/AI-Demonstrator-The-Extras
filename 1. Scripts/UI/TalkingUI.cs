using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Extras.UI
{
    public class TalkingUI : MonoBehaviour
    {
        [SerializeField] GameObject parentCanvas;
        [SerializeField] GameObject floatingTextPrefab;

        public void LaunchText(string textToDisplay)
        {
            GameObject floatTextInstance = Instantiate(floatingTextPrefab, parentCanvas.transform.position, parentCanvas.transform.rotation, parentCanvas.transform);
            TextMeshPro dialogueText = floatTextInstance.GetComponent<TextMeshPro>();
            dialogueText.text = textToDisplay;

        }
    }
}
