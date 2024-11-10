using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Managers.Party;
using UnityEngine;

namespace Creatures
{
    public class CreatureCharacter : BaseCharacter
    {
        [Header("Debug")]
        [SerializeField] private bool _addToPartyOnStart;
        [SerializeField] private int _addToPartyIndex;

        private void Start()
        {
            if (_addToPartyOnStart)
                PartyManager.Instance.AddPartyMember(this, _addToPartyIndex);
        }
    }
}