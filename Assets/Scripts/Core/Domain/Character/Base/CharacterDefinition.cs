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
        [SerializeField] private List<CombatDropItem> _dropItems;
        [SerializeField] private GameObject _visual;
        [SerializeField] private Texture2D _icon;
        [SerializeField] private Sprite _graph;

        public int Id => _id;
        public string Name => _name;
        public BaseCharacterStats BaseStats => _baseStats;
        public List<CharacterMove> CharacterMoves => _characterMoves;
        public List<CombatDropItem> DropItems => _dropItems;
        public GameObject Visual => _visual;
        public Texture2D Icon => _icon;
        public Sprite Graph => _graph;
    }
}
