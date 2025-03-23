using System;
using System.Collections.Generic;
using Character.Class;
using UnityEngine;
using static Character.Base.CharacterDefinition;

namespace Core.Domain.Character.Moves
{
    [CreateAssetMenu(fileName = nameof(CharacterMove), menuName = "Character/Moves/" + nameof(CharacterMove))]
    public class CharacterMove : ScriptableObject
    {
        public enum StatsCanImprove
        {
            Attack,
            Defense,
            Speed,
            Intelligence
        }

        [Serializable]
        public class StatsImprove
        {
            public StatsCanImprove StatToImprove;
            public int ValueToImprove;
            public GameObject VfxPrefab;
        }

        [Header("Settings")]
        [SerializeField] private string moveName;
        [SerializeField, TextArea] private string moveDescription;
        [SerializeField] private MoveCategory category;
        [SerializeField] private CharacterTypes type;
        [SerializeField] private int power;
        [SerializeField, Range(0, 100)] private int accuracy = 100;
        [SerializeField] private List<StatsImprove> statsToImprove = new();

        [Header("Visuals")]
        [SerializeField, Tooltip("If false spawns on user")] private bool spawnVfxOnTarget = true;
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private float moveAnimationDuration;

        public string MoveName => moveName;
        public string MoveDescription => moveDescription;
        public MoveCategory Category => category;
        public CharacterTypes Type => type;
        public int Power => power;
        public int Accuracy => accuracy;
        public bool SpawnVfxOnTarget => spawnVfxOnTarget;
        public GameObject VfxPrefab => vfxPrefab;
        public float AnimationDuration => moveAnimationDuration;
        public List<StatsImprove> StatsToImprove => statsToImprove;
    }
}