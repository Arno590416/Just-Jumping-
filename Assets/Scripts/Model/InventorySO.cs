using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Inventory.Model.ItemSO;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems;

        [field: SerializeField]

        public int Size { get; private set; } = 5;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
        
        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if (item.IsStackable == false)
            {
                while (quantity > 0 && IsInventoryFull() == false) 
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                }
                InformAboutChange();
                return quantity;
            }
            quantity = AddStackableItem(item, quantity);//quantitiy等于0（全部成功添加），不成功则返回剩余数量
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new InventoryItem{ item = item, quantity = quantity,itemState =new List<ItemParameter>(itemState == null ? item.DefaultParametersList : itemState)};
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;//没有空物体存在

        private int AddStackableItem(ItemSO item, int quantity)
        {
            // 遍历当前库存，尝试将物品堆叠到已有的堆叠槽中
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) // 跳过空槽
                    continue;

                if (inventoryItems[i].item.ID == item.ID) // 判断是否是同一种物品
                {
                    // 计算当前槽位还能堆叠的数量
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity > amountPossibleToTake) // 如果剩余数量超过可堆叠数量
                    {
                        // 填满当前槽位
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity -= amountPossibleToTake; // 剩余数量变化
                    }
                    else // 如果剩余数量可以完全堆叠到当前槽位
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange(); // 通知库存更新
                        return 0; // 所有物品已成功添加，返回 0
                    }
                }
            }

            // 如果还有剩余物品，尝试将它们放入空槽中
            while (quantity > 0 && IsInventoryFull() == false)
            {
                // 计算当前可以放入的数量（不能超过物品的最大堆叠数）
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity; // 剩余数量变化
                AddItemToFirstFreeSlot(item, newQuantity); // 添加到第一个空槽
            }

            return quantity; // 返回未能添加的剩余数量
        }
        public void RemoveItem(int itemIndex, int amount) //删除物品逻辑
        {
            if (inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty)
                    return;
                int reminder = inventoryItems[itemIndex].quantity - amount;
                if (reminder <= 0)
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                else
                    inventoryItems[itemIndex] = inventoryItems[itemIndex]
                        .ChangeQuantity(reminder);

                InformAboutChange();
            }
        }
        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue =
                new Dictionary<int, InventoryItem>();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)//从物品列表传回对应位置物品
        {
            return inventoryItems[itemIndex];
        }
     public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }
    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem{item = null,quantity = 0,itemState = new List<ItemParameter>()};
    }
}