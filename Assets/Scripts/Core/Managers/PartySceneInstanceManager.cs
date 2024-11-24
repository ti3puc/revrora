using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Managers;
using Managers.Party;
using NaughtyAttributes;
using Persistence;
using UnityEngine;

public class PartySceneInstanceManager : Singleton<PartySceneInstanceManager>
{
    [Header("References")]
    [SerializeField] private List<BaseCharacter> _partyBaseCharacters;

    [Header("Debug")]
    [SerializeField] private CharacterDefinition _initialCharacter;
    [SerializeField, ReadOnly] private List<CharacterDefinition> _partyCharacterDefinitions;

    public List<BaseCharacter> PartyBaseCharacters => _partyBaseCharacters;

    protected override void Awake()
    {
        base.Awake();

        if (SaveSystem.Instance.GameData == null)
            SaveSystem.Instance.GameData = new GameData();

        _partyCharacterDefinitions = SaveSystem.Instance.GameData.CreaturesData.Party;

        if (_partyCharacterDefinitions.Count > 0)
        {
            for (int i = 0; i < _partyCharacterDefinitions.Count; i++)
                AddCharacterToParty(_partyCharacterDefinitions[i], i, true);
        }
        else
        {
            AddCharacterToParty(_initialCharacter, 0, true);
        }

        for (int j = 0; j < _partyBaseCharacters.Count; j++)
        {
            BaseCharacter partyMember = _partyBaseCharacters[j];
            partyMember.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void AddCharacterToParty(CharacterDefinition character, int index, bool withDelay = false)
    {
        if (!_partyCharacterDefinitions.Contains(character))
            _partyCharacterDefinitions.Add(character);

        _partyBaseCharacters[index].Initialize(character);
        if (withDelay)
            StartCoroutine(AddPartyMemberDelayed(index));
        else
            PartyManager.Instance.AddPartyMember(_partyBaseCharacters[index], index);

        SaveSystem.Instance.GameData.CreaturesData.Party = _partyCharacterDefinitions;
    }

    public void RemoveCharacterFromParty(CharacterDefinition character, int instanceIndex)
    {
        int index = _partyCharacterDefinitions.IndexOf(character);
        if (index != -1)
        {
            _partyCharacterDefinitions.Remove(character);
            _partyBaseCharacters[instanceIndex].Initialize(null);
        }

        SaveSystem.Instance.GameData.CreaturesData.Party = _partyCharacterDefinitions;
    }

    private IEnumerator AddPartyMemberDelayed(int index)
    {
        yield return new WaitForSeconds(0.1f);
        PartyManager.Instance.AddPartyMember(_partyBaseCharacters[index], index);
    }
}
