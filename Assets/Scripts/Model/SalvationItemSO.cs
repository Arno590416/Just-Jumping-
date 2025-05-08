using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class SalvationItemSO : ItemSO, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();
        public string ActionName => "Commenmorate";
        public AudioClip actionSFX { get; private set; }
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
             StatAgent weaponSystem = character.GetComponent<StatAgent>();
            if (weaponSystem != null)
            {
                weaponSystem.SetItem(this, itemState == null ? DefaultParametersList : itemState);
                return true;
            }
            return false;
        }
    }
}