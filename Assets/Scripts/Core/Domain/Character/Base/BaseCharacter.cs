using UnityEngine;

namespace Character.Base
{
    public class BaseCharacter : MonoBehaviour
    {
        [SerializeField] private BaseCharacterStats _baseCharacterStats;
        private CharacterStats _characterStats;
    }
}