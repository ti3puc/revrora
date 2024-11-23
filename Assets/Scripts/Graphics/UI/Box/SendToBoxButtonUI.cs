using System.Collections;
using System.Collections.Generic;
using Character.Base;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SendToBoxButtonUI : MonoBehaviour
{
    public delegate void SendToBoxEvent(CharacterDefinition characterDefinition);
    public static event SendToBoxEvent OnSendToBox;

    [Header("Reference")]
    [SerializeField] private Button _switchButton;

    [Header("Debug")]
    [SerializeField, ReadOnly] private CharacterDefinition _currentDefinition;

    public Button Button => _switchButton;

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
        OnSendToBox?.Invoke(_currentDefinition);
    }
}
