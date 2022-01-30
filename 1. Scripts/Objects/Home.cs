using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Objects
{
    public class Home : MonoBehaviour
    {
        [SerializeField] GameObject ownedBy = null;
        [SerializeField] Transform sleepPosition;

        public void SetOwnedBy(GameObject owner)
        {
            ownedBy = owner;
        }

        public GameObject GetOwnedBy()
        {
            return ownedBy;
        }

        public Transform GetSleepPosition()
        {
            return sleepPosition;
        }
    }
}