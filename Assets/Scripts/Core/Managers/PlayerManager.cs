using System.Collections;
using System.Collections.Generic;
using Inventory;
using NaughtyAttributes;
using Player;
using Player.Movement;
using UnityEngine;

namespace Managers.Player
{
    /// <summary>
    /// Helper class used as single point to access player components anywhere.
    /// </summary>
    public class PlayerManager : Singleton<PlayerManager>
    {
        [Header("Debug")]
        [SerializeField, ReadOnly] private PlayerCharacter _player;
        [SerializeField, ReadOnly] private PlayerMovement _playerMovement;
        [SerializeField, ReadOnly] private Transform _playerTransform;
        [SerializeField, ReadOnly] private InventorySystem _playerInventory;

        public PlayerCharacter Player => _player ??= FindObjectOfType<PlayerCharacter>();
        public PlayerMovement PlayerMovement => _playerMovement ??= FindObjectOfType<PlayerMovement>();
        public Transform PlayerTransform => Player.transform;
        public InventorySystem PlayerInventory => _playerInventory ??= FindObjectOfType<InventorySystem>();

        protected override void Awake()
        {
            base.Awake();
        }
    }
}