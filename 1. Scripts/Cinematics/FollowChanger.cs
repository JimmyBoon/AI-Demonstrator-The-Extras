using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras.Character;
using Cinemachine;

namespace Extras.Cinematics
{
    public class FollowChanger : MonoBehaviour
    {
        List<CharacterBrian> characters = new List<CharacterBrian>();
        List<CameraZoneController> zoneControllers = new List<CameraZoneController>();
        [SerializeField] CinemachineStateDrivenCamera stateDrivenCamera;
        [SerializeField] bool panoramicCameraOn = false;
        [SerializeField] GameObject panoramicCamera;
        [SerializeField] bool autoChange = true;
        [SerializeField] float changeTime = 15f;
        float timeLapse = 0f;

        Animator animator;
        GameObject characterBeginFollowed;

        int characterToFollow = 0;

        private void Awake()
        {
            characters.AddRange(FindObjectsOfType<CharacterBrian>());
            zoneControllers.AddRange(FindObjectsOfType<CameraZoneController>());

            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SetCharacterToFollow();
        }

        private void Update()
        {
            timeLapse += Time.deltaTime;

            if(autoChange && timeLapse > changeTime)
            {
                timeLapse = 0f;
                ChangeCharacter();
            }
        }



        private void SetCharacterToFollow()
        {
            stateDrivenCamera.m_Follow = characters[characterToFollow].transform;
            stateDrivenCamera.m_LookAt = characters[characterToFollow].GetComponentInChildren<CameraAimPoint>().transform;
            stateDrivenCamera.m_AnimatedTarget = characters[characterToFollow].GetComponent<Animator>();

            foreach (CameraZoneController controller in zoneControllers)
            {
                controller.SetTargetToFollow(characters[characterToFollow].gameObject);
            }

            animator.SetTrigger("Transition");
            //animator.ResetTrigger("Transition");

        }

        public void ChangeCharacter()
        {
            if(characterToFollow == characters.Count -1)
            {
                characterToFollow = 0;
                panoramicCameraOn = true;
            }
            else
            {
                characterToFollow++;
                panoramicCameraOn = false;
            }
            ActivatePanoramicCamera();
            SetCharacterToFollow();
        }

        private void ActivatePanoramicCamera()
        {
            if(panoramicCameraOn)
            {
                panoramicCamera.SetActive(true);
            }
            else
            {
                panoramicCamera.SetActive(false);
            }
        }

        public GameObject GetCharacterBeginFollowed()
        {
            return characters[characterToFollow].gameObject;
        }

    
    }
}