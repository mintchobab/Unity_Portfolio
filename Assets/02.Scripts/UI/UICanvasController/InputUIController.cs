using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace lsy
{
    public class InputUIController : SceneUI
    {
        [SerializeField]
        private Button interactButton;

        [field: SerializeField]
        public Button InventoryButton { get; private set; }

        [SerializeField]
        private Button equipButton;

        //[SerializeField]
        //private GameObject

        [SerializeField]
        private CombatButtonController combatButtonController;

        private Sprite basicButtonSprite;
        private Sprite stopButtonSprite;


        private event Action onInteracting;


        public Vector3 TouchRotateVector { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            LoadSprite();
            Show();
        }


        private void Start()
        {
            InventoryButton.onClick.AddListener(InventoryButtonClicked);
            equipButton.onClick.AddListener(EquipButtonClicked);
        }


        // ��ư �̹��� �ε�
        private void LoadSprite()
        {
            basicButtonSprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconBasic);
            stopButtonSprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconStop);
        }


        // ��ȣ�ۿ� �⺻ ��ư���� ��ü
        public void SetBasicInteractButton()
        {
            interactButton.image.sprite = basicButtonSprite;
            interactButton.onClick.RemoveAllListeners();
        }


        // ��ȣ�ۿ� ���� ��ư���� ��ü
        public void SetStopInteractButton(Action stopInteract)
        {
            interactButton.image.sprite = stopButtonSprite;

            onInteracting = null;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => stopInteract?.Invoke());
        }


        // ��ȣ�ۿ� ���� ��ư���� ��ü
        public void SetInteractButton(IInteractable interactable)
        {
            interactButton.image.sprite = interactable.LoadButtonImage();

            onInteracting = interactable.Interact;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => onInteracting?.Invoke());
        }


        public void SetCombatReadyButton(Action action)
        {
            interactButton.image.sprite = Managers.Instance.ResourceManager.Load<Sprite>(ResourcePath.IconCombat);

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(action.Invoke);
        }


        // �κ��丮 ��ư Ŭ��
        private void InventoryButtonClicked()
        {
            Managers.Instance.UIManager.InventoryUIController.Show();
            Hide();
        }


        // ����κ��丮 ��ư Ŭ��
        private void EquipButtonClicked()
        {
            Managers.Instance.UIManager.EquipUIController.Show();
            Hide();
        }


        // ī�޶� ȸ���� ���� ���Ͱ� ����
        public void SetTouchVector(Vector3 vec)
        {
            TouchRotateVector = vec;
        }


        public void ChangeSkill(List<SkillData> skillDatas)
        {
            if (combatButtonController == null)
                Debug.LogWarning("����");

            combatButtonController.ChangeSkillButton(skillDatas);
        }

    }
}
