using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterEquipment : MonoBehaviour
{
    [SerializeField]
    private Dictionary<EquipmentType, Transform> equipmentDic = new Dictionary<EquipmentType, Transform>();


    private void Awake()
    {
        SetParts();
    }

    private void SetParts()
    {
        CharacterEquipmentParts[] parts = FindObjectsOfType<CharacterEquipmentParts>();

        foreach(var part in parts)
        {
            equipmentDic.Add(part.EquipmentType, part.transform);
        }
    }


    public void Equip(Item item)
    {
        // 아이템 프리팹 생성
        GameObject obj = Instantiate(Resources.Load<GameObject>($"Equipment/{item.ItemData.Name}"));
        obj.gameObject.name = item.ItemData.Name;

        // 프리팹 위치 지정
        EquipmentType itemType = (item.ItemData as EquipmentData).EquipmentType;
        obj.transform.parent = equipmentDic[itemType];
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }


    public void UnEquip(EquipmentType equipmentType)
    {
        Transform child = equipmentDic[equipmentType].GetChild(0);

        if (child)
        {
            Destroy(child.gameObject);
        }        
    }
}

