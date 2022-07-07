using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    public class WorldUIName : MonoBehaviour, IWorldUI
    {
        [SerializeField]
        private NameType nameType;

        [SerializeField]
        private string nameKey;

        [field: SerializeField]
        public Vector3 LocalPosition { get; private set; }

        public WorldUINameCanvas NameCanvas { get; private set; }


        private void Awake()
        {
            NameCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUINameCanvas>(ResourcePath.WorldNameCanvas);

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

            NameCanvas.SetName(name);
            NameCanvas.SetNameColor(color);
            NameCanvas.SetParent(transform);
            NameCanvas.SetLocalPosition(LocalPosition);
            NameCanvas.Hide();
        }


        public void Show()
        {
            NameCanvas.Show();
        }


        public void Hide()
        {
            NameCanvas.Hide();
        }
    }
}
