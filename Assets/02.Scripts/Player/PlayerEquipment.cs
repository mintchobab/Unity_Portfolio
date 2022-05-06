using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerEquipment : MonoBehaviour
{
    [SerializeField]
    private Dictionary<EquipmentType, Transform> equipmentDic = new Dictionary<EquipmentType, Transform>();

    [SerializeField]
    private GameObject hair;

    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;

    [SerializeField]
    private SkinnedMeshRenderer armorLeather;

    [SerializeField]
    private SkinnedMeshRenderer armorSilver;

    [SerializeField]
    private SkinnedMeshRenderer armorGold;


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
        EquipmentType equipmentType = (item.ItemData as EquipmentData).EquipmentType;

        if (equipmentType == EquipmentType.Armor)
        {
            EquipArmor(item);
            return;
        }

        // 아이템 프리팹 생성
        GameObject obj = Instantiate(Resources.Load<GameObject>($"Equipment/{item.ItemData.Name}"));
        obj.gameObject.name = item.ItemData.Name;

        // 프리팹 위치 지정        
        obj.transform.parent = equipmentDic[equipmentType];
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        if (equipmentType == EquipmentType.Helmet)
            hair.gameObject.SetActive(false);
    }


    public void UnEquip(EquipmentType equipmentType)
    {
        if (equipmentType == EquipmentType.Armor)
        {
            UnEquipArmor(equipmentType);
            return;
        }

        Transform child = equipmentDic[equipmentType].GetChild(0);

        if (child)
        {
            Destroy(child.gameObject);
        }

        if (equipmentType == EquipmentType.Helmet)
            hair.gameObject.SetActive(true);
    }


    private void EquipArmor(Item item)
    {
        if (item.ItemData.Name.Equals("Armor Leather"))
        {
            UpdateMeshRenderer(armorLeather);
        }
        else if (item.ItemData.Name.Equals("Armor Silver"))
        {
            UpdateMeshRenderer(armorSilver);
        }
        else if (item.ItemData.Name.Equals("Armor Gold"))
        {
            UpdateMeshRenderer(armorGold);
        }
    }


    private void UnEquipArmor(EquipmentType equipmentType)
    {

    }


    public void UpdateMeshRenderer(SkinnedMeshRenderer newMeshRenderer)
    {
        meshRenderer.sharedMesh = newMeshRenderer.sharedMesh;

        Transform[] childrens = transform.GetComponentsInChildren<Transform>(true);

        // sort bones.
        Transform[] bones = new Transform[newMeshRenderer.bones.Length];
        for (int boneOrder = 0; boneOrder < newMeshRenderer.bones.Length; boneOrder++)
        {
            bones[boneOrder] = Array.Find<Transform>(childrens, c => c.name == newMeshRenderer.bones[boneOrder].name);
        }
        meshRenderer.bones = bones;
    }
}

