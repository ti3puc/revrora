using System;
using System.Collections;
using System.Collections.Generic;
using Character.Class;
using UnityEngine;

namespace Character.Base
{
    [Serializable]
    public class BaseCharacterStats
    {
        [SerializeField, Range(0, 100)] private int baseHP;
        [SerializeField, Range(0, 100)] private int baseStrength;
        [SerializeField, Range(0, 100)] private int baseDefense;
        [SerializeField, Range(0, 100)] private int baseAgility;
        [SerializeField, Range(0, 100)] private int baseWisdom;
        [SerializeField] private CharacterTypes type;

        public int BaseHP => baseHP;
        public int BaseStrength => baseStrength;
        public int BaseDefense => baseDefense;
        public int BaseAgility => baseAgility;
        public int BaseWisdom => baseWisdom;
        public CharacterTypes Type => type;
    }
}