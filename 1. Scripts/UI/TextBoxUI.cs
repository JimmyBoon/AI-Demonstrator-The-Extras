using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Extras.UI
{
    
    public class TextBoxUI : MonoBehaviour
    {
        [SerializeField] float climbSpeed = 0.005f;
        [SerializeField] float destroyTime = 7f;
        [SerializeField] float fadeRate = 0.1f;
        RectTransform rectTransform;
        
        TextMeshPro textMeshPro;
        
        void Start()
        {
            textMeshPro = GetComponent<TextMeshPro>();
            rectTransform = GetComponent<RectTransform>();
            Destroy(gameObject, destroyTime);
        }

        
        void Update()
        {
            rectTransform.Translate(0,climbSpeed,0);
            textMeshPro.alpha = Mathf.MoveTowards(textMeshPro.alpha, 0, fadeRate);
        }
    }
}