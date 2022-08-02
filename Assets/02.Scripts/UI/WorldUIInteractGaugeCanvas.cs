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


        public event Action Failed;
        public event Action Successed;

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


        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Show()
        {
            canvas.enabled = true;
        }

        public void Hide()
        {
            canvas.enabled = false;
        }


        public void StartProcess(float successChance, float processTime, float limitTime)
        {
            isSuccess = false;
            isFail = false;

            // 초기화 하는것도 있어야함

            Show();

            StartCoroutine(InteractGaugeProcess(successChance, processTime));
            StartCoroutine(TimeGaugeProcess(limitTime));
        }



        // 제한 시간 게이지
        private IEnumerator TimeGaugeProcess(float limitTime)
        {
            float time = 0f;

            while (time < 1)
            {
                // 1 -> 0으로
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


        // 작업량 게이지
        private IEnumerator InteractGaugeProcess(float successChance, float processTime)
        {
            float sliderValue = 0f;

            float processOnceTime = sliderDivision / processTime;
            float sliderAddValue = 1f / sliderDivision;

            interactGaugeSlider.value = 0f;

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
                    gaugeBubble.ChangeResultText("실패");

                    continue;
                }

                sliderValue += sliderAddValue;

                if (!gaugeBubble.gameObject.activeSelf)
                    gaugeBubble.gameObject.SetActive(true);

                gaugeBubble.transform.position = gaugeBubblePositionList[i + 1].position;
                gaugeBubble.ChangeBubbleColor(successColor);
                gaugeBubble.ChangeResultText("성공");
            }

            if (!isFail)
            {
                isSuccess = true;
                Successed?.Invoke();
            }
        }
    }
}
