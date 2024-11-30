using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Managers;
using Managers.Combat;
using Managers.Party;
using Managers.Player;
using Managers.Scenes;
using Character.Class;
using NaughtyAttributes;
using UnityEngine;
using Persistence;

namespace Creatures.Combat
{
    [RequireComponent(typeof(SphereCollider))]
    public class CreatureCombatTrigger : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField, ReadOnly] private CreatureCharacter _creatureCharacter;
        [SerializeField, ReadOnly] private bool _hasTriggered;
        [SerializeField, ReadOnly] private int _sceneId;

        public int SceneId => _sceneId;

        private void Awake()
        {
            _creatureCharacter = GetComponent<CreatureCharacter>();
            _sceneId = GenerateUniqueSceneId();

            if (SaveSystem.Instance.GameData.CombatSceneWinData.CombatSceneIds.Contains(SceneId))
                gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered) return;

            if (other.CompareTag("Player"))
            {
                var playerPokemon = GameManager.Characters.Find(x => x.Id == PlayerManager.Instance.Player.Id);
                var partyPokemon = GameManager.Characters.Find(x => x.Id == PartyManager.Instance.ActivePartyMember.Id);
                var wildPokemon = GameManager.Characters.Find(x => x.Id == _creatureCharacter.Id);

                var dictOfCharacters = new Dictionary<CharacterDefinition, CharacterTeam>
                    {
                        { playerPokemon, CharacterTeam.Ally },
                        { partyPokemon, CharacterTeam.Ally },
                        { wildPokemon, CharacterTeam.Enemy }
                    };

                TurnCombatManager.Instance.CacheInstantiateCharacters(dictOfCharacters);
                TurnCombatManager.Instance.CacheLastSceneInformation(SceneId, other.transform.position);
                ScenesManager.LoadScene("Combat");

                _hasTriggered = true;
            }
        }

        private int GenerateUniqueSceneId()
        {
            string uniqueString = $"{gameObject.name}_{transform.position.x}_{transform.position.y}_{transform.position.z}";
            return uniqueString.GetHashCode();
        }
    }
}
