using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Extras.Vehicle
{
    public class VehicleController : MonoBehaviour
    {

        NavMeshAgent agent;
        [SerializeField] List<Transform> destinations = new List<Transform>();
        [SerializeField] GameObject leftWheelFront;
        [SerializeField] GameObject leftWheelBack;

        [SerializeField] GameObject rightWheelFront;
        [SerializeField] GameObject rightWheelBack;

        [SerializeField] float speed = 0f;
        float x;
        float angularVelocity;
        int positionNumber = 0;
        bool delay = false;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(destinations[0].position);
        }

        private void Update()
        {
            speed = agent.velocity.magnitude;
            RotateWheels();
            

            if (delay) { return; }
            if (Vector3.Distance(transform.position, destinations[positionNumber].position) < 2f)
            {
                

                positionNumber++;

                if (positionNumber == destinations.Count)
                {
                    positionNumber = 0;
                }

                if (positionNumber == destinations.Count - 1)
                {
                    StartCoroutine(SetDestination(10f));
                }
                else
                {
                    agent.SetDestination(destinations[positionNumber].position);
                }
                

            }
        }

        IEnumerator SetDestination(float delayTime)
        {
            delay = true;
            yield return new WaitForSeconds(delayTime);
            agent.SetDestination(destinations[positionNumber].position);
            delay = false;

        }

        private void RotateWheels()
        {
            angularVelocity = (speed / 0.44f) * (180 / Mathf.PI);
            x = angularVelocity * Time.deltaTime;
            leftWheelFront.transform.Rotate(x, 0, 0);
            leftWheelBack.transform.Rotate(x, 0, 0);
            rightWheelFront.transform.Rotate(x, 0, 0);
            rightWheelBack.transform.Rotate(x, 0, 0);
        }
    }
}
