using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items
{
    public enum ItemType { Currency, Key, MonsterDrops, Quest }

    [CreateAssetMenu(fileName = nameof(ItemData), menuName = "Inventory/" + nameof(ItemData))]
    public class ItemData : ScriptableObject
    {
        [Header("Internal")]
        [SerializeField] private string _id;
        [SerializeField] private ItemType _type;

        [Header("Stack")]
        [SerializeField] private int _minimumStackSize = 0;
        [SerializeField] private int _maximumStackSize = 99;

        [Header("UI")]
        [SerializeField] private string _displayName;
        [SerializeField] private Sprite _icon;


        public string Id => _id;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public ItemType Type => _type;
        public int MinimumStackSize => _minimumStackSize;
        public int MaximumStackSize => _maximumStackSize;
    }
}
