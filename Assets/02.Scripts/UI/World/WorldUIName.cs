using UnityEngine;
using UnityEngine.UI;

namespace lsy
{
    // 여기서 오브젝트를 생성하기

    public class WorldUIName : MonoBehaviour, IWorldUI
    {
        [SerializeField]
        private bool isNPC;

        [SerializeField]
        private string nameKey;

        [field: SerializeField]
        public Vector3 LocalPosition { get; private set; }

        private WorldUINameCanvas nameCanvas;


        private void Awake()
        {
            nameCanvas = Managers.Instance.ResourceManager.Instantiate<WorldUINameCanvas>(ResourcePath.WorldNameCanvas, transform);

            string name = isNPC ? StringManager.GetLocalizedNPCName(nameKey) : StringManager.GetLocalizedCollection(nameKey);
            nameCanvas.Initialize(name);
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
