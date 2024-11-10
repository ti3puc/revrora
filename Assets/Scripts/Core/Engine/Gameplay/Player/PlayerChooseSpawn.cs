using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Scenes;
using NaughtyAttributes;
using UnityEngine;

namespace Player.Movement.Spawn
{
    public class PlayerChooseSpawn : MonoBehaviour
    {
        [Serializable]
        public struct ScenePerSpawnPoint
        {
            [Scene] public string LastScene;
            public Transform SpawnPoint;
        }

        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private ScenePerSpawnPoint[] _scenesPerSpawnPoint;

        private void Start()
        {
            foreach (var scenePerSpawnPoint in _scenesPerSpawnPoint)
            {
                if (ScenesManager.LastScene == scenePerSpawnPoint.LastScene)
                {
                    _playerMovement.DisableMovement();

                    transform.position = scenePerSpawnPoint.SpawnPoint.position;
                    transform.rotation = scenePerSpawnPoint.SpawnPoint.rotation;

                    break;
                }
            }

            _playerMovement.EnableMovement();
        }
    }
}