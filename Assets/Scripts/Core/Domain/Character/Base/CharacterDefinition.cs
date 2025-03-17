using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Core.Domain.Character.Moves;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.Base
{
    [CreateAssetMenu(fileName = nameof(CharacterDefinition), menuName = "Character/" + nameof(CharacterDefinition))]
    public class CharacterDefinition : ScriptableObject
    {
        public enum StatsOrder
        {
            HP,
            Attack,
            Defense,
            Speed,
            Intelligence
        }

        public enum StatsOrderDivision
        {
            Balanced, // 33211
            Specialist, // 42211
            SuperSpecialist // 53110
        }

        [SerializeField] private int _id;
        [Tooltip("If ID is 0 then it's the player character")]
        [SerializeField, ReadOnly] private bool _isPlayer;
        [SerializeField] private string _name;

        [Header("Attributes")]
        [SerializeField] private BaseCharacterStats _baseStats;

        [Header("Leveling")]
        [SerializeField, Range(100, 500)] private int maxBuildPoints = 400;
        [SerializeField, Range(0f, 1f)] private float baseBuildPointsPercentage = .6f;
        [SerializeField, ReadOnly] private float baseBuildPoints;
        [SerializeField, ReadOnly] private float baseBuildPointsPerStat;
        private float hpBaseBuildPoints;
        private float strengthBaseBuildPoints;
        private float defenseBaseBuildPoints;
        private float agilityBaseBuildPoints;
        private float wisdomBaseBuildPoints;
        [SerializeField, ReadOnly] private float totalBuildPointsPerLevelPercentage;
        [SerializeField, ReadOnly] private float totalBuildPointsPerLevel;
        [SerializeField, ReadOnly] private float buildPointsPerLevelPercentagePerStat; // debug only
        [SerializeField, ReadOnly] private float buildPointsPerLevelPerStat; // debug only
        private float hpStatBuildPointsPerLevel;
        private float strengthStatBuildPointsPerLevel;
        private float defenseStatBuildPointsPerLevel;
        private float agilityStatBuildPointsPerLevel;
        private float wisdomStatBuildPointsPerLevel;
        [SerializeField] private StatsOrderDivision statsLevelDivision;
        [SerializeField] private List<StatsOrder> statsPriorityOrder;

        [Header("Moves")]
        [SerializeField] private List<CharacterMove> _characterMoves = new();

        [Header("Items")]
        [SerializeField] private List<CombatDropItem> _dropItems;

        [Header("Visuals")]
        [SerializeField] private GameObject _visual;
        [SerializeField, ShowAssetPreview(128)] private Texture2D _icon;

        public int Id => _id;
        public bool IsPlayer => _isPlayer = _id == 0;
        public string Name => _name;
        public BaseCharacterStats BaseStats => _baseStats;
        public List<CharacterMove> CharacterMoves => _characterMoves;
        public List<CombatDropItem> DropItems => _dropItems;
        public GameObject Visual => _visual;
        public Texture2D Icon => _icon;
        public StatsOrderDivision StatsLevelDivision => statsLevelDivision;
        public List<StatsOrder> StatsPriorityOrder => statsPriorityOrder;

        public float HPBaseBuildPoints => hpBaseBuildPoints;
        public float StrengthBaseBuildPoints => strengthBaseBuildPoints;
        public float DefenseBaseBuildPoints => defenseBaseBuildPoints;
        public float AgilityBaseBuildPoints => agilityBaseBuildPoints;
        public float WisdomBaseBuildPoints => wisdomBaseBuildPoints;

        public float HPLevelBuildPoints => hpStatBuildPointsPerLevel;
        public float StrengthLevelBuildPoints => strengthStatBuildPointsPerLevel;
        public float DefenseLevelBuildPoints => defenseStatBuildPointsPerLevel;
        public float AgilityLevelBuildPoints => agilityStatBuildPointsPerLevel;
        public float WisdomLevelBuildPoints => wisdomStatBuildPointsPerLevel;

        private void OnValidate()
        {
            _isPlayer = _id == 0;

            baseBuildPoints = maxBuildPoints * baseBuildPointsPercentage; // if base points is 20%, each level (max 8) is 10%
            totalBuildPointsPerLevelPercentage = 1f - baseBuildPointsPercentage;
            totalBuildPointsPerLevel = (maxBuildPoints - baseBuildPoints) / 8; // 8 levels
            buildPointsPerLevelPercentagePerStat = totalBuildPointsPerLevelPercentage / 5;
            buildPointsPerLevelPerStat = totalBuildPointsPerLevel / 5;

            baseBuildPointsPerStat = baseBuildPoints / 5; // 5 stats
            hpBaseBuildPoints = _baseStats.BaseHP / 100f * baseBuildPointsPerStat;
            strengthBaseBuildPoints = _baseStats.BaseStrength / 100f * baseBuildPointsPerStat;
            defenseBaseBuildPoints = _baseStats.BaseDefense / 100f * baseBuildPointsPerStat;
            agilityBaseBuildPoints = _baseStats.BaseAgility / 100f * baseBuildPointsPerStat;
            wisdomBaseBuildPoints = _baseStats.BaseWisdom / 100f * baseBuildPointsPerStat;

            if (statsPriorityOrder.Count < 5)
            {
                for (int i = statsPriorityOrder.Count; i < 5; i++)
                    statsPriorityOrder.Add(StatsOrder.HP + i);
            }
            if (statsPriorityOrder.Count > 5)
                statsPriorityOrder.RemoveRange(5, statsPriorityOrder.Count - 5);

            switch (statsLevelDivision)
            {
                case StatsOrderDivision.Balanced:
                    SetStat(statsPriorityOrder[0], totalBuildPointsPerLevel * 0.3f);
                    SetStat(statsPriorityOrder[1], totalBuildPointsPerLevel * 0.3f);
                    SetStat(statsPriorityOrder[2], totalBuildPointsPerLevel * 0.2f);
                    SetStat(statsPriorityOrder[3], totalBuildPointsPerLevel * 0.1f);
                    SetStat(statsPriorityOrder[4], totalBuildPointsPerLevel * 0.1f);
                    break;
                case StatsOrderDivision.Specialist:
                    SetStat(statsPriorityOrder[0], totalBuildPointsPerLevel * 0.4f);
                    SetStat(statsPriorityOrder[1], totalBuildPointsPerLevel * 0.2f);
                    SetStat(statsPriorityOrder[2], totalBuildPointsPerLevel * 0.2f);
                    SetStat(statsPriorityOrder[3], totalBuildPointsPerLevel * 0.1f);
                    SetStat(statsPriorityOrder[4], totalBuildPointsPerLevel * 0.1f);
                    break;
                case StatsOrderDivision.SuperSpecialist:
                    SetStat(statsPriorityOrder[0], totalBuildPointsPerLevel * 0.5f);
                    SetStat(statsPriorityOrder[1], totalBuildPointsPerLevel * 0.3f);
                    SetStat(statsPriorityOrder[2], totalBuildPointsPerLevel * 0.1f);
                    SetStat(statsPriorityOrder[3], totalBuildPointsPerLevel * 0.1f);
                    SetStat(statsPriorityOrder[4], 0);
                    break;
            }
        }

        private void SetStat(StatsOrder stat, float value)
        {
            switch (stat)
            {
                case StatsOrder.HP:
                    hpStatBuildPointsPerLevel = value;
                    break;
                case StatsOrder.Attack:
                    strengthStatBuildPointsPerLevel = value;
                    break;
                case StatsOrder.Defense:
                    defenseStatBuildPointsPerLevel = value;
                    break;
                case StatsOrder.Speed:
                    agilityStatBuildPointsPerLevel = value;
                    break;
                case StatsOrder.Intelligence:
                    wisdomStatBuildPointsPerLevel = value;
                    break;
            }
        }
    }
}
