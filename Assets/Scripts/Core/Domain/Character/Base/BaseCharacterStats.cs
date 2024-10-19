using System.Collections;
using System.Collections.Generic;
using Character.Class;
using UnityEngine;

namespace Character.Base
{
    [CreateAssetMenu(fileName = nameof(BaseCharacterStats), menuName = "Character/" + nameof(BaseCharacterStats))]
    public class BaseCharacterStats : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string charName;
        [SerializeField] private int baseHP;
        [SerializeField] private int baseMana;
        [SerializeField] private int baseStrength;
        [SerializeField] private int baseDefense;
        [SerializeField] private int baseAgility;
        [SerializeField] private int baseWisdom;
        [SerializeField] private CharacterTypes type;

        public int Id => id;
        public string Name => charName;
        public int BaseHP => baseHP;
        public int BaseMana => baseMana;
        public int BaseStrength => baseStrength;
        public int BaseDefense => baseDefense;
        public int BaseAgility => baseAgility;
        public int BaseWisdom => baseWisdom;
        public CharacterTypes Type => type;
    }
}