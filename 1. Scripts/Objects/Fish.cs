using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour

{
    [SerializeField] float turnSpeed = 200f;
    [SerializeField] float speed = 10f;


    [SerializeField] float avoidanceRange = 5f;
    [SerializeField] float seeingRange = 12f;
    [SerializeField] float sightAngle = 60f;
    [SerializeField] float allignAngle = 40f;
    [SerializeField] float allignSpeed = 100f;
    [SerializeField] float allignOffest = 10f;
    [SerializeField] float centerAllignSpeed = 10f;
    [SerializeField] int numberOfRays = 10;
    [SerializeField] GameObject eyes;

    [SerializeField] bool centerSeek;
    [SerializeField] Vector3 center;
    [SerializeField] float centerSeekAngle = 150f;
    [SerializeField] GameObject particalPrefab;
    [SerializeField] bool flocking = true;
    [SerializeField] Transform destination;
    [SerializeField] float normalSpeed;

    float speedCooldown = 0f;
    float angle;



    void Start()
    {
        normalSpeed = speed;
    }


    void Update()
    {
        if (transform.position.x > Mathf.Abs(50f) || transform.position.y > Mathf.Abs(50f))
        {
            transform.position = new Vector3(-15f, 16f, 4f);
        }

        if (flocking)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            AvoidCollisions();
            AllignDirection();
        }


        speedCooldown -= Time.deltaTime;
        if (speedCooldown < 0)
        {
            speed = normalSpeed;
        }

    }

    private void AllignDirection()
    {
        GameObject closestSheep = null;
        float closestDistance = Mathf.Infinity;
        center = transform.position;

        RaycastHit[] nearBys = Physics.SphereCastAll(transform.position, seeingRange, Vector3.up, 0);
        foreach (RaycastHit near in nearBys)
        {
            if (near.transform.GetComponent<Fish>() && near.transform.gameObject != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, near.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSheep = near.transform.gameObject;
                }

                center += near.transform.position;
            }
        }





        if (closestSheep != null)
        {
            Vector3 heading = closestSheep.transform.position - transform.position;
            float closeAngle = Vector3.Angle(heading, transform.forward);

            if (closeAngle < allignAngle)
            {

                Vector3 sheepDirection = closestSheep.transform.forward * allignOffest - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, sheepDirection, allignSpeed * Time.deltaTime, 0.00f);
                //Debug.DrawRay(closestSheep.transform.position, closestSheep.transform.forward * 10, Color.blue);
                //Debug.DrawRay(transform.position, newDirection * 10, Color.blue);
                transform.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));
            }
        }

        if (centerSeek)
        {
            center = center / nearBys.Length;
        }

        if (nearBys.Length > 1 && centerSeek)
        {
            Vector3 heading = center - transform.position;
            float closeAngle = Vector3.Angle(heading, transform.forward);

            if (closeAngle < centerSeekAngle)
            {
                Vector3 centerDirection = center - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, centerDirection, centerAllignSpeed * Time.deltaTime, 0.00f);
                transform.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));
                Debug.DrawRay(transform.position, newDirection * 10, Color.red);
            }
        }
    }

    private void AvoidCollisions()
    {
        float angle = sightAngle / numberOfRays;

        for (int i = 1; i < numberOfRays; i++)
        {
            bool hit = false;
            RaycastHit hitInfo;
            hit = Physics.Raycast(transform.position, avoidanceRange * transform.TransformVector(Mathf.Sin(((Mathf.Pow(-1, i) * angle * i) * Mathf.PI / 180)), 0, Mathf.Cos((Mathf.Pow(-1, i) * angle * i) * Mathf.PI / 180)), out hitInfo, avoidanceRange);
            Debug.DrawRay(transform.position, avoidanceRange * transform.TransformVector(Mathf.Sin(((Mathf.Pow(-1, i) * angle * i) * Mathf.PI / 180)), 0, Mathf.Cos((Mathf.Pow(-1, i) * angle * i) * Mathf.PI / 180)), Color.red);

            if (hit && hitInfo.transform.gameObject != this.gameObject)
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                transform.Rotate(Vector3.up * turnSpeed * (Mathf.Pow(-1, i + 1)));
            }
        }
    }
}
