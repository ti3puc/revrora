using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using Managers.Player;
using NaughtyAttributes;
using UnityEngine;

namespace Inventory.Items
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _persistentPickup;
        [SerializeField] private bool _use3d;

        [Header("References")]
        [SerializeField, HideIf("_use3d")] private SpriteRenderer _spriteRenderer;
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
            if (!_use3d && _spriteRenderer == null && _itemReference != null)
                _spriteRenderer.sprite = _itemReference.Icon;

            if (_use3d && _itemReference != null)
            {
                if (_spriteRenderer != null)
                    Destroy(_spriteRenderer.gameObject);
                var gameObject = Instantiate(_itemReference.Icon3d, transform);
                gameObject.AddComponent<Rotate>().Speed = 30f;
            }
        }

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (_use3d && _spriteRenderer == null && _itemReference != null)
                _spriteRenderer.sprite = _itemReference.Icon;

            if (_use3d && _itemReference != null)
            {
                if (_spriteRenderer != null)
                    Destroy(_spriteRenderer.gameObject);
                var gameObject = Instantiate(_itemReference.Icon3d, transform);
                gameObject.AddComponent<Rotate>().Speed = 30f;
            }

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
