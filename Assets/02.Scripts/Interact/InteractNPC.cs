using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 대사만 가지고 있는 NPC
public class InteractNPC : InteractBase
{
    // NPC 정보 
    [field: SerializeField]
    public int NPCId { get; private set; }

    protected DialogueUIController dialogueController;
    protected NPC npc;    

    protected string imageName = "Dialogue";



    //이름이나 키를 가지고 있으면 데이터랑 대사를 처음부터 다 가져오자....
    //굳이 매번 찾을 필요가 없을거 같다.

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
