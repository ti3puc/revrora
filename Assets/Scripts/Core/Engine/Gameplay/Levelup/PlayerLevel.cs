using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Persistence;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxLevel = 4;
    [SerializeField] private List<int> _experiencePerLevel = new List<int>();

    [Header("Debug")]
    [SerializeField, ReadOnly] private int _level = 0;
    [SerializeField, ReadOnly] private int _experience = 0;

    public int Level => _level;
    public int Experience => _experience;

    private void Awake()
    {
        var playerData = SaveSystem.Instance.GameData.PlayerData;
        _level = playerData.Level;
        _experience = playerData.Experience;
    }

    public void AddExperience(int experience)
    {
        if (_level >= _maxLevel)
        {
            _experience = _experiencePerLevel[_experiencePerLevel.Count - 1];
            return;
        }

        _experience += experience;

        if (_experience >= _experiencePerLevel[_level])
        {
            _experience -= _experiencePerLevel[_level];
            LevelUp();
        }

        var playerData = SaveSystem.Instance.GameData.PlayerData;
        playerData.Level = _level;
        playerData.Experience = _experience;
    }

    private void LevelUp()
    {
        _level++;

        if (_level >= _maxLevel)
            _experience = _experiencePerLevel[_experiencePerLevel.Count - 1];
    }

    [Button]
    private void DebugAddExperience()
    {
        AddExperience(11);
    }
}
