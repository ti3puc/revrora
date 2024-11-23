using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Managers.Box;
using Managers.Party;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Box
{
    public class BoxManagementUI : MonoBehaviour
    {
        [Header("References Box")]
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _partyMemberPrefab;

        [Header("References Party")]
        [SerializeField] private List<Button> _partyMembersButtons = new List<Button>();

        [Header("Debug")]
        [SerializeField, ReadOnly] private Button _currentPartyMemberSelected;

        private void Awake()
        {
            if (_partyMembersButtons.Count <= 0 || _partyMembersButtons.Count > PartyManager.Instance.MaxPartySize)
            {
                Debug.LogError("PartyMembersButtons count is invalid");
                return;
            }

            _partyMembersButtons[0].onClick.AddListener(() => SelectPartyMember(0));
            _partyMembersButtons[1].onClick.AddListener(() => SelectPartyMember(1));
            _partyMembersButtons[2].onClick.AddListener(() => SelectPartyMember(2));

            SendToBoxButtonUI.OnSendToBox += SendToBox;
            BoxSwitchButtonUI.OnSwitchFromBox += SwitchBoxCharacter;
        }

        private void OnDestroy()
        {
            foreach (var partyMemberButton in _partyMembersButtons)
                partyMemberButton.onClick.RemoveAllListeners();

            SendToBoxButtonUI.OnSendToBox -= SendToBox;
            BoxSwitchButtonUI.OnSwitchFromBox -= SwitchBoxCharacter;
        }

        private void OnEnable()
        {
            UpdateBoxMembers();
            UpdatePartyMembers();
        }

        private void UpdateBoxMembers()
        {
            ClearBoxMembers();

            for (int i = 0; i < BoxManager.Instance.BoxCharacters.Count; i++)
            {
                var partyMember = Instantiate(_partyMemberPrefab, _content.transform);
                var image = partyMember.transform.GetChild(0).GetComponent<RawImage>();
                image.color = new Color(1, 1, 1, 1);
                image.texture = BoxManager.Instance.BoxCharacters[i].Icon;

                var switchButton = partyMember.GetComponent<BoxSwitchButtonUI>();
                switchButton.SetCharacter(BoxManager.Instance.BoxCharacters[i]);
            }
        }

        private void ClearBoxMembers()
        {
            foreach (Transform child in _content.transform)
                Destroy(child.gameObject);
        }

        private void UpdatePartyMembers()
        {
            for (int i = 0; i < _partyMembersButtons.Count; i++)
            {
                var partyInstanceMember = PartySceneInstanceManager.Instance.PartyBaseCharacters[i];
                bool hasPartyMember = PartyManager.Instance.PartyMembers.Contains(partyInstanceMember);

                var sendToBoxButton = _partyMembersButtons[i].GetComponentInChildren<SendToBoxButtonUI>(true);
                sendToBoxButton.gameObject.SetActive(hasPartyMember);

                var image = _partyMembersButtons[i].transform.GetChild(0).GetComponent<RawImage>();

                if (!hasPartyMember)
                {
                    image.color = new Color(1, 1, 1, 0);
                    continue;
                }

                image.color = new Color(1, 1, 1, 1);
                image.texture = partyInstanceMember.CharacterDefinition.Icon;

                sendToBoxButton.SetCharacter(partyInstanceMember.CharacterDefinition);
            }
        }

        private void SelectPartyMember(int index)
        {
            if (index >= PartyManager.Instance.MaxPartySize)
                return;

            _currentPartyMemberSelected = _partyMembersButtons[index];
            foreach (var partyMemberButton in _partyMembersButtons)
                partyMemberButton.interactable = partyMemberButton != _partyMembersButtons[index];
        }

        private void SwitchBoxCharacter(CharacterDefinition characterDefinition)
        {
            if (_currentPartyMemberSelected == null)
                return;

            var boxMember = BoxManager.Instance.BoxCharacters.Find(x => x.Id == characterDefinition.Id);
            if (boxMember == null)
                return;

            BoxManager.Instance.RemoveCharacterFromBox(boxMember);

            if (characterDefinition != null)
            {
                var partyMemberIndex = _partyMembersButtons.IndexOf(_currentPartyMemberSelected);
                PartySceneInstanceManager.Instance.AddCharacterToParty(characterDefinition, partyMemberIndex);
            }

            UpdateBoxMembers();
            UpdatePartyMembers();

            _currentPartyMemberSelected = null;
            foreach (var partyMemberButton in _partyMembersButtons)
                partyMemberButton.interactable = true;
        }

        private void SendToBox(CharacterDefinition characterDefinition)
        {
            var partyMember = PartyManager.Instance.PartyMembers.Find(x => x.Id == characterDefinition.Id);
            if (partyMember == null)
                return;

            BoxManager.Instance.AddCharacterToBox(partyMember.CharacterDefinition);

            var instanceIndex = PartySceneInstanceManager.Instance.PartyBaseCharacters.IndexOf(partyMember);
            PartySceneInstanceManager.Instance.RemoveCharacterFromParty(partyMember.CharacterDefinition, instanceIndex);

            PartyManager.Instance.RemovePartyMember(partyMember);

            PartyManager.Instance.SwitchActiveMemberIndex(PartyManager.Instance.ActiveMemberIndex);

            UpdateBoxMembers();
            UpdatePartyMembers();

            _currentPartyMemberSelected = null;
            foreach (var partyMemberButton in _partyMembersButtons)
                partyMemberButton.interactable = true;
        }
    }
}
