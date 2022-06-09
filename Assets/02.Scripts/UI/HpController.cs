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



        // Canvas HP가 있는 경우와 없는 경우가 있음
        // 1.있는 경우 => 플레이어, 보스 or 엘리트 몬스터
        // 2.없는 경우 => 일반 몬스터
        public void ChangeHpUI(int currentHp, int maxHp)
        {
            // 일단은 World만 바꾸도록 하자
            float value = currentHp / (float)maxHp;
            hpBar.ChangeHpBar(value);
        }
    }
}
