using System.Collections;
using System.Collections.Generic;
using Managers.Player;
using UnityEngine;

namespace Inventory.Items
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _persistentPickup;

        [Header("References")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SphereCollider _sphereCollider;

        [Header("Item")]
        [SerializeField] private ItemData _itemReference;
        [SerializeField] private int _itemQuantity = 1;

        private void Awake()
        {
            _spriteRenderer.sprite = _itemReference.Icon;
        }

        private void Start()
        {
            if (_persistentPickup && PlayerManager.Instance.PlayerInventory.GetItem(_itemReference) != null)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                PlayerManager.Instance.PlayerInventory.AddItem(_itemReference, _itemQuantity);

            Destroy(gameObject);
        }
    }
}
