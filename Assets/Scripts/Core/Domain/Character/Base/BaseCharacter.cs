using Character.Class;
using NaughtyAttributes;
using UnityEngine;

namespace Character.Base
{
    public abstract class BaseCharacter : MonoBehaviour, ICharacterClass
    {
        [Header("References")]
        [SerializeField] private BaseCharacterStats _baseCharacterStats;
        [Header("Debug")]
        [SerializeField, ReadOnly] private CharacterStats _characterStats;

        public int Id => _baseCharacterStats.Id;
        public string Name => _baseCharacterStats.Name;
        public int BaseHP => _baseCharacterStats.BaseHP;
        public int BaseMana => _baseCharacterStats.BaseMana;
        public int BaseStrength => _baseCharacterStats.BaseStrength;
        public int BaseDefense => _baseCharacterStats.BaseDefense;
        public int BaseAgility => _baseCharacterStats.BaseAgility;
        public int BaseWisdom => _baseCharacterStats.BaseWisdom;
        public CharacterTypes Type => _baseCharacterStats.Type;
    }
}