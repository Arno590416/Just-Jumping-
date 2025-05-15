using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.Model.ItemSO;

public class StatAgent : MonoBehaviour
{
    [SerializeField]
    private SalvationItemSO leftover;

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    public List<ItemParameter> parametersToModify, itemCurrentState;

    public void SetItem(SalvationItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if (leftover != null)//用于检查是否已装备物品
        {
            inventoryData.AddItem(leftover, 1, itemCurrentState);//卸下物品
        }

        this.leftover = weaponItemSO;//装备物品weapon至leftover
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    private void ModifyParameters() // 特征量具体变化逻辑
    {
        foreach (var parameter in parametersToModify) // 遍历需要修改的参数列表
        {
            if (itemCurrentState.Contains(parameter)) // 检查当前状态中是否包含该参数
            {
                int index = itemCurrentState.IndexOf(parameter); // 找到该参数在当前状态中的索引
                float newValue =itemCurrentState[index].value + parameter.value; // 计算新值
                itemCurrentState[index] = new ItemParameter{itemParameter = parameter.itemParameter,value = newValue};
            }
        }
    }
}
