using Character.Class;
using UnityEngine;

namespace Core.Domain.Character.Moves
{
    [CreateAssetMenu(fileName = nameof(CharacterMove), menuName = "Character/Moves/" + nameof(CharacterMove))]
    public class CharacterMove : ScriptableObject
    {
        [SerializeField] private string moveName;
        [SerializeField] private MoveCategory category;
        [SerializeField] private CharacterTypes type;
        [SerializeField] private int power;
        [SerializeField] private int manaCost;
        [SerializeField] private int rate;
        [SerializeField] private string script;

        public string MoveName => moveName;
        public MoveCategory Category => category;
        public CharacterTypes Type => type;
        public int Power => power;
        public int ManaCost => manaCost;
        public int Rate => rate;
        public string Script => script;
    }
}