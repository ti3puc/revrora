using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory.Items
{
    public enum ItemType { Currency, Key, MonsterDrops, Collectible }

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
        [SerializeField] private bool _hideItemIfNotAvailable;
        [SerializeField, HideIf("_hideItemIfNotAvailable")] private bool _ghostItemIfNotAvailable;
        [SerializeField] private bool _showStackText;
        [SerializeField, ShowIf("_showStackText")] private bool _hideStackIfEmpty;


        public string Id => _id;
        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public ItemType Type => _type;
        public int MinimumStackSize => _minimumStackSize;
        public int MaximumStackSize => _maximumStackSize;
        public bool HideItemIfNotAvailable => _hideItemIfNotAvailable;
        public bool GhostItemIfNotAvailable => _ghostItemIfNotAvailable;
        public bool ShowStackText => _showStackText;
        public bool HideStackIfEmpty => _hideStackIfEmpty;
    }
}
