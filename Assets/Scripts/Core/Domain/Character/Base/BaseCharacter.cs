using System;
using System.Collections;
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
        public static event CharacterEvent OnHealReceived;
        public static event CharacterEvent OnDamageMissed;
        public static event CharacterEvent OnImprovedStat;

        [Header("Level")]
        [SerializeField] private int _customLevel = 1;

        [Header("References")]
        [SerializeField, OnValueChanged("InstantiateVisual")] private CharacterDefinition _characterDefinition;
        [SerializeField] private CharacterTeam _characterTeam;

        [Header("Animation")]
        [SerializeField] private float _rotationSpeed = 180;

        [Header("VFX and Audio")]
        // damage vfx gets from move
        [SerializeField] private string _hitSoundId = "hit";
        [SerializeField] private GameObject _missVfx;
        [SerializeField] private string _missSoundId = "miss";
        [SerializeField] private string _healSoundId = "heal";
        [SerializeField] private GameObject _deathVfx;
        [SerializeField] private string _deadSoundId = "dead";
        [SerializeField] private string _improvedSoundId = "improved";

        [Header("Debug")]
        [SerializeField, ReadOnly] private CharacterStats _characterStats = null;

        public int Id => _characterDefinition.Id;
        public string Name => _characterDefinition.Name;
        public int HP => _characterStats.HP;
        public int MaxHP => _characterStats.MaxHP;
        public int Attack => _characterStats.Attack;
        public int Defense => _characterStats.Defense;
        public int Speed => _characterStats.Speed;
        public int Intelligence => _characterStats.Intelligence;
        public CharacterTypes Type => _characterDefinition.BaseStats.Type;
        public CharacterStats CharacterStats => _characterStats;
        public List<CharacterMove> CharacterMoves => _characterDefinition.CharacterMoves;
        public CharacterTeam CharacterTeam => _characterTeam;
        public int CustomLevel
        {
            get => _customLevel;
            set => _customLevel = value;
        }
        public CharacterDefinition CharacterDefinition
        {
            get => _characterDefinition;
            set => _characterDefinition = value;
        }
        public bool IsInitialized => _characterDefinition != null && _characterStats != null;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize() => Initialize(_characterDefinition);
        public void Initialize(CharacterDefinition newCharacterDefinition)
        {
            if (newCharacterDefinition == null)
                return;

            _characterDefinition = newCharacterDefinition;
            _characterDefinition.Setup();
            InstantiateVisual();
        }

        public void RaiseCharacterDied()
        {
            if (_deathVfx != null)
                Instantiate(_deathVfx, transform);
            AudioManager.Instance.PlaySoundOneShot(_deadSoundId, 3);

            // TODO: die animation
            // if player dies does not disable object, only enter in a dead state
            if (CharacterDefinition.IsPlayer == false)
                gameObject.SetActive(false);

            OnCharacterDied?.Invoke(this);
        }

        public void RaiseDamageReceived(GameObject vfx)
        {
            if (vfx != null)
                Instantiate(vfx, transform);
            AudioManager.Instance.PlaySoundOneShot(_hitSoundId, 3);
            OnDamageReceived?.Invoke(this);
        }

        public void RaiseHealReceived(GameObject vfx)
        {
            if (vfx != null)
                Instantiate(vfx, transform);
            AudioManager.Instance.PlaySoundOneShot(_healSoundId, 3);
            OnHealReceived?.Invoke(this);
        }

        public void RaiseDamageMissed()
        {
            if (_missVfx != null)
                Instantiate(_missVfx, transform);
            AudioManager.Instance.PlaySoundOneShot(_missSoundId, 3);
            OnDamageMissed?.Invoke(this);
        }

        public void RaiseImprovedStat(GameObject vfx)
        {
            if (vfx != null)
                Instantiate(vfx, transform);
            AudioManager.Instance.PlaySoundOneShot(_improvedSoundId, 3);
            OnImprovedStat?.Invoke(this);
        }

        public void SpawnVfx(GameObject vfx)
        {
            if (vfx != null)
                Instantiate(vfx, transform);
        }

        public void RotateTo(Quaternion rotation) => StartCoroutine(SmoothRotateTo(rotation));
        public void RotateTo(Transform target) => StartCoroutine(SmoothRotateTo(target));

        private IEnumerator SmoothRotateTo(Transform target)
        {
            var direction = target.position - transform.position;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // Use X and Z for Y-axis rotation
            var rotation = Quaternion.Euler(0, angle, 0); // Only modify the Y-axis

            while (Quaternion.Angle(transform.rotation, rotation) > 0.01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator SmoothRotateTo(Quaternion target)
        {
            var targetEuler = target.eulerAngles;
            var targetRotation = Quaternion.Euler(0, targetEuler.y, 0); // Only modify the Y-axis

            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }

        [Button]
        private void InstantiateVisual()
        {
            // clean Visual and instantiate the correct one on Definition
            var visualObj = transform.Find("Visuals");
            if (visualObj == null)
            {
                Debug.LogWarning("Could not found 'Visuals' object on " + name);
                return;
            }

            foreach (Transform child in visualObj)
            {
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }

            if (CharacterDefinition != null)
            {
                _characterStats = new CharacterStats(this);
                Instantiate(_characterDefinition.Visual, visualObj);
            }
        }
    }
}