using System;
using Character.Class;

namespace Character.Base
{
    public class CharacterStats
    {
        #region Constants
            private const int MaxLevel = 100;
            private const int BuildPointsPerLevel = 5;
        #endregion
        
        #region Properties
            private ICharacterClass _characterClass;
            private int _level;
            private int _experience;
            private int _hp;
            private int _mana;
            private int _buildPoints;
            private int _strengthPoints;
            private int _defensePoints;
            private int _agilityPoints;
            private int _dexterityPoints;
        #endregion
        
        #region Constructors
            public CharacterStats(ICharacterClass characterClass, int level, bool randomizeBuild = false)
            {
                _characterClass = characterClass;
                _level = level;
                _experience = 0;
                _hp = 0;
                _mana = 0;
                _strengthPoints = 1;
                _defensePoints = 1;
                _agilityPoints = 1;
                _dexterityPoints = 1;
                _buildPoints = randomizeBuild ? RandomizeBuildPoints() : _level * BuildPointsPerLevel;
            }
        #endregion
        
        #region Public Methods
            public void LevelUp()
            {
                if (_level < MaxLevel)
                {
                    _level++;
                    _buildPoints += BuildPointsPerLevel;
                }
            }
        #endregion
        
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
                _dexterityPoints += i;
            }
            
            private int CalculateMaxHP()
            {
                return _characterClass.BaseHP + _strengthPoints * 10;
            }
            
            private int CalculateMaxMana()
            {
                return _characterClass.BaseMana + _dexterityPoints * 5;
            }
            
            private int CalculateAttack()
            {
                return (_strengthPoints * 2) + _agilityPoints;
            }
            
            private int CalculateDefense()
            {
                return (_defensePoints * 2) + _dexterityPoints;
            }
            
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
            public int DexterityPoints => _dexterityPoints;
                #region Calculed Stats
                    public int MaxHP => CalculateMaxHP();
                    public int MaxMana => CalculateMaxMana();
                    public int Attack => CalculateAttack();
                    public int Defense => CalculateDefense();
                #endregion
        #endregion
    }
}