using UnityEngine;

namespace lsy
{
    public class PlayerEquipController : MonoBehaviour
    {
        [SerializeField]
        private Transform leftHand;

        [SerializeField]
        private Transform rightHand;

        private GameObject currentTool;


        // 도구 생성
        public void MakeTool(InteractType interactType, bool isLeft)
        {
            // 어떤 도구를 생성하기 => 인터렉트 타입으로 구분?
            // 아이템 로드하기
            Transform parent = isLeft ? leftHand : rightHand;

            switch (interactType)
            {
                case InteractType.Pickaxing:
                    currentTool = Managers.Instance.ResourceManager.Instantiate<GameObject>(ResourcePath.Pickax, parent);
                    //currentTool.transform.localPosition = new Vector3(0, 0, 0);
                    //currentTool.transform.localRotation = Quaternion.identity;
                    break;
            }
        }


        public void DestoryCurrentTool()
        {
            Destroy(currentTool);
        }
    }
}
