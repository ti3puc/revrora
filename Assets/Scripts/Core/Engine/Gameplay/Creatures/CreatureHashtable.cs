using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using UnityEngine;

namespace Creatures
{
    [CreateAssetMenu(fileName = nameof(CreatureHashtable), menuName = "Character/" + nameof(CreatureHashtable))]
    public class CreatureHashtable : ScriptableObject
    {
        [SerializeField] private List<BaseCharacter> _creatures = new List<BaseCharacter>();

        public List<BaseCharacter> Creatures => _creatures;
    }
}
