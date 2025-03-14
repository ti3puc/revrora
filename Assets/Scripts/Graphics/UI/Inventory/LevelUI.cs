using System.Collections;
using System.Collections.Generic;
using Managers.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevelText;
    [SerializeField] private Slider _xpBar;
    [SerializeField] private TMP_Text _xpBarText;

    private PlayerLevel PlayerLevel => PlayerManager.Instance.PlayerLevel;

    private void OnEnable()
    {
        // workaround to update the UI
        PlayerLevel.AddExperience(0);
    }

    private void LateUpdate()
    {
        _currentLevelText.text = (PlayerLevel.Level + 1).ToString();

        _xpBar.minValue = 0;
        _xpBar.maxValue = PlayerLevel.CurrentMaxExperience;
        _xpBar.value = PlayerLevel.Experience;

        if (PlayerLevel.Level == PlayerLevel.MaxLevelTotal)
            _xpBarText.text = "MAX";
        else if (PlayerLevel.IsLevelLocked)
            _xpBarText.text = "LOCKED";
        else
            _xpBarText.text = $"{PlayerLevel.Experience} / {PlayerLevel.CurrentMaxExperience}";
    }
}
