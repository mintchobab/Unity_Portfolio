using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    // 여기서 오브젝트를 생성하기

    public class WorldUIName : MonoBehaviour, IWorldUI
    {
        [SerializeField]
        private NameType nameType;

        [SerializeField]
        private string nameKey;

        [field: SerializeField]
        public Vector3 LocalPosition { get; private set; }

        private WorldUINameCanvas nameCanvas;


        private void Awake()
        {
            nameCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUINameCanvas>(ResourcePath.WorldNameCanvas);

            string name = string.Empty;
            string colorString = string.Empty;            

            switch (nameType)
            {
                case NameType.Player:
                    break;

                case NameType.Npc:
                    name = StringManager.GetLocalizedNPCName(nameKey);
                    colorString = ValueData.npcNameColorString;
                    break;

                case NameType.Monster:
                    break;

                case NameType.Collection:
                    name = StringManager.GetLocalizedItemName(nameKey);
                    colorString = ValueData.collectionNameColorString;
                    break;
            }

            ColorUtility.TryParseHtmlString(colorString, out Color color);

            nameCanvas.SetName(name);
            nameCanvas.SetNameColor(color);
            nameCanvas.SetParent(transform);
            nameCanvas.SetLocalPosition(LocalPosition);
            nameCanvas.Hide();
        }


        public void Show()
        {
            nameCanvas.Show();
        }


        public void Hide()
        {
            nameCanvas.Hide();
        }
    }
}
