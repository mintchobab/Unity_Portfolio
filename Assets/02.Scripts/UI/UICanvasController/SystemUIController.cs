using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy 
{
    public class SystemUIController : SceneUI
    {
        [SerializeField]
        private Image itemImage;

        private Camera cam;

        protected override void Awake()
        {
            base.Awake();
            cam = Camera.main;
            itemImage.gameObject.SetActive(false);
        }


        // 아이템 획득 했을 때 인벤토리 창으로 들어가기
        // 아이템의 생성 위치, 아이템 아이콘 필요
        public void GetItem(InteractCollection interactCollection, Vector3 targetPostion)
        {
            string path = ResourcePath.GetItemSpritePathToType(interactCollection.MyCollectionData.itemType);

            //itemImage.sprite = Managers.Instance.ResourceManager.Load<Sprite>($"{path}/{interactCollection.InteractObjResourceName}");
            //StartCoroutine(MoveItemImage(targetPostion));
        }


        private IEnumerator MoveItemImage(Vector3 targetPos)
        {
            float targetTime = 0.4f;
            float time = 0f;

            itemImage.gameObject.SetActive(true);

            Vector3 currentPosition = Camera.main.WorldToScreenPoint(targetPos);
            Vector3 targetPosition = Managers.Instance.UIManager.InputUIController.InventoryButton.transform.position;

            while (time < 1)
            {
                time += Time.deltaTime / targetTime;
                itemImage.transform.position = Vector3.Lerp(currentPosition, targetPosition, time);
                yield return null;
            }

            itemImage.gameObject.SetActive(false);
        }
    }
}
