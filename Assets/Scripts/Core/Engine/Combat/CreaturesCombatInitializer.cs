using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Creatures;
using Managers.Combat;
using NaughtyAttributes;
using UnityEngine;

namespace Combat.Creatures
{
    public class CreaturesCombatInitializer : MonoBehaviour
    {
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
            var playerCharacter = TurnCombatManager.Instance.TurnCharacters.Find(x => x.CompareTag("Player"));
            var player = Instantiate(playerCharacter, _playerSpawnPoint.position, _playerSpawnPoint.rotation);

            var partyCharacter = TurnCombatManager.Instance.TurnCharacters.Find(x => x.CompareTag("Player") == false && x.IsTeamPlayer);
            var party = Instantiate(partyCharacter, _partySpawnPoint.position, _partySpawnPoint.rotation);

            var enemyCharacter = TurnCombatManager.Instance.TurnCharacters.Find(x => x.IsTeamPlayer == false);
            var enemy = Instantiate(enemyCharacter, _enemySpawnPoint.position, _enemySpawnPoint.rotation);

            _instantiatedCharacters = new List<BaseCharacter> { player, party, enemy };
        }
    }
}
