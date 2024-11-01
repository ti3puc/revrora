using System;
using UnityEngine;
using Character.Class;
using NaughtyAttributes;
using Random = System.Random;

namespace Character.Base
{
    [System.Serializable]
    public class CharacterStats
    {
        #region Constants
        private const int MaxLevel = 100;
        private const int BuildPointsPerLevel = 5;
        #endregion

        #region Attributes
        private ICharacterClass _characterClass;
        [SerializeField, ReadOnly] private int _level;
        [SerializeField, ReadOnly] private int _experience;
        [SerializeField, ReadOnly] private int _hp;
        [SerializeField, ReadOnly] private int _mana;

        // Atributos de build, que são os pontos que o jogador pode distribuir
        [SerializeField, ReadOnly] private int _buildPoints;
        [SerializeField, ReadOnly] private int _strengthPoints;
        [SerializeField, ReadOnly] private int _defensePoints;
        [SerializeField, ReadOnly] private int _agilityPoints;
        [SerializeField, ReadOnly] private int _wisdomPoints;
        #endregion

        #region Constructors
        public CharacterStats(ICharacterClass characterClass, int level, bool randomizeBuild = false)
        {
            _characterClass = characterClass;
            _level = level;
            _experience = 0;
            _mana = 0;
            _strengthPoints = 1;
            _defensePoints = 1;
            _agilityPoints = 1;
            _wisdomPoints = 1;
            _buildPoints = randomizeBuild ? RandomizeBuildPoints() : _level * BuildPointsPerLevel;
            _hp = MaxHP;
        }
        #endregion

        // Importante que as propriedades de um personagem nunca estejam diretamente acessíveis;
        // Esta região é responsável por definir os métodos públicos que permitem
        // a manipulação dos atributos do personagem baseado em regras de negócio.
        #region Public Methods
        public void LevelUp()
        {
            if (_level < MaxLevel)
            {
                _level++;
                _buildPoints += BuildPointsPerLevel;
            }
        }
        
        public void ReceiveDamage(int damage)
        {
            _hp -= damage;
            if (_hp <= 0)
            {
                _hp = 0;
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
        private void IncStrengthPoints(int i)
        {
            _strengthPoints += i;
        }

        private void IncDefensePoints(int i)
        {
            _defensePoints += i;
        }

        private void IncAgilityPoints(int i)
        {
            _agilityPoints += i;
        }

        private void IncDexterityPoints(int i)
        {
            _wisdomPoints += i;
        }

        private int CalculateMaxHP()
        {
            return (2 * _characterClass.BaseHP + _strengthPoints / 4) * _level / 100 + _level + 10;
        }

        private int CalculateMaxMana()
        {
            return (2 * _characterClass.BaseMana + _wisdomPoints / 4) * _level / 100 + _level + 10;
        }

        private int CalculateAttack()
        {
            return (2 * _characterClass.BaseStrength + (_strengthPoints * 2 + _agilityPoints) / 4) * _level / 100 + 5;
        }

        private int CalculateDefense()
        {
            return (2 * _characterClass.BaseDefense + (_defensePoints * 2 + _wisdomPoints) / 4) * _level / 100 + 5;
        }

        private int CalculateAgility()
        {
            return (2 * _characterClass.BaseAgility + _agilityPoints * 2 / 4) * _level / 100 + 5;
        }

        private int CalculateWisdom()
        {
            return (2 * _characterClass.BaseWisdom + _wisdomPoints * 2 / 4) * _level / 100 + 5;
        }

        // Este método é responsável por randomizar os pontos de build de um personagem aleatorio ao ser criado
        private int RandomizeBuildPoints()
        {
            Random rnd = new();
            for (int i = 0; i < _level; i++)
            {
                for (int j = 0; j < BuildPointsPerLevel; j++)
                {
                    int random = rnd.Next(0, 4);
                    switch (random)
                    {
                        case 0:
                            IncStrengthPoints(1);
                            break;
                        case 1:
                            IncDefensePoints(1);
                            break;
                        case 2:
                            IncAgilityPoints(1);
                            break;
                        case 3:
                            IncDexterityPoints(1);
                            break;
                    }
                }
            }
            return 0;
        }
        #endregion

        // Esta região é responsável por definir os getters e setters
        #region Getters/Setters
        public int Level => _level;
        public int Experience => _experience;
        public CharacterTypes Type => _characterClass.Type;
        public int HP => _hp;
        public int Mana => _mana;
        public int BuildPoints => _buildPoints;
        public int StrenghtPoints => _strengthPoints;
        public int DefensePoints => _defensePoints;
        public int AgilityPoints => _agilityPoints;
        public int WisdomPoints => _wisdomPoints;
        #region Calculed Stats
        public int MaxHP => CalculateMaxHP();
        public int MaxMana => CalculateMaxMana();
        public int Attack => CalculateAttack();
        public int Defense => CalculateDefense();
        public int Agility => CalculateAgility();
        public int Wisdom => CalculateWisdom();
        #endregion
        #endregion
    }
}