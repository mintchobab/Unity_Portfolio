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

        [SerializeField]
        private Color systemTextOriginColor;

        [SerializeField]
        private Text[] systemTexts;

        private Color systemTextTransparentColor;

        private Queue<Text> systemTextQueue = new Queue<Text>();
        private List<Text> activatedSystemTextQueue = new List<Text>();


        // 텍스트의 시작 위치
        private Vector2 startPos = new Vector2(0f, -170f);

        // 텍스트가 올라가는 높이
        private float height = 50f;


        protected override void Awake()
        {
            base.Awake();

            itemImage.gameObject.SetActive(false);

            for (int i = 0; i < systemTexts.Length; i++)
            {
                systemTexts[i].color = systemTextOriginColor;
                systemTextQueue.Enqueue(systemTexts[i]);
                systemTexts[i].gameObject.SetActive(false);
            }
        }



        // UI가 중첩될 수 있는 구조로 만들기........ㅣㄴ;ㅇ리;너ㅏㅇ러;ㅣㄴㅁㅇ러ㅣ;'ㄻㄴ어ㅣ;'ㄹㄴㅇ;'


        // 하나가 추가되면 이미 추가되어있는 애들 위치 다 위로 올리기
        // => 여러 텍스트가 한꺼번에 보일 수 있게
        // => 알파값 변경은 각자 알아서 할 수 있도록


        // 1.나타났다가
        // 2.일정시간이 지나면
        // 3.서서히 사라지기
        public void ShowSystemText(string messageKey)
        {
            Text systemText = systemTextQueue.Dequeue();
            systemText.text = StringManager.GetLocalizedSystemMessage(messageKey);
            systemText.rectTransform.anchoredPosition = startPos;
            systemText.gameObject.SetActive(true);

            for (int i = 0; i < activatedSystemTextQueue.Count; i++)
            {
                activatedSystemTextQueue[i].rectTransform.anchoredPosition += new Vector2(0f, height);
            }

            activatedSystemTextQueue.Add(systemText);

            StartCoroutine(FadeText(systemText));
        }


        // 페이드 기능은 고려해보기...
        private IEnumerator FadeText(Text text)
        {
            float fadeTime = ValueData.SystemMessageFadeTime;
            text.gameObject.SetActive(true);

            yield return new WaitForSeconds(fadeTime);

            text.gameObject.SetActive(false);

            activatedSystemTextQueue.Remove(text);
            systemTextQueue.Enqueue(text);
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
