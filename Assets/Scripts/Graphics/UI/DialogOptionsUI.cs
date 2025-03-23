using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class DialogOptionsUI : MonoBehaviour
{
    [SerializeField] private Button _storeButton;
    [SerializeField] private Button _storageButton;

    private void Awake()
    {
        _storeButton.onClick.AddListener(OnStoreButtonClicked);
        _storageButton.onClick.AddListener(OnStorageButtonClicked);
    }

    private void OnDestroy()
    {
        _storeButton.onClick.RemoveListener(OnStoreButtonClicked);
        _storageButton.onClick.RemoveListener(OnStorageButtonClicked);
    }

    private void OnStoreButtonClicked()
    {
        CanvasManager.Instance.ToggleStore();
    }

    private void OnStorageButtonClicked()
    {
        CanvasManager.Instance.ToggleStorage();
    }
}
