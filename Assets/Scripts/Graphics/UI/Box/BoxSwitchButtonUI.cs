using System.Collections;
using System.Collections.Generic;
using Character.Base;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class BoxSwitchButtonUI : MonoBehaviour
{
    public delegate void SwitchBoxEvent(CharacterDefinition characterDefinition);
    public static event SwitchBoxEvent OnSwitchFromBox;

    [Header("Reference")]
    [SerializeField] private Button _switchButton;

    [Header("Debug")]
    [SerializeField, ReadOnly] private CharacterDefinition _currentDefinition;

    private void Awake()
    {
        _switchButton.onClick.AddListener(RaiseSwitchBoxEvent);
    }

    private void OnDestroy()
    {
        _switchButton.onClick.RemoveListener(RaiseSwitchBoxEvent);
    }

    public void SetCharacter(CharacterDefinition characterDefinition)
    {
        _currentDefinition = characterDefinition;
    }

    public void RaiseSwitchBoxEvent()
    {
        OnSwitchFromBox?.Invoke(_currentDefinition);
    }
}
