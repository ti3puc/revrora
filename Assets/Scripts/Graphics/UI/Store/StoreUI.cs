using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers.Player;
using NaughtyAttributes;
using TMPro;
using UI.Menu.Settings;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _title;
    [SerializeField] private SliderSettingsButton _quantitySlider;
    [SerializeField] private Button _sellButton;
    [SerializeField] private TMP_Text _sellButtonText;

    [Header("Debug")]
    [SerializeField, ReadOnly] private List<ItemStoreUI> _itemStoreUIs;
    [SerializeField, ReadOnly] private ItemStoreUI _currentSelectedItem;
    [SerializeField, ReadOnly] private int _currentQuantity = 1;

    private void Awake()
    {
        _sellButton.onClick.AddListener(SellItems);

        _itemStoreUIs = GetComponentsInChildren<ItemStoreUI>().ToList();
        foreach (var itemStoreUI in _itemStoreUIs)
            itemStoreUI.OnItemButtonClicked += OnItemButtonClicked;
    }

    private void OnDestroy()
    {
        _sellButton.onClick.AddListener(SellItems);
        foreach (var itemStoreUI in _itemStoreUIs)
            itemStoreUI.OnItemButtonClicked -= OnItemButtonClicked;
    }

    private void OnEnable()
    {
        UpdateButtonsVisibility(false);
        UpdateStoreUI();
    }

    public void ChangeQuantity(int newValue)
    {
        _currentQuantity = newValue;
        var item = PlayerManager.Instance.PlayerInventory.GetItem(_currentSelectedItem.ItemReference);
        _sellButtonText.text = $"Sell for {item.ItemData.SellXp * _currentQuantity} XP";
    }

    private void UpdateStoreUI()
    {
        bool hasItem = _currentSelectedItem != null;
        _title.text = hasItem ? "Set quantity and sell" : "Select item to sell";

        if (_currentSelectedItem != null)
        {
            var item = PlayerManager.Instance.PlayerInventory.GetItem(_currentSelectedItem.ItemReference);

            _currentQuantity = item.StackSize;
            _currentSelectedItem.ItemButton.interactable = false;

            _quantitySlider.MinMaxRange = new Vector2Int(1, item.StackSize);
            _quantitySlider.CurrentValue = _currentQuantity;
            _sellButtonText.text = $"Sell for {item.ItemData.SellXp * _currentQuantity} XP";
        }

        _sellButton.interactable = !PlayerManager.Instance.PlayerLevel.IsLevelLocked;
    }

    private void UpdateButtonsVisibility(bool visibility)
    {
        _quantitySlider.gameObject.SetActive(visibility);
        _sellButton.gameObject.SetActive(visibility);
    }

    private void OnItemButtonClicked(ItemStoreUI itemStoreUI)
    {
        if (_currentSelectedItem == itemStoreUI)
        {
            DeselectItem();
            return;
        }

        var item = PlayerManager.Instance.PlayerInventory.GetItem(itemStoreUI.ItemReference);
        if (item == null || (item != null && item.StackSize < 1))
        {
            DeselectItem();
            return;
        }

        if (_currentSelectedItem != null)
            _currentSelectedItem.ItemButton.interactable = true;

        _currentSelectedItem = itemStoreUI;

        UpdateStoreUI();
        UpdateButtonsVisibility(true);
    }

    private void SellItems()
    {
        if (_currentSelectedItem == null) return;
        if (_currentQuantity < 1) return;

        var item = PlayerManager.Instance.PlayerInventory.GetItem(_currentSelectedItem.ItemReference);
        if (item == null) return;

        PlayerManager.Instance.PlayerLevel.AddExperience(item.ItemData.SellXp * _currentQuantity);
        PlayerManager.Instance.PlayerInventory.RemoveItem(item.ItemData, _currentQuantity);

        if (item == null || (item != null && item.StackSize < 1))
        {
            DeselectItem();
            return;
        }

        UpdateStoreUI();
    }

    private void DeselectItem()
    {
        if (_currentSelectedItem)
        {
            _currentSelectedItem.ItemButton.interactable = true;
            _currentSelectedItem = null;
        }

        UpdateButtonsVisibility(false);
    }
}
