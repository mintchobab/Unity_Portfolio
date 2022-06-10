using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace lsy
{
    public class RotateTouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Vector3 prevPos;
        private float mouseDragDist;        
        private float mouseSensitivity = 0.25f;

        private Coroutine rotateCamera;
        private InputUIController inputUIController => Managers.Instance.UIManager.InputUIController;


        public void OnPointerDown(PointerEventData eventData)
        {
            if (rotateCamera != null)
                StopCoroutine(rotateCamera);

            rotateCamera = StartCoroutine(ChangeTouchVector());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (rotateCamera != null)
                StopCoroutine(rotateCamera);            
        }


        private IEnumerator ChangeTouchVector()
        {
            prevPos = Input.mousePosition;

            while (true)
            {
                Vector3 newPos = Input.mousePosition;
                Vector3 dist = newPos - prevPos;

                mouseDragDist += dist.x * mouseSensitivity;

                inputUIController.SetTouchVector(new Vector3(0f, mouseDragDist, 0f));
                prevPos = newPos;
                yield return null;
            }
        }
    }
}
