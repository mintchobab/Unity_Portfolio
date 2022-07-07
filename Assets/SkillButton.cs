using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField]
        private Image skillImage;

        [SerializeField]
        private Image coolDownImage;

        [SerializeField]
        private Text nameText;


        private Button button;



        private void Awake()
        {
            button = GetComponent<Button>();
        }


        public void ChangeButton(Sprite sprite, string name)
        {
            // 버튼 클릭 이벤트도 변경하기

            //skillImage.sprite = sprite;
            //coolDownImage.sprite = sprite;
            //nameText.text = name;
        }
    }
}
