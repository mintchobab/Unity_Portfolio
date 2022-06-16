using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy 
{
    public class WorldUIChecker : MonoBehaviour
    {
        // 1.������ NPC�� ��ȣ�ۿ� ������Ʈ�� ������ 
        // 2.�ش� ������Ʈ�� �̸��� ���
        // 3.�������� ����� ����
        // 4.�ѹ��� �����ϴ°� �ʿ�
        // 5.�ѹ��� �����ϴ°� �ʿ�(������ ��)


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
