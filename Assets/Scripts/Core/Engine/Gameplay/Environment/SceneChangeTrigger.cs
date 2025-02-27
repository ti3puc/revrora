using System.Collections;
using System.Collections.Generic;
using Managers.Scenes;
using NaughtyAttributes;
using Persistence;
using UnityEngine;

namespace Environment.Scene
{
    public class SceneChangeTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _disableTrigger;
        [SerializeField, HideIf("_disableTrigger"), Scene] private string _sceneToLoad;

        private void OnTriggerEnter(Collider other)
        {
            if (_disableTrigger) return;
            if (other.CompareTag("Player"))
            {
                SaveSystem.Instance.GameData.CombatSceneWinData.CombatSceneIds.Clear();
                SaveSystem.Instance.GameData.CombatSceneWinData.LastPlayerPosition = Vector3.zero;
                ScenesManager.LoadScene(_sceneToLoad);
            }
        }
    }
}
