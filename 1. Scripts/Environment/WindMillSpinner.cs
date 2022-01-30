using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMillSpinner : MonoBehaviour
{
    [SerializeField] float windMillSpeed = 0.1f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, windMillSpeed);
    }
}
