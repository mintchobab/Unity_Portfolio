using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy 
{
    public class SystemUIController : SceneUI
    {
        [SerializeField]
        private Image fadeImage;

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private Color systemTextOriginColor;

        [SerializeField]
        private Text[] systemTexts;

        [SerializeField]
        private Text endText;


        private Queue<Text> systemTextQueue = new Queue<Text>();
        private List<Text> activatedSystemTextQueue = new List<Text>();
        private Coroutine fade;

        private Vector2 startPos = new Vector2(0f, -170f);

        private readonly float messageFadeTime = 4f;
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


        public void FadeScreen(Color start, Color end, float time)
        {
            if (fade != null)
                StopCoroutine(fade);

            fade = StartCoroutine(Fade(start, end, time));
        }


        private IEnumerator Fade(Color start, Color end, float time)
        {
            float t = 0f;

            fadeImage.color = start;

            while (t < 1f)
            {
                t += Time.deltaTime / time;
                fadeImage.color = Color.Lerp(start, end, t);

                yield return null;
            }
        } 


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


        private IEnumerator FadeText(Text text)
        {
            float fadeTime = messageFadeTime;
            text.gameObject.SetActive(true);

            yield return new WaitForSeconds(fadeTime);

            text.gameObject.SetActive(false);

            activatedSystemTextQueue.Remove(text);
            systemTextQueue.Enqueue(text);
        }


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
            Vector3 targetPosition = Managers.Instance.UIManager.MainUIController.InventoryButton.transform.position;

            while (time < 1)
            {
                time += Time.deltaTime / targetTime;
                itemImage.transform.position = Vector3.Lerp(currentPosition, targetPosition, time);
                yield return null;
            }

            itemImage.gameObject.SetActive(false);
        }


        public void ShowEndText()
        {
            endText.gameObject.SetActive(true);
        }
    }
}
