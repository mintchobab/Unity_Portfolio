using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem : Singleton<EquipSystem>
{
    [SerializeField]
    private GameObject equipObj;

    private bool isActive;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isActive)
                Activate();
            else
                Deactivate();
        }
    }

    public void Activate()
    {
        isActive = true;
        equipObj.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        equipObj.SetActive(false);
    }


    // 아이템 장착
    // UI 장착 + 실제 캐릭터에도 바뀌어야함
    public void PutOnItem(Item item)
    {
        
    }

    // // 아이템 장착 해제
    public void TakeOffItem()
    {

    }

}
