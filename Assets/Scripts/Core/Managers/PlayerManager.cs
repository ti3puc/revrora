using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Player;
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
        [SerializeField, ReadOnly] private Transform _playerTransform;

        public PlayerCharacter Player => _player;
        public Transform PlayerTransform => _player.transform;

        protected override void Awake()
        {
            base.Awake();

            _player ??= FindObjectOfType<PlayerCharacter>();
        }
    }
}