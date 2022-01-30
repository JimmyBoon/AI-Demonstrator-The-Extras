using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Cinematics
{
    public class CameraZoneController : MonoBehaviour
    {
        [SerializeField] GameObject targetToFollow;
        [SerializeField] Animator animator;
        [SerializeField] string triggerName;
        [SerializeField] bool selfDestructZone = false;
        [SerializeField] float destroyTime = 3f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == targetToFollow.gameObject)
            {
                animator.SetTrigger(triggerName);
                if (selfDestructZone)
                {
                    Destroy(gameObject, destroyTime);
                }
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == targetToFollow.gameObject)
            {
                animator.ResetTrigger(triggerName);

            }
        }

        public void SetTargetToFollow(GameObject target)
        {
            targetToFollow = target;
        }
    }
}