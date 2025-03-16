using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Inventory.UI;
using Managers.Combat;
using Managers.Player;
using NaughtyAttributes;
using UnityEngine;

public class ItemCombatUI : ItemUI
{
    [Header("Debug")]
    [SerializeField, ReadOnly] private CombatDropItem _combatItem;

    protected override void Awake()
    {
    }

    protected override void OnDestroy()
    {
    }

    protected override void OnEnable()
    {
        UpdateItemUI();
    }

    private void OnDisable()
    {
        _combatItem = null;
    }

    protected override void UpdateItemUI()
    {
        if (_stackText != null)
            _stackText.text = "";

        if (_image != null)
            _image.sprite = _itemReference.Icon;

        var item = _combatItem;
        gameObject.SetActive(item != null && item.ItemReference != null && item.Quantity > 0);

        if (_itemReference.GhostItemIfNotAvailable)
        {
            var colorShow = new Color(1f, 1f, 1f);
            var colorHide = new Color(.4f, .4f, .4f);

            _image.color = item != null ? colorShow : colorHide;
        }

        if (_itemReference.ShowStackText && _stackText != null)
            _stackText.text = item != null ? item.Quantity.ToString() : _itemReference.HideStackIfEmpty ? "" : "0";
    }

    public void SetupItemToGive(CombatDropItem combatItem)
    {
        _combatItem = combatItem;
        UpdateItemUI();
    }
}
