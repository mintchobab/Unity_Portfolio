using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class InteractCircle : MonoBehaviour
    {
        private BillboardUIController billboardUIController => Managers.Instance.UIManager.BillboardUIController;

        public void Show(Transform target)
        {
            billboardUIController.AddTarget(gameObject, target, Vector3.zero);

            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }


        public void Hide()
        {
            billboardUIController.RemoveTarget(gameObject);

            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }
}
