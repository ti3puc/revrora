using System;
using UnityEngine;
using Character.Class;
using NaughtyAttributes;
using Random = System.Random;
using Managers.Player;
using static Character.Base.CharacterDefinition;

namespace Character.Base
{
    [System.Serializable]
    public class CharacterStats
    {
        #region Fields

        [SerializeField, ReadOnly] private int _hp;
        [SerializeField, ReadOnly] private int _maxHp;
        [SerializeField, ReadOnly] private int _attack;
        [SerializeField, ReadOnly] private int _defense;
        [SerializeField, ReadOnly] private int _speed;
        [SerializeField, ReadOnly] private int _intelligence;
        [SerializeField, ReadOnly] private int _firstRemainingPoints;
        [SerializeField, ReadOnly] private int _secondRemainingPoints;
        [SerializeField, ReadOnly] private int _thirdRemainingPoints;
        [SerializeField, ReadOnly] private int _fourthRemainingPoints;
        [SerializeField, ReadOnly] private int _fifthRemainingPoints;

        private ICharacterClass _characterClass;

        #endregion

        #region Constructors

        public CharacterStats(ICharacterClass characterClass)
        {
            _characterClass = characterClass;
            _hp = MaxHP;
        }

        #endregion


        #region Properties

        public int HP => _hp;
        public int MaxHP
        {
            get
            {
                CalculateAttributes();
                return _maxHp;
            }
        }
        public int Attack
        {
            get
            {
                CalculateAttributes();
                return _attack;
            }
        }
        public int Defense
        {
            get
            {
                CalculateAttributes();
                return _defense;
            }
        }
        public int Speed
        {
            get
            {
                CalculateAttributes();
                return _speed;
            }
        }
        public int Intelligence
        {
            get
            {
                CalculateAttributes();
                return _intelligence;
            }
        }

        public CharacterDefinition CharacterDefinition => _characterClass.CharacterDefinition;
        public int ActualLevel => _characterClass.CharacterTeam == CharacterTeam.Ally
                    ? (PlayerManager.InstanceIsValid ? PlayerManager.Instance.PlayerLevel.Level : 1)
                    : _characterClass.CustomLevel;

        #endregion

        // Importante que as propriedades de um personagem nunca estejam diretamente acessíveis;
        // Esta região é responsável por definir os métodos públicos que permitem
        // a manipulação dos atributos do personagem baseado em regras de negócio.
        #region Public Methods

        public void ReceiveDamage(int damage)
        {
            _hp -= damage;
            _characterClass.RaiseDamageReceived();
            if (_hp <= 0)
            {
                _hp = 0;
                _characterClass.RaiseCharacterDied();
                Debug.Log("Character is dead! (" + _characterClass.Name + ")");
            }
        }

        public bool IsDead()
        {
            return _hp <= 0;
        }

        #endregion

        // Esta região é responsável por definir os métodos privados que permitem
        // a manipulação dos atributos do personagem baseado em regras de negócio,
        // mas que não devem ser acessíveis diretamente.
        #region Private Methods


        private void CalculateAttributes()
        {
            var statsPriorityOrder = CharacterDefinition.StatsPriorityOrder;

            _firstRemainingPoints = CalculateStat(statsPriorityOrder[0], 0);
            _secondRemainingPoints = CalculateStat(statsPriorityOrder[1], _firstRemainingPoints);
            _thirdRemainingPoints = CalculateStat(statsPriorityOrder[2], _secondRemainingPoints);
            _fourthRemainingPoints = CalculateStat(statsPriorityOrder[3], _thirdRemainingPoints);
            _fifthRemainingPoints = CalculateStat(statsPriorityOrder[4], _fourthRemainingPoints);
        }

        private int CalculateStat(StatsOrder stat, int remaining)
        {
            switch (stat)
            {
                case StatsOrder.HP:
                    return CalculateMaxHP(remaining);
                case StatsOrder.Attack:
                    return CalculateAttack(remaining);
                case StatsOrder.Defense:
                    return CalculateDefense(remaining);
                case StatsOrder.Speed:
                    return CalculateAgility(remaining);
                case StatsOrder.Intelligence:
                    return CalculateIntelligence(remaining);
            }

            return 0;
        }

        private int CalculateMaxHP(int remaining)
        {
            var maxHp = CharacterDefinition.HPBaseBuildPoints + (CharacterDefinition.HPLevelBuildPoints * ActualLevel);

            var totalValue = Mathf.FloorToInt(maxHp) + remaining;
            _maxHp = totalValue;
            if (totalValue > 100)
            {
                _maxHp = 100;
                return totalValue - 100;
            }

            return 0;
        }

        private int CalculateAttack(int remaining)
        {
            var attack = CharacterDefinition.StrengthBaseBuildPoints + (CharacterDefinition.StrengthLevelBuildPoints * ActualLevel);

            var totalValue = Mathf.FloorToInt(attack) + remaining;
            _attack = totalValue;
            if (totalValue > 100)
            {
                _attack = 100;
                return totalValue - 100;
            }

            return 0;
        }

        private int CalculateDefense(int remaining)
        {
            var defense = CharacterDefinition.DefenseBaseBuildPoints + (CharacterDefinition.DefenseLevelBuildPoints * ActualLevel);

            var totalValue = Mathf.FloorToInt(defense) + remaining;
            _defense = totalValue;
            if (totalValue > 100)
            {
                _defense = 100;
                return totalValue - 100;
            }

            return 0;
        }

        private int CalculateAgility(int remaining)
        {
            var agility = CharacterDefinition.AgilityBaseBuildPoints + (CharacterDefinition.AgilityLevelBuildPoints * ActualLevel);

            var totalValue = Mathf.FloorToInt(agility) + remaining;
            _speed = totalValue;
            if (totalValue > 100)
            {
                _speed = 100;
                return totalValue - 100;
            }

            return 0;
        }

        private int CalculateIntelligence(int remaining)
        {
            var wisdom = CharacterDefinition.WisdomBaseBuildPoints + (CharacterDefinition.WisdomLevelBuildPoints * ActualLevel);

            var totalValue = Mathf.FloorToInt(wisdom) + remaining;
            _intelligence = totalValue;
            if (totalValue > 100)
            {
                _intelligence = 100;
                return totalValue - 100;
            }

            return 0;
        }

        #endregion
    }
}