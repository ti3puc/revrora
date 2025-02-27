using System.Collections;
using System.Collections.Generic;
using Managers.Scenes;
using Persistence;
using UnityEngine;

namespace Environment.Combat
{
    public class ChooseCombatTerrain : MonoBehaviour
    {
        [SerializeField] private GameObject _waterTerrain;
        [SerializeField] private GameObject _earthTerrain;
        [SerializeField] private GameObject _fireTerrain;
        [SerializeField] private GameObject _airTerrain;

        private void Awake()
        {
            _waterTerrain.SetActive(false);
            _earthTerrain.SetActive(false);
            _fireTerrain.SetActive(false);
            _airTerrain.SetActive(false);

            var lastScene = ScenesManager.LastScene;
            if (lastScene.Contains("Air"))
            {
                _airTerrain.SetActive(true);
                return;
            }

            if (lastScene.Contains("Fire"))
            {
                _fireTerrain.SetActive(true);
                return;
            }

            if (lastScene.Contains("Earth"))
            {
                _earthTerrain.SetActive(true);
                return;
            }

            _waterTerrain.SetActive(true);
        }
    }
}
