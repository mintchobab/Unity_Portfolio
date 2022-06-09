using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class HpController : MonoBehaviour
    {
        [SerializeField]
        private float hpBarHeight;

        private BillboardUIController billboardController => Managers.Instance.UIManager.BillboardUIController;
        private HpBar hpBar;


        private void Start()
        {
            CreateWorldHpBar();
        }


        private void CreateWorldHpBar()
        {
            //hpBar = Managers.Instance.ResourceManager.Instantiate<HpBar>(ResourcePath.HpBar, billboardController.transform);
            //billboardController.AddTarget(hpBar.gameObject, new Vector3(0f, hpBarHeight, 0f));
        }



        // Canvas HP�� �ִ� ���� ���� ��찡 ����
        // 1.�ִ� ��� => �÷��̾�, ���� or ����Ʈ ����
        // 2.���� ��� => �Ϲ� ����
        public void ChangeHpUI(int currentHp, int maxHp)
        {
            // �ϴ��� World�� �ٲٵ��� ����
            float value = currentHp / (float)maxHp;
            hpBar.ChangeHpBar(value);
        }
    }
}
