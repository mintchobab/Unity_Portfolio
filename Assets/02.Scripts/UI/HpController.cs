using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lsy 
{
    public class HpController : MonoBehaviour
    {
        [SerializeField]
        private bool isShowDamage;

        [SerializeField]
        private Vector3 damagePosition;

        [SerializeField]
        private Material rimMaterial;


        [Header("Hp Bar")]
        [SerializeField]
        private float hpBarHeight;


        private Renderer myRenderer;
        private Material originMaterial;
        private WorldUIHpCanvas hpCanvas;


        public event Action onTakeDamage;
        public event Action onDead;


        private int currentHp;
        private int maxHp;
        private int defensivePower;


        public bool IsDead { get; private set; }



        // MaxHp구하는것도 추가하기..
        private void Start()
        {
            myRenderer = GetComponentInChildren<Renderer>();
            IHpHasable hpHasable = GetComponent<IHpHasable>();

            originMaterial = myRenderer.material;

            maxHp = hpHasable.GetMaxHp();
            defensivePower = hpHasable.GetDefensivePower();
            currentHp = maxHp;

            MakeWorldHpUI(maxHp);
        }



        public void ChangeMaxHp(int maxHp)
        {
            this.maxHp = maxHp;

            hpCanvas.MakePartitions(maxHp);
        }


        public void ChangeDeffensivePower(int defensivePower)
        {
            this.defensivePower = defensivePower;
        }



        // 데미지를 받았을 때 처리
        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;

            damage -= defensivePower;

            if (damage < 0)
                damage = 0;

            // 플로팅데미지 UI 생성
            if (isShowDamage)
            {
                string name = Managers.Instance.PoolManager.GetOriginPrefabName(ResourcePath.DamageText);
                if (name != null)
                {
                    Poolable poolable = Managers.Instance.PoolManager.Pop(name);
                    poolable.GetComponent<DamageText>().ShowDamage(transform.position + damagePosition, damage);
                }
            }            

            if (rimMaterial)
            {
                StartCoroutine(ChangeMaterialToRimLight());
            }


            currentHp -= damage;
            if (currentHp <= 0)
            {
                IsDead = true;

                hpCanvas.ChangeHp(0);
                onDead?.Invoke();
                onDead = null;

                return;
            }

            hpCanvas.ChangeHp(currentHp);
            onTakeDamage?.Invoke();
        }


        // 피격시 RimLight 효과 적용
        private IEnumerator ChangeMaterialToRimLight()
        {
            myRenderer.material = rimMaterial;
            yield return new WaitForSeconds(0.8f);
            myRenderer.material = originMaterial;
        }


        // World Hp UI 생성
        private void MakeWorldHpUI(int maxHp)
        {
            hpCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUIHpCanvas>(ResourcePath.WorldHpCanvas);
            hpCanvas.transform.SetParent(transform);
            hpCanvas.transform.localPosition = Vector3.zero + new Vector3(0f, hpBarHeight, 0f);

            hpCanvas.Init(maxHp);
        }
    }
}
