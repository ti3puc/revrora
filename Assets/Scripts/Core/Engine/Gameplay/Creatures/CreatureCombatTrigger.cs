using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Managers;
using Managers.Combat;
using Managers.Party;
using Managers.Player;
using Managers.Scenes;
using NaughtyAttributes;
using UnityEngine;

namespace Creatures.Combat
{
    [RequireComponent(typeof(SphereCollider))]
    public class CreatureCombatTrigger : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField, ReadOnly] private CreatureCharacter _creatureCharacter;
        [SerializeField, ReadOnly] private bool _hasInitialized;

        private void Awake()
        {
            _creatureCharacter = GetComponent<CreatureCharacter>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasInitialized) return;

            if (other.CompareTag("Player"))
            {
                var playerPokemon = GameManager.Creatures.Find(x => x.Id == PlayerManager.Instance.Player.Id);
                var partyPokemon = GameManager.Creatures.Find(x => x.Id == _creatureCharacter.Id);
                var wildPokemon = GameManager.Creatures.Find(x => x.Id == PartyManager.Instance.ActivePartyMember.Id);

                var listOfCharacters = new List<BaseCharacter> { playerPokemon, partyPokemon, wildPokemon };
                TurnCombatManager.Instance.CacheInstantiateCharacters(listOfCharacters);
                ScenesManager.LoadScene("Combat");

                _hasInitialized = true;
            }
        }
    }
}
