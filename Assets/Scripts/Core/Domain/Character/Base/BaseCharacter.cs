using System;
using System.Collections.Generic;
using Character.Class;
using Core.Domain.Character.Moves;
using NaughtyAttributes;
using UnityEngine;

namespace Character.Base
{
    public abstract class BaseCharacter : MonoBehaviour, ICharacterClass
    {
        [Header("References")]
        [SerializeField] private BaseCharacterStats _baseCharacterStats;
        [SerializeField] private List<CharacterMove> _characterMoves;
        [SerializeField] private bool _isTeamPlayer;
        [Header("Debug")]
        [SerializeField, ReadOnly] private CharacterStats _characterStats;

        private void Awake()
        {
            _characterStats = new CharacterStats(this, 5, true);
        }

        public int Id => _baseCharacterStats.Id;
        public string Name => _baseCharacterStats.Name;
        public int BaseHP => _baseCharacterStats.BaseHP;
        public int BaseMana => _baseCharacterStats.BaseMana;
        public int BaseStrength => _baseCharacterStats.BaseStrength;
        public int BaseDefense => _baseCharacterStats.BaseDefense;
        public int BaseAgility => _baseCharacterStats.BaseAgility;
        public int BaseWisdom => _baseCharacterStats.BaseWisdom;
        public CharacterTypes Type => _baseCharacterStats.Type;
        public CharacterStats CharacterStats => _characterStats;
        public List<CharacterMove> CharacterMoves => _characterMoves;
        public bool IsTeamPlayer => _isTeamPlayer;
    }
}