using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class SalvationItemSO : ItemSO, IItemAction,IDestroyableItem
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();
        public string ActionName => "Commenmorate";
        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            StatAgent weaponSystem = character.GetComponent<StatAgent>();
            if (weaponSystem != null)
            {
                weaponSystem.SetItem(this, itemState == null ? DefaultParametersList : itemState);
                //Debug.Log(itemState != null ? $"itemstate: {string.Join(", ", itemState)}" : "itemstate is null");
                return true;
            }
            return false;
        }
    }
}