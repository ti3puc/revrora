using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Base;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using Extensions;
using Combat.Creatures;
using Unity.VisualScripting;
using Character.Class;

namespace Managers.Combat
{
    public class TurnCombatManager : Singleton<TurnCombatManager>
    {
        [Header("Debug")]
        [SerializeField, ReadOnly] private List<BaseCharacter> _turnCharacters = new List<BaseCharacter>();
        [SerializeField, ReadOnly] private int _turnIndex;
        [SerializeField, ReadOnly] private int _turnCount;
        [SerializeField, ReadOnly] private bool _isTurnEnd;
        [SerializeField, ReadOnly] private int _combatCreatureSceneId;
        [SerializeField, ReadOnly] private Vector3 _lastPlayerPosition;
        [SerializeField, ReadOnly] private List<CharacterDefinition> _toInstanceCacheCharacterDefinitions = new();
        [SerializeField, ReadOnly] private List<CharacterTeam> _toInstanceCacheCharacterTeams = new();

        public List<BaseCharacter> TurnCharacters => _turnCharacters;
        public List<CharacterDefinition> ToInstanceCharacterDefinitions => _toInstanceCacheCharacterDefinitions;
        public List<CharacterTeam> ToInstanceCharacterTeams => _toInstanceCacheCharacterTeams;
        public bool HasInitialized => _turnCharacters != null && _turnCharacters.Count > 0;
        public bool IsTurnEnd => _isTurnEnd = _turnIndex > _turnCharacters.Count - 1;
        public int CombatCreatureSceneId => _combatCreatureSceneId;
        public Vector3 LastPlayerPosition => _lastPlayerPosition;

        public void CacheInstantiateCharacters(List<CharacterDefinition> characterDefinitions, List<CharacterTeam> characterTeams)
        {
            _toInstanceCacheCharacterDefinitions.Clear();
            _toInstanceCacheCharacterTeams.Clear();
            _toInstanceCacheCharacterDefinitions.AddRange(characterDefinitions);
            _toInstanceCacheCharacterTeams.AddRange(characterTeams);
        }

        public void CacheLastSceneInformation(int combatCreatureSceneId, Vector3 lastPlayerPosition)
        {
            _combatCreatureSceneId = combatCreatureSceneId;
            _lastPlayerPosition = lastPlayerPosition;
        }


        public void InitializeCharacters(List<BaseCharacter> characters)
        {
            _toInstanceCacheCharacterDefinitions.Clear();
            _toInstanceCacheCharacterTeams.Clear();
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
            var enemies = _turnCharacters.Where(c => (c.CharacterTeam != character.CharacterTeam) && (!c.CharacterStats.IsDead())).ToList();
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