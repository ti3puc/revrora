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
            // player as pokemon
            var playerCharacter = Instantiate(_playerCombatPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation)
                .GetComponent<BaseCharacter>();

            var playerDefinition = TurnCombatManager.Instance.ToInstanceCharacters
                .FirstOrDefault(x => x.Key.Id == PlayerManager.Instance.Player.Id).Key;

            playerCharacter.CharacterDefinition = playerDefinition;
            playerCharacter.Initialize();

            // party pokemon
            var partyCharacter = Instantiate(_partyCombatPrefab, _partySpawnPoint.position, _partySpawnPoint.rotation)
                .GetComponent<BaseCharacter>();

            var partyDefinition = TurnCombatManager.Instance.ToInstanceCharacters
                .FirstOrDefault(x => x.Key.Id != PlayerManager.Instance.Player.Id && x.Value == CharacterTeam.Ally).Key;

            partyCharacter.CharacterDefinition = partyDefinition;
            partyCharacter.Initialize();

            // enemy pokemon
            var enemyCharacter = Instantiate(_enemyCombatPrefab, _enemySpawnPoint.position, _enemySpawnPoint.rotation)
                .GetComponent<BaseCharacter>();

            var enemyDefinition = TurnCombatManager.Instance.ToInstanceCharacters.FirstOrDefault(x => x.Value == CharacterTeam.Enemy).Key;
            enemyCharacter.CharacterDefinition = enemyDefinition;
            enemyCharacter.Initialize();

            _instantiatedCharacters = new List<BaseCharacter> { playerCharacter, partyCharacter, enemyCharacter };
        }
    }
}
