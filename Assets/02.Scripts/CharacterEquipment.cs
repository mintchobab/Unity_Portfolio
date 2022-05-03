using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterEquipment : MonoBehaviour
{
    [SerializeField]
    private Dictionary<EquipmentType, Transform> equipmentDic = new Dictionary<EquipmentType, Transform>();

    [SerializeField]
    private GameObject hair;


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
        // ������ ������ ����
        GameObject obj = Instantiate(Resources.Load<GameObject>($"Equipment/{item.ItemData.Name}"));
        obj.gameObject.name = item.ItemData.Name;

        // ������ ��ġ ����
        EquipmentType equipmentType = (item.ItemData as EquipmentData).EquipmentType;
        obj.transform.parent = equipmentDic[equipmentType];
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;


        if (equipmentType == EquipmentType.Helmet)
            hair.gameObject.SetActive(false);
    }


    public void UnEquip(EquipmentType equipmentType)
    {
        Transform child = equipmentDic[equipmentType].GetChild(0);

        if (child)
        {
            Destroy(child.gameObject);
        }

        if (equipmentType == EquipmentType.Helmet)
            hair.gameObject.SetActive(true);
    }
}

