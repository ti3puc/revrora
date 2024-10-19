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
        [SerializeField] private float _addToPartyDelay;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_addToPartyDelay);
            if (_addToPartyOnStart)
                PartyManager.Instance.AddPartyMember(this);
        }
    }
}