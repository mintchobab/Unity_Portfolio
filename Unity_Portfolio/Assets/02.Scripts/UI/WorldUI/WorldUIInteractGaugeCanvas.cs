using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class WorldUIInteractGaugeCanvas : MonoBehaviour
    {
        [SerializeField]
        private Slider timeSlider;

        [SerializeField]
        private Slider interactGaugeSlider;

        [SerializeField]
        private InteractGaugeBubble gaugeBubble;

        [SerializeField]
        private AnimationCurve animationCurve;

        [SerializeField]
        private List<Transform> gaugeBubblePositionList;


        public event Action Successed;
        public event Action Failed;

        private Canvas canvas;

        private bool isSuccess;
        private bool isFail;

        private Color failColor;
        private Color successColor;

        private readonly int sliderDivision = 5;
        private readonly float randomFailCheckTime = 0.7f;
        private readonly string failColorString = "#CB5F5F";
        private readonly string successColorString = "#C6EC98";


        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            ColorUtility.TryParseHtmlString(failColorString, out failColor);
            ColorUtility.TryParseHtmlString(successColorString, out successColor);
        }


        private void OnEnable()
        {
            canvas.enabled = true;
            InitailizeEvents();
        }

        private void OnDisable()
        {
            canvas.enabled = false;
            StopAllCoroutines();
        }


        public void InitailizeEvents()
        {
            Successed = null;
            Failed = null;
        }


        public void StartProcess(float successChance, float processTime, float limitTime)
        {
            isSuccess = false;
            isFail = false;

            StartCoroutine(InteractGaugeProcess(successChance, processTime));
            StartCoroutine(TimeGaugeProcess(limitTime));
        }


        private IEnumerator TimeGaugeProcess(float limitTime)
        {
            float time = 0f;

            while (time < 1)
            {
                time += Time.deltaTime / limitTime;
                timeSlider.value = Mathf.Lerp(1f, 0f, time);
                yield return null;
            }

            if (!isSuccess)
            {
                isFail = true;
                Failed?.Invoke();
            }
        }


        private IEnumerator InteractGaugeProcess(float successChance, float processTime)
        {
            float sliderValue = 0f;

            float processOnceTime = sliderDivision / processTime;
            float sliderAddValue = 1f / sliderDivision;

            interactGaugeSlider.value = 0f;
            gaugeBubble.gameObject.SetActive(false);

            for (int i = 0; i < sliderDivision; i++)
            {
                float time = 0f;
                bool isRandomFail = false;
                bool isRandomCheck = false;

                while (time < 1)
                {
                    // 일정확률로 실패하기
                    if (time > randomFailCheckTime && !isRandomCheck)
                    {
                        isRandomCheck = true;

                        float ranValue = UnityEngine.Random.Range(0f, 1f);
                        if (ranValue > successChance)
                        {
                            isRandomFail = true;
                            break;
                        }
                    }

                    time += Time.deltaTime / processOnceTime;
                    interactGaugeSlider.value = sliderValue + animationCurve.Evaluate(time) * sliderAddValue;
                    yield return null;
                }

                // 중간 실패
                if (isRandomFail)
                {
                    i--;

                    interactGaugeSlider.value = sliderValue;

                    if (!gaugeBubble.gameObject.activeSelf)
                        gaugeBubble.gameObject.SetActive(true);

                    if (i < 0)
                        gaugeBubble.transform.position = gaugeBubblePositionList[0].position;
                    else
                        gaugeBubble.transform.position = gaugeBubblePositionList[i + 1].position;

                    gaugeBubble.ChangeBubbleColor(failColor);
                    gaugeBubble.ChangeResultText(StringManager.GetLocalizedUIText("Text_Fail"));

                    continue;
                }

                sliderValue += sliderAddValue;

                if (!gaugeBubble.gameObject.activeSelf)
                    gaugeBubble.gameObject.SetActive(true);

                gaugeBubble.transform.position = gaugeBubblePositionList[i + 1].position;
                gaugeBubble.ChangeBubbleColor(successColor);
                gaugeBubble.ChangeResultText(StringManager.GetLocalizedUIText("Text_Success"));
            }

            if (!isFail)
            {
                isSuccess = true;
                Successed?.Invoke();
            }
        }
    }
}
