using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace lsy
{
    public class TaskPatrol : Node
    {
        private Transform transform;
        private Transform[] waypoints;
        private Animator animator;

        private int currentWaypointIndex = 0;

        private float waitTime = 1f;
        private float waitCounter = 0f;
        private bool waiting = false;

        public TaskPatrol(Transform transform, Transform[] waypoints)
        {
            this.transform = transform;
            this.waypoints = waypoints;

            //animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            if (waiting)
            {
                waitCounter += Time.deltaTime;
                if (waitCounter >= waitTime)
                {
                    waiting = false;
                    //animator.SetBool();
                }
            }
            else
            {
                Transform wp = waypoints[currentWaypointIndex];
                if (Vector3.Distance(transform.position, wp.position) < 0.01f)
                {
                    transform.position = wp.position;
                    waitCounter = 0f;
                    waiting = true;

                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    //animator.SetBool()
                }
                else
                {
                    //transform.position = Vector3.MoveTowards(transform.position, wp.position, monst.Speed * Time.deltaTime);
                    transform.LookAt(wp.position);
                }
            }

            state = NodeState.Running;
            return state;
        }
    }
}
