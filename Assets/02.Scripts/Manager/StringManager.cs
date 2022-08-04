using System.Collections.Generic;


namespace lsy 
{
    public class StringManager : IManager
    {
        private static JsonString jsonString;

        // 번호에 따라 한글, 영어 바뀜
        private static int localizeIndex = 0;


        public void Init()
        {
            jsonString = Managers.Instance.JsonManager.jsonString;

            string language = UnityEngine.PlayerPrefs.GetString("Language");
            localizeIndex = language.Equals("Korean") ? 0 : 1;
        }


        // 인덱스 값은 Localize에서 받아오기
        public static string GetLocalizedNPCName(string nameKey)
        {
            return jsonString.stringNpcNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedNpcDialogue(string nameKey)
        {
            return jsonString.stringNpcDialogues.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestName(string nameKey)
        {
            return jsonString.stringQuestNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestGoal(string nameKey)
        {
            return jsonString.stringQuestGoals.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestContent(string nameKey)
        {
            return jsonString.stringQuestContents.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedQuestDialogue(string nameKey)
        {
            return jsonString.stringQuestDialogues.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedItemName(string nameKey)
        {
            return jsonString.stringItemNames.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedItemExplanation(string nameKey)
        {
            return jsonString.stringItemExplanations.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedUIText(string nameKey)
        {
            return jsonString.stringUITexts.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedSystemMessage(string nameKey)
        {
            return jsonString.stringSystemMessages.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        }

        public static string GetLocalizedSkillName(string namekey)
        {
            return jsonString.stringSkillNames.Find(x => x.key.Equals(namekey)).values[localizeIndex];
        }

        //public static string GetLocalizedCollection(string nameKey)
        //{
        //    return jsonString.stringCollection.Find(x => x.key.Equals(nameKey)).values[localizeIndex];
        //}
    }
}
