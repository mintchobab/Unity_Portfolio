using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy 
{
    public class WorldUIChecker : MonoBehaviour
    {
        // 1.범위에 NPC나 상호작용 오브젝트가 들어오면 
        // 2.해당 오브젝트의 이름을 출력
        // 3.범위에서 벗어나면 삭제
        // 4.한번에 삭제하는게 필요
        // 5.한번에 생성하는게 필요(시작할 때)


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
