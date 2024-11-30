using System;
using System.Collections.Generic;
using Character.Base;
using UnityEngine;

namespace Persistence
{
    [Serializable]
    public class GameData
    {
        public int IndexId;
        public string CurrentScene;
        public string StartDate;
        public string LastPlayedDate;
        public PlayerData PlayerData;
        public InventoryData InventoryData;
        public CreaturesData CreaturesData;
        public CombatSceneWinData CombatSceneWinData;

        public string SlotName => $"Slot {IndexId}";

        public GameData()
        {
            PlayerData = new PlayerData();
            InventoryData = new InventoryData();
            CreaturesData = new CreaturesData();
            CombatSceneWinData = new CombatSceneWinData();
        }
    }

    [Serializable]
    public class PlayerData
    {

    }

    [Serializable]
    public class InventoryData
    {

    }

    [Serializable]
    public class CreaturesData
    {
        public int ActivePartyMemberIndex = 0;
        public List<CharacterDefinition> Party = new List<CharacterDefinition>();
        public List<CharacterDefinition> Box = new List<CharacterDefinition>();
    }

    [Serializable]
    public class CombatSceneWinData
    {
        public List<int> CombatSceneIds = new List<int>();
        public Vector3 LastPlayerPosition;
    }
}