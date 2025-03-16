using System;
using System.Collections.Generic;
using Character.Class;
using Core.Domain.Character.Moves;
using Managers.Audio;
using NaughtyAttributes;
using UnityEngine;

namespace Character.Base
{
    public abstract class BaseCharacter : MonoBehaviour, ICharacterClass
    {
        public delegate void CharacterEvent(BaseCharacter character);
        public static event CharacterEvent OnCharacterDied;
        public static event CharacterEvent OnDamageReceived;

        [Header("References")]
        [SerializeField] private CharacterDefinition _characterDefinition;
        [SerializeField] private CharacterTeam _characterTeam;
        [SerializeField] private GameObject _damageVfx;
        [SerializeField] private string _hitSoundId = "hit";

        [Header("Debug")]
        [SerializeField, ReadOnly] private CharacterStats _characterStats;

        public int Id => _characterDefinition.Id;
        public string Name => _characterDefinition.Name;
        public int BaseHP => _characterDefinition.BaseStats.BaseHP;
        public int BaseStrength => _characterDefinition.BaseStats.BaseStrength;
        public int BaseDefense => _characterDefinition.BaseStats.BaseDefense;
        public int BaseAgility => _characterDefinition.BaseStats.BaseAgility;
        public int BaseWisdom => _characterDefinition.BaseStats.BaseWisdom;
        public CharacterTypes Type => _characterDefinition.BaseStats.Type;
        public CharacterStats CharacterStats => _characterStats;
        public List<CharacterMove> CharacterMoves => _characterDefinition.CharacterMoves;
        public CharacterTeam CharacterTeam => _characterTeam;
        public CharacterDefinition CharacterDefinition
        {
            get => _characterDefinition;
            set => _characterDefinition = value;
        }

        private void Awake()
        {
            Initialize();
        }

        public void Initialize() => Initialize(_characterDefinition);
        public void Initialize(CharacterDefinition newCharacterDefinition)
        {
            _characterDefinition = newCharacterDefinition;

            // clean Visual and instantiate the correct one on Definition
            var visualObj = transform.Find("Visuals");
            if (visualObj == null)
            {
                Debug.LogWarning("Could not found 'Visuals' object on " + name);
                return;
            }

            foreach (Transform child in visualObj)
                Destroy(child.gameObject);

            if (CharacterDefinition != null)
            {
                _characterStats = new CharacterStats(this, 5, true);
                Instantiate(_characterDefinition.Visual, visualObj);
            }
        }

        public void RaiseCharacterDied()
        {
            gameObject.SetActive(false);
            OnCharacterDied?.Invoke(this);
        }

        public void RaiseDamageReceived()
        {
            Instantiate(_damageVfx, transform);
            AudioManager.Instance.PlaySoundOneShot(_hitSoundId, 3);
            OnDamageReceived?.Invoke(this);
        }
    }
}