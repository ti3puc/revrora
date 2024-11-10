using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Base;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using Utility.Extensions;

namespace Managers.Combat
{
    public class TurnInputManager : Singleton<TurnInputManager>
    {
        public delegate void InputEvent(BaseCharacter character);
        public static event InputEvent OnChangedInputCharacter;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<BaseCharacter> _turnInputCharacters = new List<BaseCharacter>();
        [SerializeField, ReadOnly] private int _turnInputIndex;

        public List<BaseCharacter> TurnInputCharacters => _turnInputCharacters;
        public bool HasInitialized => _turnInputCharacters != null && _turnInputCharacters.Count > 0;
        public bool IsTurnInputEnd => _turnInputIndex > _turnInputCharacters.Count - 1;

        public void InitializeCharacters(List<BaseCharacter> characters)
        {
            _turnInputCharacters.Clear();
            _turnInputCharacters.AddRange(characters);

            _turnInputIndex = 0;
        }

        public void SetNewTurn()
        {
            _turnInputIndex = 0;
        }

        public void SetNextCharacter()
        {
            _turnInputIndex++;
        }

        public BaseCharacter GetCurrentCharacter()
        {
            var character = _turnInputCharacters[_turnInputIndex];
            OnChangedInputCharacter?.Invoke(character);
            return character;
        }
    }
}