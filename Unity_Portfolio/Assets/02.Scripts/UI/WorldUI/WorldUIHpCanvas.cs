using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace lsy
{
    public class WorldUIHpCanvas : MonoBehaviour
    {
        [SerializeField]
        private Transform partitionParent;

        [SerializeField]
        private RectTransform partitionPrefab;

        [SerializeField]
        private Slider hpSlider;

        private CombatManager combatManager => Managers.Instance.CombatManager;

        private int maxHp;
        private int currentHp;
        private float frameWidth;



        private void Awake()
        {
            frameWidth = GetComponentInParent<RectTransform>().sizeDelta.x;
        }


        private void OnEnable()
        {
            float hpValue = currentHp / (float)maxHp;

            if (!float.IsNaN(hpValue))
            {
                hpSlider.value = hpValue;
            }
        }


        public void Init(int maxHp, int currentHp)
        {
            this.currentHp = currentHp;

            MakePartitions(maxHp);

            combatManager.onStartedCombat += () => gameObject.SetActive(true);
            combatManager.onEndedCombat += () => gameObject.SetActive(false);

            if (!combatManager.IsCombating)
            {
                gameObject.SetActive(false);
            }
        }


        public void MakePartitions(int maxHp)
        {
            this.maxHp = maxHp;

            int count = maxHp / 100;
            int childCount = partitionParent.childCount;

            if (childCount < count)
            {
                for (int i = 0; i < count - childCount; i++)
                {
                    Instantiate(partitionPrefab, partitionParent);
                }
            }
            else if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
                    partitionParent.GetChild(i).gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < count; i++)
            {
                RectTransform partition = partitionParent.GetChild(i).GetComponent<RectTransform>();
                partition.anchoredPosition = new Vector2((frameWidth / count) * i, 0);
                partition.gameObject.SetActive(true);
            }
        }


        // HP바 게이지 변경
        public void ChangeHp(int currentHp)
        {
            this.currentHp = currentHp;

            float hpValue = 0f;

            if (currentHp >= maxHp)
                hpValue = 1;
            else
                hpValue = currentHp / (float)maxHp;

            DOTween.Kill(transform);

            if (currentHp > 0)
            {
                hpSlider.DOValue(hpValue, 0.3f);
            }
            else
            {
                hpSlider.DOValue(0, 0.3f).OnComplete(() => gameObject.SetActive(false));
            }
        }
    }
}
