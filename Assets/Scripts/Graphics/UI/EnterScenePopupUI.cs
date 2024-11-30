using System.Collections;
using System.Collections.Generic;
using Managers;
using Managers.Scenes;
using TMPro;
using UnityEngine;

namespace UI.Utility
{
    public class EnterScenePopupUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _sceneNameText;

        private void Awake()
        {
            var name = GameManager.NamesPerScene.Find(x => x.Scene == ScenesManager.CurrentScene).Name;

            if (string.IsNullOrEmpty(name))
            {
                gameObject.SetActive(false);
                return;
            }

            _sceneNameText.text = name;
        }
    }
}
