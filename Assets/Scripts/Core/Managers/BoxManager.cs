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
        [SerializeField] private List<CharacterDefinition> _debugAddToBox;
        [SerializeField, ReadOnly] private List<CharacterDefinition> _boxCharacters;

        public List<CharacterDefinition> BoxCharacters => _boxCharacters;

        protected override void Awake()
        {
            base.Awake();
            SaveSystem.OnSaveLoaded += LoadBoxCharacters;

            // TODO: remove
            DebugAddCharacter();
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
            SaveSystem.Instance.GameData.CreaturesData.Box = _boxCharacters;
        }

        public void RemoveCharacterFromBox(CharacterDefinition character)
        {
            _boxCharacters.Remove(character);
            SaveSystem.Instance.GameData.CreaturesData.Box = _boxCharacters;
        }

        [Button]
        public void DebugAddCharacter()
        {
            foreach (var character in _debugAddToBox)
                AddCharacterToBox(character);
        }

        [Button]
        public void ClearBox()
        {
            _boxCharacters.Clear();
        }
    }
}
