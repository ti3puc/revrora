using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using Managers;
using NaughtyAttributes;
using Persistence;
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

        public List<Item> Items => _items;
        public int KeysCount => _items.FindAll(item => item.ItemData.Type == ItemType.Key).Count;

        private void Awake()
        {
            _items.Clear();
            _itemsDictionary.Clear();

            // populate inventory from saved data
            var inventoryData = SaveSystem.Instance.GameData.InventoryData;
            var itemsToAdd = new List<(ItemData, int)>();

            foreach (var itemData in inventoryData.Keys)
                itemsToAdd.Add((GetItemData(itemData.Id), itemData.Quantity));

            foreach (var itemData in inventoryData.Currency)
                itemsToAdd.Add((GetItemData(itemData.Id), itemData.Quantity));

            foreach (var itemData in inventoryData.Drops)
                itemsToAdd.Add((GetItemData(itemData.Id), itemData.Quantity));

            foreach (var itemData in inventoryData.Collectibles)
                itemsToAdd.Add((GetItemData(itemData.Id), itemData.Quantity));

            foreach (var (itemData, quantity) in itemsToAdd)
                AddItem(itemData, quantity);
        }

        public ItemData GetItemData(string id)
        {
            return GameManager.Items.Find(item => item.Id == id);
        }

        public Item GetItem(ItemData dataReference)
        {
            if (dataReference == null)
                return null;

            if (_itemsDictionary.TryGetValue(dataReference, out var item))
                return item;

            return null;
        }

        public void AddItem(ItemData dataReference, int value = 1)
        {
            if (dataReference == null) return;

            if (_itemsDictionary.TryGetValue(dataReference, out var item))
                item.AddToStack(value);
            else
            {
                Item newItem = new Item(dataReference, value);
                _items.Add(newItem);
                _itemsDictionary.Add(dataReference, newItem);
            }

            OnInventoryChanged?.Invoke();
            SaveInventory();
        }

        public void RemoveItem(ItemData dataReference, int value = 1)
        {
            if (dataReference == null) return;

            if (_itemsDictionary.TryGetValue(dataReference, out var item))
            {
                item.RemoveFromStack(value);

                if (item.StackSize <= 0)
                {
                    _items.Remove(item);
                    _itemsDictionary.Remove(dataReference);
                }
            }

            OnInventoryChanged?.Invoke();
            SaveInventory();
        }

        private void SaveInventory()
        {
            var inventoryData = SaveSystem.Instance.GameData.InventoryData;

            inventoryData.Keys.Clear();
            inventoryData.Currency.Clear();
            inventoryData.Drops.Clear();
            inventoryData.Collectibles.Clear();

            foreach (var item in _items)
            {
                var itemData = new ItemSavedData
                {
                    Id = item.ItemData.Id,
                    Quantity = item.StackSize
                };

                switch (item.ItemData.Type)
                {
                    case ItemType.Key:
                        inventoryData.Keys.Add(itemData);
                        break;
                    case ItemType.Currency:
                        inventoryData.Currency.Add(itemData);
                        break;
                    case ItemType.MonsterDrops:
                        inventoryData.Drops.Add(itemData);
                        break;
                    case ItemType.Collectible:
                        inventoryData.Collectibles.Add(itemData);
                        break;
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