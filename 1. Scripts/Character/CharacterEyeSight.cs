using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extras.Objects;

namespace Extras.Character
{


    public class CharacterEyeSight : MonoBehaviour
    {
        [Tooltip("Add a Game Object where the character will look from")]
        [SerializeField] GameObject eyes = null;
        [Tooltip("How far the character will see")]
        [SerializeField] float seeingRange = 20f;
        [Tooltip("Sight angle will only be up to 180 degrees. With 0 begin directly forward and 90 begin able to see completely left or right")]
        [SerializeField(), Range(0, 180)] float sightAngle = 80f;


        Vector3 heading;
        float angle;

        public ObjectOfInterest Look()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, seeingRange, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.GetComponent<ObjectOfInterest>() && InSightAngle(hit.transform.gameObject))
                {
                    if (InSight(hit.transform.gameObject))
                    {
                        return hit.transform.GetComponent<ObjectOfInterest>();
                    }
                }
            }

            return null;
        }

        //Seeing

        private bool InSightAngle(GameObject target)
        {
            if (target == null) { return false; }
            heading = target.transform.position - eyes.transform.position;
            angle = Vector3.Angle(heading, eyes.transform.forward);
            return angle < sightAngle;
        }

        private bool InSight(GameObject target)
        {
            if (target == null) { return false; }

            RaycastHit hit;
            if ((target.transform.position - eyes.transform.position).sqrMagnitude >= seeingRange * seeingRange || !InSightAngle(target)) { return false; }

            return Physics.Linecast(eyes.transform.position, target.transform.position, out hit) && hit.transform.gameObject.name == target.gameObject.name;
        }
    }
}