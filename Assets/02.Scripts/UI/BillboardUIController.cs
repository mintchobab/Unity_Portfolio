using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public struct BillboardTarget
    {
        public GameObject Obj;
        public Transform TargetTransform;
        public Vector3 Position;
    }


    public class BillboardUIController : SceneUI
    {
        private List<BillboardTarget> targetList = new List<BillboardTarget>();
        private PlayerController player;


        protected override void Awake()
        {
            base.Awake();

            player = FindObjectOfType<PlayerController>();
            Show();
        }


        private void LateUpdate()
        {
            foreach (var target in targetList)
            {
                target.Obj.transform.position = Camera.main.WorldToScreenPoint(target.TargetTransform.position + target.Position);
            }
        }


        public void AddTarget(GameObject obj, Transform target, Vector3 pos)
        {
            BillboardTarget billboardTarget = new BillboardTarget();
            billboardTarget.Obj = obj;
            billboardTarget.TargetTransform = target;
            billboardTarget.Position = pos;

            targetList.Add(billboardTarget);
        }


        public void RemoveTarget(GameObject obj)
        {
            BillboardTarget target = targetList.Find(x => x.Obj == obj);
            targetList.Remove(target);
        }


        // 리스트에 추가하기
        // 리스트에서 삭제하기
        // 각각 생성 삭제는 빌보드 스크립트에서 처리하기
    }
}
