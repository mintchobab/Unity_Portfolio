using System;
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

        private Sprite basicButtonSprite;
        private Sprite stopButtonSprite;
        private Sprite dialogueSprite;


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
        public void SetStopInteractButton(Action Stop)
        {
            interactButton.image.sprite = stopButtonSprite;

            onInteracting = null;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => Stop?.Invoke());
        }


        // ��ȣ�ۿ� ���� ��ư���� ��ü
        public void SetInteractButton(InteractBase interact)
        {
            interactButton.image.sprite = interact.LoadButtonImage();

            onInteracting = interact.Interact;

            interactButton.onClick.RemoveAllListeners();
            interactButton.onClick.AddListener(() => onInteracting?.Invoke());
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

    }
}
