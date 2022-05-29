using System.Collections.Generic;

namespace lsy 
{
    public class StringManager : IManager
    {
        private static JsonString jsonString;

        // 번호에 따라 한글, 영어 바뀜
        private static int localizeIndex = 1;


        public void Initialize()
        {
            jsonString = Managers.Instance.JsonManager.jsonString;
        }


        // 인덱스 값은 Localize에서 받아오기
        public static string GetLocalizedNPCName(string nameKey)
        {
            return jsonString.stringNpcNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestName(string nameKey)
        {
            return jsonString.stringQuestNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestDescription(string nameKey)
        {
            return jsonString.stringQuestDescriptions.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestDialogue(string nameKey)
        {
            return jsonString.stringQuestDialogues.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedItemName(string nameKey)
        {
            return jsonString.stringItemNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedItemDescription(string nameKey)
        {
            return jsonString.stringItemDescriptions.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedUIText(string nameKey)
        {
            return jsonString.stringUITexts.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }
    }
}
