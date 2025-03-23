using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Base;
using Character.Class;
using Creatures;
using Managers.Combat;
using Managers.Player;
using NaughtyAttributes;
using Player;
using UnityEngine;

namespace Combat.Creatures
{
    public class CreaturesCombatInitializer : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerCombatPrefab;
        [SerializeField] private GameObject _partyCombatPrefab;
        [SerializeField] private GameObject _enemyCombatPrefab;

        [Header("Transform")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _partySpawnPoint;
        [SerializeField] private Transform _enemySpawnPoint;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<BaseCharacter> _instantiatedCharacters;

        private void Start()
        {
            InstantiateCharacters();
            TurnCombatManager.Instance.InitializeCharacters(_instantiatedCharacters);
        }

        public void InstantiateCharacters()
        {
            var characterDefinitions = TurnCombatManager.Instance.ToInstanceCharacterDefinitions;
            var characterTeams = TurnCombatManager.Instance.ToInstanceCharacterTeams;
            var customLevels = TurnCombatManager.Instance.ToInstanceCustomLevels;

            // player as pokemon
            var playerCharacter = Instantiate(_playerCombatPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation)
                .GetComponent<BaseCharacter>();

            var playerDefinition = characterDefinitions
                .FirstOrDefault(x => x.Id == PlayerManager.Instance.Player.Id);

            playerCharacter.CharacterDefinition = playerDefinition;
            playerCharacter.Initialize();

            // party pokemon
            var partyCharacter = Instantiate(_partyCombatPrefab, _partySpawnPoint.position, _partySpawnPoint.rotation)
                .GetComponent<BaseCharacter>();

            var partyDefinition = characterDefinitions
                .FirstOrDefault(x => x.Id != PlayerManager.Instance.Player.Id && characterTeams[characterDefinitions.IndexOf(x)] == CharacterTeam.Ally);

            partyCharacter.CharacterDefinition = partyDefinition;
            partyCharacter.Initialize();

            // enemy pokemon
            var enemyCharacter = Instantiate(_enemyCombatPrefab, _enemySpawnPoint.position, _enemySpawnPoint.rotation)
                .GetComponent<BaseCharacter>();

            var enemyIndex = characterTeams.IndexOf(CharacterTeam.Enemy);
            var enemyDefinition = characterDefinitions[enemyIndex];

            enemyCharacter.CustomLevel = customLevels[enemyIndex];
            enemyCharacter.CharacterDefinition = enemyDefinition;
            enemyCharacter.Initialize();

            _instantiatedCharacters = new List<BaseCharacter> { playerCharacter, partyCharacter, enemyCharacter };
        }
    }
}
