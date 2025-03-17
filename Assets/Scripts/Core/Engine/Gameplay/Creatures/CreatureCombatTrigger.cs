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
using Managers.Audio;
using static Audio.PlayLevelMusic;

namespace Creatures.Combat
{
    [RequireComponent(typeof(SphereCollider))]
    public class CreatureCombatTrigger : MonoBehaviour
    {
		[SerializeField] private MusicPerTrack _combatMusic = new MusicPerTrack
            { MusicId = "combat", Track = 1 };

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
                AudioManager.Instance.PlaySound(_combatMusic.MusicId, _combatMusic.Track);

                var playerPokemon = GameManager.Characters.Find(x => x.Id == PlayerManager.Instance.Player.Id);
                var partyPokemon = GameManager.Characters.Find(x => x.Id == PartyManager.Instance.ActivePartyMember.Id);
                var wildPokemon = GameManager.Characters.Find(x => x.Id == _creatureCharacter.Id);

                var characterDefinitions = new List<CharacterDefinition>
                {
                    playerPokemon,
                    partyPokemon,
                    wildPokemon
                };

                var characterTeams = new List<CharacterTeam>
                {
                    CharacterTeam.Ally,
                    CharacterTeam.Ally,
                    CharacterTeam.Enemy
                };

                var customLevels = new List<int>
                {
                    -1,
                    -1,
                    _creatureCharacter.CustomLevel
                };

                TurnCombatManager.Instance.CacheInstantiateCharacters(characterDefinitions, characterTeams, customLevels);
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
