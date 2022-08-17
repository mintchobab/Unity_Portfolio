using UnityEngine;

namespace lsy 
{
    public class WorldUIBillboard : MonoBehaviour
    {
        private Camera cam;

        private void Awake()
        {
            cam = FindObjectOfType<WorldUICamera>().GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            transform.rotation = cam.transform.rotation;
        }

    }
}

