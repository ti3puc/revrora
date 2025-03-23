using System;
using System.Collections;
using System.Collections.Generic;
using Character.Class;
using NaughtyAttributes;
using UnityEngine;

namespace Character.Base
{
    [Serializable]
    public class BaseCharacterStats
    {
        [SerializeField, Range(0, 100)] private int baseHP;
        [SerializeField, Range(0, 100)] private int baseAttack;
        [SerializeField, Range(0, 100)] private int baseDefense;
        [SerializeField, Range(0, 100)] private int baseSpeed;
        [SerializeField, Range(0, 100)] private int baseIntelligence;
        [SerializeField] private CharacterTypes type;

        public int BaseHP => baseHP;
        public int BaseAttack => baseAttack;
        public int BaseDefense => baseDefense;
        public int BaseSpeed => baseSpeed;
        public int BaseIntelligence => baseIntelligence;
        public CharacterTypes Type => type;
    }
}