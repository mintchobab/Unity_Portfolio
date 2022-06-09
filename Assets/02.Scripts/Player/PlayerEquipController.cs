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


        // ���� ����
        public void MakeTool(InteractType interactType, bool isLeft)
        {
            // � ������ �����ϱ� => ���ͷ�Ʈ Ÿ������ ����?
            // ������ �ε��ϱ�
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
