using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��縸 ������ �ִ� NPC
public class InteractNPC : InteractBase
{
    // NPC ���� 
    [field: SerializeField]
    public int NPCId { get; private set; }

    protected DialogueUIController dialogueController;
    protected NPC npc;    

    protected string imageName = "Dialogue";



    //�̸��̳� Ű�� ������ ������ �����Ͷ� ��縦 ó������ �� ��������....
    //���� �Ź� ã�� �ʿ䰡 ������ ����.

    protected override void Awake()
    {
        base.Awake();
        SetNPCData();

        dialogueController = Managers.Instance.UIManager.DialogueController;
    }


    public override void Interact()
    {
        //dialogueController.SetNameText(StringManager.GetLocalizedNPCName(npc.name));
        ////dialogueController.SetCurrentDialogues(npc.dialogues[0]);
        //dialogueController.Show();
    }


    protected void SetNPCData()
    {
        JsonNPC dialogueData = Managers.Instance.JsonManager.jsonNPC;

        foreach (NPC npc in dialogueData.npcs)
        {
            if (npc.id.Equals(NPCId))
                this.npc = npc;
        }

        if (npc == null)
            Debug.LogError("NPC Data Not Found");
    }


    protected override void SetButtonImage()
    {
        buttonImage = Managers.Instance.ResourceManager.Load<Sprite>($"{folderPath}{imageName}");
    }
}
