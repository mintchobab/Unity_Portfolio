using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;


namespace lsy 
{
    public class WolfMovePath : MonoBehaviour
    {
        [SerializeField]
        private Transform[] pathTr;

        private Vector3[] destinations;
        private NavMeshAgent agent;
        private Animator anim;

        private int groundLayerMask;

        private int hashMoveSpeed = Animator.StringToHash("moveSpeed");
        private int hashIsBarking = Animator.StringToHash("isBarking");


        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();

            groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

            SetAgentDestinations();
        }


        private void Start()
        {
            anim.SetFloat(hashMoveSpeed, 1f);
            transform.DOPath(destinations, 8f, PathType.CatmullRom)
                .SetLookAt(0f, true)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
                {
                    anim.SetFloat(hashMoveSpeed, 0f);
                    anim.SetBool(hashIsBarking, true);
                });
        }


        private void SetAgentDestinations()
        {
            destinations = new Vector3[pathTr.Length];

            for (int i = 0; i < pathTr.Length; i++)
            {
                Ray ray = new Ray(pathTr[i].position, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
                {
                    destinations[i] = hit.point;
                }
                else
                {
                    Debug.LogError("Path Error");
                }
            }
        }

    }
}
