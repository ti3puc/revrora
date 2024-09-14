using System;
using System.Collections.Generic;
using Character.Class;
using Infra.Exception.Managers;

public class PartyManager : Singleton<PartyManager>
{
    private List<ICharacterClass> _partyMembers;
    
    private const int MinPartySize = 1;
    private const int MaxPartySize = 6;

    protected override void Awake()
    {
        base.Awake();
        _partyMembers = new List<ICharacterClass>();
    }

    public void InitializePartyManager(List<ICharacterClass> partyMembers)
    {
        if (partyMembers.Count < MinPartySize || partyMembers.Count > MaxPartySize)
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

    public void AddPartyMember(ICharacterClass character)
    {
        if (_partyMembers.Count >= MaxPartySize)
        {
            throw new PartyManagerPartyIsFullException();
        }
        try
        {
            _partyMembers.Add(character);
        }
        catch (Exception exception)
        {
            throw new PartyManagerGeneralException(exception.Message);
        }
    }
    
    public void RemovePartyMember(ICharacterClass character)
    {
        if (_partyMembers.Count <= MinPartySize)
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
        }
        catch (Exception exception)
        {
            throw new PartyManagerGeneralException(exception.Message);
        }
    }
    
    public void SwitchMemberPosition(ICharacterClass character1, ICharacterClass character2)
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
        }
        catch (Exception exception)
        {
            throw new PartyManagerGeneralException(exception.Message);
        }
    }
    
    public List<ICharacterClass> GetPartyMembers()
    {
        return _partyMembers;
    }
}