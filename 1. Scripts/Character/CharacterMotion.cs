using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Extras.Character
{

    public class CharacterMotion : MonoBehaviour
    {
        Animator animator;
        NavMeshAgent agent;

        [SerializeField] float walkSpeed = 1.6f;
        [SerializeField] float runSpeed = 5.6f;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }


        private void Update()
        {
            animator.SetFloat("forwardSpeed", agent.velocity.magnitude);
        }

        public void SetDestination(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        public void CancelDestination()
        {
            agent.ResetPath();
        }

        public void SetRun()
        {
            agent.speed = runSpeed;
        }

        public void SetWalk()
        {
            agent.speed = walkSpeed;
        }


        void FootL()
        {

        }

        void FootR()
        {

        }
    }
}
