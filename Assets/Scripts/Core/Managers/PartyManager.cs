using System;
using System.Collections.Generic;
using Character.Base;
using Character.Class;
using Infra.Exception.Managers;
using NaughtyAttributes;
using UnityEngine;

namespace Managers.Party
{
    public class PartyManager : Singleton<PartyManager>
    {
        public static event Action OnPartyChangedEvent;

        [Header("Settings")]
        [SerializeField] private int _minPartySize = 1;
        [SerializeField] private int _maxPartySize = 6;
        [SerializeField] private int _partyMembersToShow = 1;

        [Header("Debug")]
        [SerializeField, ReadOnly] private List<BaseCharacter> _partyMembers;
        [SerializeField, ReadOnly] private BaseCharacter _activePartyMember;

        public List<BaseCharacter> PartyMembers => _partyMembers;
        public BaseCharacter ActivePartyMember => _activePartyMember;

        #region Unity Messages
        protected override void Awake()
        {
            base.Awake();
            _partyMembers = new List<BaseCharacter>(_maxPartySize);

            OnPartyChangedEvent += HandlePartyVisibility;
        }

        private void OnDestroy()
        {
            OnPartyChangedEvent -= HandlePartyVisibility;
        }
        #endregion

        #region Public Methods
        public void InitializePartyManager(List<BaseCharacter> partyMembers)
        {
            if (partyMembers.Count < _minPartySize || partyMembers.Count > _maxPartySize)
            {
                throw new PartyManagerGeneralException("Party size is invalid!");
            }
            try
            {
                _partyMembers = partyMembers;
            }
            catch (Exception exception)
            {
                throw new PartyManagerGeneralException(exception.Message);
            }
        }

        public void AddPartyMember(BaseCharacter character, int index = -1)
        {
            if (_partyMembers.Count >= _maxPartySize)
            {
                throw new PartyManagerPartyIsFullException();
            }
            try
            {
                if (index >= 0 && index <= _partyMembers.Count - 1)
                    _partyMembers.Insert(index, character);
                else
                    _partyMembers.Add(character);

                OnPartyChangedEvent?.Invoke();
            }
            catch (Exception exception)
            {
                throw new PartyManagerGeneralException(exception.Message);
            }
        }

        public void RemovePartyMember(BaseCharacter character)
        {
            if (_partyMembers.Count <= _minPartySize)
            {
                throw new PartyManagerPartyIsEmptyException();
            }
            if (!_partyMembers.Contains(character))
            {
                throw new PartyManagerCharacterNotInPartyException();
            }
            try
            {
                _partyMembers.Remove(character);
                OnPartyChangedEvent?.Invoke();
            }
            catch (Exception exception)
            {
                throw new PartyManagerGeneralException(exception.Message);
            }
        }

        public void SwitchMemberPosition(BaseCharacter character1, BaseCharacter character2)
        {
            if (!_partyMembers.Contains(character1))
            {
                throw new PartyManagerCharacterNotInPartyException("Character 1 is not in party!");
            }
            if (!_partyMembers.Contains(character2))
            {
                throw new PartyManagerCharacterNotInPartyException("Character 2 is not in party!");
            }
            try
            {
                int index1 = _partyMembers.IndexOf(character1);
                int index2 = _partyMembers.IndexOf(character2);
                _partyMembers[index1] = character2;
                _partyMembers[index2] = character1;

                OnPartyChangedEvent?.Invoke();
            }
            catch (Exception exception)
            {
                throw new PartyManagerGeneralException(exception.Message);
            }
        }

        public void SwitchActiveMember(int index)
        {
            if (_partyMembers.Count <= index)
                throw new PartyManagerCharacterNotInPartyException();

            try
            {
                _activePartyMember = _partyMembers[index];

                OnPartyChangedEvent?.Invoke();
            }
            catch (Exception exception)
            {
                throw new PartyManagerGeneralException(exception.Message);
            }
        }
        #endregion

        #region Private Methods
        private void HandlePartyVisibility()
        {
            if (_partyMembers.Count <= _minPartySize)
                return;

            if (_activePartyMember == null)
                _activePartyMember = _partyMembers[0];

            for (int i = 0; i < _partyMembers.Count; i++)
            {
                bool isActiveMember = _partyMembers[i] == _activePartyMember;
                _partyMembers[i].transform.GetChild(0).gameObject.SetActive(isActiveMember);
            }
        }
        #endregion
    }
}