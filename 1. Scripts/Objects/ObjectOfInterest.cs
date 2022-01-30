using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extras.Objects
{
    public class ObjectOfInterest : MonoBehaviour
    {
        [SerializeField] private ObjectType objectType;
        [SerializeField] private CultureType cultureType;
 //       [SerializeField] private GameObject ownedBy = null;
        [SerializeField] private Transform goToPoint;

        public ObjectType GetObjectType()
        {
            return objectType;
        }

        // public void SetOwnedBy(GameObject owner)
        // {
        //     ownedBy = owner;
        // }

        // public GameObject GetOwnedBy()
        // {
        //     return ownedBy;
        // }

        public Transform GetGoToPoint()
        {
            return goToPoint;
        }

        public CultureType GetCultureType()
        {
            return cultureType;
        }
    }
}
