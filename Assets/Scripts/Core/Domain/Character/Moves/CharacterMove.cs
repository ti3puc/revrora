using Character.Class;
using UnityEngine;

namespace Core.Domain.Character.Moves
{
    [CreateAssetMenu(fileName = nameof(CharacterMove), menuName = "Character/Moves/" + nameof(CharacterMove))]
    public class CharacterMove : ScriptableObject
    {
        public string name;
        
        public MoveCategory category;
        public CharacterTypes type;
        
        public int power;
        public int manaCost;
        public int rate;

        public string script;
    }
}