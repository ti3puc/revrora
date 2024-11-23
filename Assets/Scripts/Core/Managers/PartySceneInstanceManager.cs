using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Managers.Party;
using NaughtyAttributes;
using Persistence;
using UnityEngine;

public class PartySceneInstanceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<BaseCharacter> _partyBaseCharacters;

    [Header("Debug")]
    [SerializeField] private CharacterDefinition _initialCharacter;
    [SerializeField, ReadOnly] private List<CharacterDefinition> _partyCharacterDefinitions;

    private void Awake()
    {
        _partyCharacterDefinitions = SaveSystem.Instance.GameData.CreaturesData.Party;

        if (_partyCharacterDefinitions.Count > 0)
        {
            for (int i = 0; i < _partyCharacterDefinitions.Count; i++)
                InitializePartyMember(_partyCharacterDefinitions[i], i);
        }
        else
        {
            AddCharacterToParty(_initialCharacter, 0);
        }
    }

    public void AddCharacterToParty(CharacterDefinition character, int index)
    {
        _partyCharacterDefinitions.Add(character);
        InitializePartyMember(character, index);
    }

    public void InitializePartyMember(CharacterDefinition character, int index)
    {
        _partyBaseCharacters[index].Initialize(character);
        StartCoroutine(AddPartyMemberDelayed(index));
    }

    private IEnumerator AddPartyMemberDelayed(int index)
    {
        yield return new WaitForSeconds(0.1f);
        PartyManager.Instance.AddPartyMember(_partyBaseCharacters[index], index);
    }
}
