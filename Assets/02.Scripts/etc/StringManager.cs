using System.Collections.Generic;

public class StringManager : IManager
{
    private static JsonString jsonString;

    // ��ȣ�� ���� �ѱ�, ���� �ٲ�
    private static int localizeIndex = 0;


    public void Initialize()
    {
        jsonString = Managers.Instance.JsonManager.jsonString;
    }


    // �ε��� ���� Localize���� �޾ƿ���
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



    //public static string GetLocalizedDialogue(string key)
    //{
    //    return dialogueDic[key][index];
    //}

    //public static string GetLocalizedQuestName(string key)
    //{
    //    return questNameDic[key][index];
    //}

    //public static string GetLocalizedQuestDescription(string key)
    //{
    //    return questDescriptionDic[key][index];
    //}
}
