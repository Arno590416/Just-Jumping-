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
    private List<ItemParameter> parametersToModify, itemCurrentState;

    public void SetItem(SalvationItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if (leftover != null)
        {
            inventoryData.AddItem(leftover, 1, itemCurrentState);
        }

        this.leftover = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    private void ModifyParameters()//特征量具体变化逻辑
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                float newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter{itemParameter = parameter.itemParameter,value = newValue};
            }
        }
    }
}
