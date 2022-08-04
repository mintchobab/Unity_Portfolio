using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy
{
    public class BirdController : MonoBehaviour
    {
        [SerializeField]
        public BoxCollider areaCollider;

        [SerializeField]
        private float minMoveDistance = 1f;

        [SerializeField]
        private float maxMoveDistance = 3f;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float rotSpeed;



        private InteractCollection interactCollection;
        private Animator anim;
        private Rigidbody rigid;

        private Vector3 nextPos;
        private Vector2 idleTimeRange = new Vector2(5f, 10f);

        private float elapsedTime;
        private float idleTime;


        private bool canMoving = true;
        private int hashMoveSpeed = Animator.StringToHash("moveSpeed");

        


        private void Awake()
        {
            anim                = GetComponent<Animator>();
            rigid               = GetComponent<Rigidbody>();
            interactCollection  = GetComponent<InteractCollection>();
        }


        private void Start()
        {
            Init();
        }


        private void OnEnable()
        {
            interactCollection.onInteract += OnInteract;
            interactCollection.onEndInteract += OnEndInteract;
        }

        private void OnDisable()
        {
            interactCollection.onInteract -= OnInteract;
            interactCollection.onEndInteract -= OnEndInteract;
        }


        private void FixedUpdate()
        {
            MoveRandomPos();
        }


        private void Init()
        {
            elapsedTime = 0f;

            idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
            nextPos = GetRandomNextPos();
        }


        private void OnInteract()
        {
            canMoving = false;

            nextPos = transform.position;
            elapsedTime = 0f;

            anim.SetFloat(hashMoveSpeed, 0f);
        }


        private void OnEndInteract()
        {
            canMoving = true;
        }


        private void MoveRandomPos()
        {
            if (!canMoving)
                return;

            // y축 값을 제외한 포지션으로 비교하기
            Vector3 tmpNextPos = nextPos;
            tmpNextPos.y = transform.position.y;

            if (Vector3.Distance(transform.position, tmpNextPos) > 0.01f)
            {
                anim.SetFloat(hashMoveSpeed, 1f);

                Quaternion lookRotation = Quaternion.LookRotation(tmpNextPos - transform.position).normalized;

                // 이동
                Vector3 dir = (nextPos - rigid.position).normalized;
                dir.y = 0f;
                rigid.MovePosition(rigid.position + dir * moveSpeed * Time.fixedDeltaTime);

                // 회전
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotSpeed * Time.deltaTime);
            }
            // 일정시간 대기
            else
            {
                anim.SetFloat(hashMoveSpeed, 0f);

                elapsedTime += Time.deltaTime;
                if (elapsedTime >= idleTime)
                {
                    Init();
                }                
            }
        }


        private Vector3 GetRandomNextPos()
        {
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            direction *= Random.Range(minMoveDistance, maxMoveDistance);

            Vector3 pos = transform.position + direction;

            bool isInner = areaCollider.bounds.Contains(pos);

            if (isInner)
                return pos;
            else
            {
                return transform.position;
            }
        }


    }
}
