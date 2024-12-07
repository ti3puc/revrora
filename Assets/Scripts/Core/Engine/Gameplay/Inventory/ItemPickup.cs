using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
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
        [SerializeField] private string _collectSoundId = "collect";

        [Header("Item")]
        [SerializeField] private ItemData _itemReference;
        [SerializeField] private int _itemQuantity = 1;

        public bool PersistentPickup
        {
            get => _persistentPickup;
            set => _persistentPickup = value;
        }
        public ItemData ItemData
        {
            get => _itemReference;
            set => _itemReference = value;
        }

        private void Awake()
        {
            if (_spriteRenderer == null && _itemReference != null)
                _spriteRenderer.sprite = _itemReference.Icon;
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _spriteRenderer.sprite = _itemReference.Icon;

            if (_persistentPickup && PlayerManager.Instance.PlayerInventory.GetItem(_itemReference) != null)
                Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySoundOneShot(_collectSoundId, 3);
                PlayerManager.Instance.PlayerInventory.AddItem(_itemReference, _itemQuantity);
                Destroy(gameObject);
            }

        }
    }
}
