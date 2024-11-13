using System;

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

        public string SlotName => $"Slot {IndexId}";
    }

    [Serializable]
    public class PlayerData
    {

    }

    [Serializable]
    public class InventoryData
    {

    }
}