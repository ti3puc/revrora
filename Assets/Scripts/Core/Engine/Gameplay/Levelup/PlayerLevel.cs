using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers.Player;
using NaughtyAttributes;
using Persistence;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<int> _maxLevelPerRegion = new List<int>();
    [SerializeField] private List<int> _experiencePerLevel = new List<int>();

    [Header("Debug")]
    [SerializeField, ReadOnly] private int _level = 0;
    [SerializeField, ReadOnly] private int _experience = 0;
    [SerializeField, ReadOnly] private int _currentMaxLevel = 0;
    [SerializeField, ReadOnly] private bool _isLevelLocked;

    public int Level => _level;
    public int Experience => _experience;
    public int CurrentMaxExperience => _level < _experiencePerLevel.Count ? _experiencePerLevel[_level] : MaxLevelTotal;
    public int MaxLevelTotal => _maxLevelPerRegion[_maxLevelPerRegion.Count - 1];
    public bool IsLevelLocked => _isLevelLocked = _level > 0 && _level >= _currentMaxLevel;

    private void Awake()
    {
        var playerData = SaveSystem.Instance.GameData.PlayerData;
        _level = playerData.Level;
        _experience = playerData.Experience;
    }

    private void OnValidate()
    {
        if (_experiencePerLevel.Count < MaxLevelTotal)
        {
            for (int i = _experiencePerLevel.Count; i < MaxLevelTotal; i++)
                _experiencePerLevel.Add(0);
        }
        else if (_experiencePerLevel.Count > MaxLevelTotal)
            _experiencePerLevel.RemoveRange(MaxLevelTotal, _experiencePerLevel.Count - MaxLevelTotal);
    }

    public void AddExperience(int experience)
    {
        _currentMaxLevel = _maxLevelPerRegion[Mathf.Clamp(PlayerManager.Instance.PlayerInventory.KeysCount, 0, _maxLevelPerRegion.Count - 1)];

        if (_level >= _currentMaxLevel)
        {
            _experience = 0;
            if (_currentMaxLevel >= MaxLevelTotal)
                _experience = _experiencePerLevel[_maxLevelPerRegion.Count - 1];
            return;
        }

        _experience += experience;

        if (_experience >= _experiencePerLevel[_level])
        {
            _experience -= _experiencePerLevel[_level];
            LevelUp(_currentMaxLevel);
        }

        var playerData = SaveSystem.Instance.GameData.PlayerData;
        playerData.Level = _level;
        playerData.Experience = _experience;
    }

    private void LevelUp(int currentMaxLevel)
    {
        _level++;

        if (_level >= currentMaxLevel)
        {
            _level = currentMaxLevel;
            _experience = 0;
            if (_currentMaxLevel >= MaxLevelTotal)
                _experience = _experiencePerLevel[_maxLevelPerRegion.Count - 1];
        }
    }

    [Button]
    private void DebugAddExperience()
    {
        AddExperience(11);
    }
}
