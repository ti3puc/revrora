using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items
{
    [Serializable]
    public class Item
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private int stackSize;

        public ItemData ItemData
        {
            get => itemData;
            set => itemData = value;
        }

        public int StackSize
        {
            get
            {
                if (ItemData == null)
                    return -1;

                if (stackSize < ItemData.MinimumStackSize)
                    return stackSize = ItemData.MinimumStackSize;

                if (stackSize > ItemData.MaximumStackSize)
                    return stackSize = ItemData.MaximumStackSize;

                return stackSize;
            }
            private set => stackSize = value;
        }

        public Item(ItemData itemData, int initialStackSize = 1)
        {
            ItemData = itemData;
            AddToStack(initialStackSize);
        }

        public void AddToStack(int value = 1)
        {
            StackSize += value;
        }

        public void RemoveFromStack(int value = 1)
        {
            StackSize -= value;
        }
    }
}