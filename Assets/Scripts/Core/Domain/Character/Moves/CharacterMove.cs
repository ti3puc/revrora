using Character.Class;
using UnityEngine;

namespace Core.Domain.Character.Moves
{
    [CreateAssetMenu(fileName = nameof(CharacterMove), menuName = "Character/Moves/" + nameof(CharacterMove))]
    public class CharacterMove : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private string moveName;
        [SerializeField] private MoveCategory category;
        [SerializeField] private CharacterTypes type;
        [SerializeField] private int power;
        [SerializeField] private int accuracy;

        [Header("Visuals")]
        [SerializeField] private GameObject visualPrefab;
        [SerializeField] private float animationDuration;

        public string MoveName => moveName;
        public MoveCategory Category => category;
        public CharacterTypes Type => type;
        public int Power => power;
        public int Accuracy => accuracy;
        public GameObject VisualPrefab => visualPrefab;
        public float AnimationDuration => animationDuration;
    }
}