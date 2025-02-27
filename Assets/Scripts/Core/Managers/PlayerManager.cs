using System.Collections;
using System.Collections.Generic;
using Inventory;
using NaughtyAttributes;
using Persistence;
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
        [SerializeField, ReadOnly] private PlayerLevel _playerLevel;

        public PlayerCharacter Player => _player ??= FindObjectOfType<PlayerCharacter>();
        public PlayerMovement PlayerMovement => _playerMovement ??= FindObjectOfType<PlayerMovement>();
        public Transform PlayerTransform => Player.transform;
        public InventorySystem PlayerInventory => _playerInventory ??= FindObjectOfType<InventorySystem>();
        public PlayerLevel PlayerLevel => _playerLevel ??= FindObjectOfType<PlayerLevel>();

        protected override void Awake()
        {
            base.Awake();

            if (SaveSystem.Instance.GameData.CombatSceneWinData.LastPlayerPosition != null &&
                SaveSystem.Instance.GameData.CombatSceneWinData.LastPlayerPosition != Vector3.zero &&
                PlayerMovement != null)
            {
                PlayerMovement.DisableMovement();
                PlayerTransform.position = SaveSystem.Instance.GameData.CombatSceneWinData.LastPlayerPosition;
                PlayerMovement.EnableMovement();
            }
        }
    }
}