using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        public static event Action OnInventoryChanged;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<Item> _items = new();
        [SerializeField] private ItemData debugItem;

        private Dictionary<ItemData, Item> _itemsDictionary = new();

        private void Awake()
        {
            _items.Clear();
            _itemsDictionary.Clear();
        }

        public Item GetItem(ItemData dataReference)
        {
            if (dataReference == null)
                return null;

            if (_itemsDictionary.TryGetValue(dataReference, out var item))
                return item;

            return null;
        }

        public void AddItem(ItemData dataReference)
        {
            if (dataReference == null) return;

            if (_itemsDictionary.TryGetValue(dataReference, out var item))
                item.AddToStack();
            else
            {
                Item newItem = new Item(dataReference);
                _items.Add(newItem);
                _itemsDictionary.Add(dataReference, newItem);
            }

            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(ItemData dataReference)
        {
            if (dataReference == null) return;

            if (_itemsDictionary.TryGetValue(dataReference, out var item))
            {
                item.RemoveFromStack();

                if (item.StackSize <= 0)
                {
                    _items.Remove(item);
                    _itemsDictionary.Remove(dataReference);
                }
            }

            OnInventoryChanged?.Invoke();
        }

        [Button]
        private void DebugAddItem()
        {
            AddItem(debugItem);
        }

        [Button]
        private void DebugRemoveItem()
        {
            RemoveItem(debugItem);
        }
    }
}