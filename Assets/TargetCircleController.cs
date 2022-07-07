using UnityEngine;

namespace lsy
{
    public class TargetCircleController : MonoBehaviour
    {
        public WorldUIInteractCircleCanvas InteractCircleCanvas { get; private set; }

        private void Awake()
        {
            InteractCircleCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUIInteractCircleCanvas>(ResourcePath.WorldInteractCircleCanvas, transform);
            InteractCircleCanvas.Initialize(transform);
        }

        public void ShowCircle(Transform target, Vector3 addedPosition)
        {
            InteractCircleCanvas.Show(target, addedPosition);
        }

        public void HideCircle()
        {
            InteractCircleCanvas.Hide();
        }
    }
}
