using System.Collections;
using System.Collections.Generic;
using Inventory.Items;
using Managers.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] protected Image _image;
        [SerializeField] protected TMP_Text _stackText;

        [Header("Item")]
        [SerializeField] protected ItemData _itemReference;

        public ItemData ItemReference => _itemReference;

        protected virtual void Awake()
        {
            InventorySystem.OnInventoryChanged += UpdateItemUI;
        }

        protected virtual void OnDestroy()
        {
            InventorySystem.OnInventoryChanged -= UpdateItemUI;
        }

        protected virtual void OnEnable()
        {
            UpdateItemUI();
        }

        protected virtual void UpdateItemUI()
        {
            if (_stackText != null)
                _stackText.text = "";

            if (_image != null)
                _image.sprite = _itemReference.Icon;

            var item = PlayerManager.Instance.PlayerInventory.GetItem(_itemReference);
            if (_itemReference.HideItemIfNotAvailable)
                gameObject.SetActive(item != null);
            else if (_itemReference.GhostItemIfNotAvailable)
            {
                var colorShow = new Color(1f, 1f, 1f);
                var colorHide = new Color(.4f, .4f, .4f);

                _image.color = item != null ? colorShow : colorHide;
            }

            if (_itemReference.ShowStackText && _stackText != null)
                _stackText.text = item != null ? item.StackSize.ToString() : _itemReference.HideStackIfEmpty ? "" : "0";
        }
    }
}
