using System;
using System.Collections;
using System.Collections.Generic;
using Character.Class;
using Inventory.Items;
using UnityEngine;

namespace Character.Base
{
    [Serializable]
    public class CombatDropItem
    {
        [SerializeField] private ItemData _itemReference;
        [SerializeField] private int _quantity;

        public ItemData ItemReference => _itemReference;
        public int Quantity => _quantity;
    }
}