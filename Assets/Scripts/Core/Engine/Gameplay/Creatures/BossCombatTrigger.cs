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
using Environment.Interaction;
using Inventory.Items;

namespace Creatures.Combat
{
    public class BossCombatTrigger : Interactable
    {
        [Header("Item Drop")]
        [SerializeField] private ItemData _itemDrop;
        [SerializeField] private ItemPickup _itemPickupPrefab;
        [SerializeField] private Vector3 _itemSpawnOffset = new Vector3(0, 0, -2);

        [Header("Debug")]
        [SerializeField, ReadOnly] private CreatureCharacter _creatureCharacter;
        [SerializeField, ReadOnly] private bool _hasTriggered;
        [SerializeField, ReadOnly] private int _sceneId;

        public int SceneId => _sceneId;

        protected override void Awake()
        {
            base.Awake();

            _creatureCharacter = GetComponent<CreatureCharacter>();
            _sceneId = GenerateUniqueSceneId();

            if (SaveSystem.Instance.GameData.CombatSceneWinData.CombatSceneIds.Contains(SceneId))
            {
                var itemPickup = Instantiate(_itemPickupPrefab, transform.position + _itemSpawnOffset, Quaternion.identity);
                itemPickup.PersistentPickup = true;
                itemPickup.ItemData = _itemDrop;
                itemPickup.Initialize();

                gameObject.SetActive(false);
            }
        }

        private int GenerateUniqueSceneId()
        {
            string uniqueString = $"{gameObject.name}_{transform.position.x}_{transform.position.y}_{transform.position.z}";
            return uniqueString.GetHashCode();
        }

        public override void ReceiveInteraction()
        {
            if (_hasTriggered) return;

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
            TurnCombatManager.Instance.CacheLastSceneInformation(SceneId, PlayerManager.Instance.PlayerTransform.position);
            ScenesManager.LoadScene("Combat");

            _hasTriggered = true;
        }
    }
}
