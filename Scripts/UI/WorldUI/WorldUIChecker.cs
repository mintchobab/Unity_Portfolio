using UnityEngine;

namespace lsy 
{
    public class WorldUIChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            IWorldUI worldUI = other.GetComponent<IWorldUI>();

            if (worldUI != null)
            {
                worldUI.Show();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IWorldUI worldUI = other.GetComponent<IWorldUI>();

            if (worldUI != null)
            {
                worldUI.Hide();
            }
        }
    }
}
