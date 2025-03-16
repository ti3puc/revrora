using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.UI;
using Managers.Player;
using UnityEngine;
using UnityEngine.UI;

public class ItemStoreUI : ItemUI
{
    public event Action<ItemStoreUI> OnItemButtonClicked;

    [SerializeField] private bool _hideIfEmpty;
    [SerializeField] private Button _itemButton;

    public Button ItemButton => _itemButton;

    protected override void Awake()
    {
        base.Awake();
        _itemButton.onClick.AddListener(OnItemButtonClickedEvent);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _itemButton.onClick.RemoveListener(OnItemButtonClickedEvent);
    }

    protected override void UpdateItemUI()
    {
        if (_stackText != null)
            _stackText.text = "";

        if (_image != null)
            _image.sprite = _itemReference.Icon;

        var item = PlayerManager.Instance.PlayerInventory.GetItem(_itemReference);
        if (_itemReference.ShowStackText && _stackText != null)
            _stackText.text = item != null ? item.StackSize.ToString() : _itemReference.HideStackIfEmpty ? "" : "0";

        if (_hideIfEmpty && _itemReference.HideItemIfNotAvailable)
            gameObject.SetActive(item != null);
    }

    private void OnItemButtonClickedEvent()
    {
        OnItemButtonClicked?.Invoke(this);
    }
}
