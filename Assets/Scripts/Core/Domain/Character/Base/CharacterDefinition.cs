using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Core.Domain.Character.Moves;
using UnityEngine;

namespace Character.Base
{
    [CreateAssetMenu(fileName = nameof(CharacterDefinition), menuName = "Character/" + nameof(CharacterDefinition))]
    public class CharacterDefinition : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private BaseCharacterStats _baseStats;
        [SerializeField] private List<CharacterMove> _characterMoves = new();
        [SerializeField] private GameObject _visual;

        public int Id => _id;
        public string Name => _name;
        public BaseCharacterStats BaseStats => _baseStats;
        public List<CharacterMove> CharacterMoves => _characterMoves;
        public GameObject Visual => _visual;
    }
}
