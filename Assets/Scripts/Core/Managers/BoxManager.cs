using System.Collections;
using System.Collections.Generic;
using Character.Base;
using NaughtyAttributes;
using Persistence;
using UnityEngine;

namespace Managers.Box
{
    public class BoxManager : Singleton<BoxManager>
    {
        [Header("Debug")]
        [SerializeField] private CharacterDefinition _debugAddToBox;
        [SerializeField, ReadOnly] private List<CharacterDefinition> _boxCharacters;

        protected override void Awake()
        {
            base.Awake();
            SaveSystem.OnSaveLoaded += LoadBoxCharacters;
        }

        private void OnDestroy()
        {
            SaveSystem.OnSaveLoaded -= LoadBoxCharacters;
        }

        private void LoadBoxCharacters()
        {
            _boxCharacters = new List<CharacterDefinition>();
            _boxCharacters = SaveSystem.Instance.GameData.CreaturesData.Box;
        }

        public void AddCharacterToBox(CharacterDefinition character)
        {
            _boxCharacters.Add(character);
        }

        [Button]
        public void DebugAddCharacter()
        {
            AddCharacterToBox(_debugAddToBox);
        }

        [Button]
        public void ClearBox()
        {
            _boxCharacters.Clear();
        }
    }
}
