using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory
{
    public class InventorySystem : MonoBehaviour
    {
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
            if (_itemsDictionary.TryGetValue(dataReference, out var item))
                return item;

            return null;
        }

        public void AddItem(ItemData dataReference)
        {
            if (_itemsDictionary.TryGetValue(dataReference, out var item))
                item.AddToStack();
            else
            {
                Item newItem = new Item(dataReference);
                _items.Add(newItem);
                _itemsDictionary.Add(dataReference, newItem);
            }
        }

        public void RemoveItem(ItemData dataReference)
        {
            if (_itemsDictionary.TryGetValue(dataReference, out var item))
            {
                item.RemoveFromStack();

                if (item.StackSize <= 0)
                {
                    _items.Remove(item);
                    _itemsDictionary.Remove(dataReference);
                }
            }
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