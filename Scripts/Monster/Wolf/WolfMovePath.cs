using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;


namespace lsy
{
    public class WolfMovePath : MonoBehaviour
    {
        [SerializeField]
        private Transform[] pathTr;

        private Vector3[] destinations;
        private Animator anim;

        private int groundLayerMask;
        private int hashMoveSpeed = Animator.StringToHash("moveSpeed");
        private int hashIsBarking = Animator.StringToHash("isBarking");


        private void Awake()
        {
            anim = GetComponent<Animator>();

            groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

            SetAgentDestinations();
        }


        private void OnDisable()
        {
            anim.SetBool(hashIsBarking, false);
        }


        private void Start()
        {
            float pathMoveTime = Random.Range(4f, 6f);

            anim.SetFloat(hashMoveSpeed, 1f);
            transform.DOPath(destinations, pathMoveTime, PathType.CatmullRom)
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
                    Vector3 destination = hit.point;
                    //destination.y += 0.3f;
                    destinations[i] = destination;
                }
                else
                {
                    Debug.LogError("Path Error");
                }
            }
        }

    }
}
