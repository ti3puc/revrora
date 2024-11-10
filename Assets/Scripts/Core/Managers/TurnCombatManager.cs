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
    public class TurnCombatManager : Singleton<TurnCombatManager>
    {
        [Header("Debug")]
        [SerializeField, ReadOnly] private List<BaseCharacter> _turnCharacters = new List<BaseCharacter>();
        [SerializeField, ReadOnly] private int _turnIndex;
        [SerializeField, ReadOnly] private int _turnCount;
        [SerializeField, ReadOnly] private bool _isTurnEnd;

        public List<BaseCharacter> TurnCharacters => _turnCharacters;
        public bool HasInitialized => _turnCharacters != null && _turnCharacters.Count > 0;
        public bool IsTurnEnd => _isTurnEnd = _turnIndex > _turnCharacters.Count - 1;

        public void InitializeCharacters(List<BaseCharacter> characters)
        {
            _turnCharacters.Clear();
            _turnCharacters.AddRange(characters);

            _turnIndex = 0;
            _turnCount = 0;

            // TODO: logic to organize with stats
            _turnCharacters.Shuffle<BaseCharacter>();

            TurnInputManager.Instance.InitializeCharacters(_turnCharacters);
        }

        public void SetNewTurn()
        {
            _turnIndex = 0;
            _turnCount++;
        }

        public void SetNextCharacter()
        {
            _turnIndex++;
        }

        public BaseCharacter GetCurrentCharacter()
        {
            return _turnCharacters[_turnIndex];
        }

        public List<BaseCharacter> GetEnemies(BaseCharacter character)
        {
            var enemies = _turnCharacters.Where(c => (c.IsTeamPlayer != character.IsTeamPlayer) && (!c.CharacterStats.IsDead())).ToList();
            var list = new List<BaseCharacter>();
            if (enemies.Count <= 0)
                return list;

            enemies.Shuffle<BaseCharacter>();
            list.Add(enemies[0]);
            return list;
        }

        [Button]
        public void DebugInitialize()
        {
            InitializeCharacters(FindObjectsOfType<BaseCharacter>().ToList());
        }
    }
}