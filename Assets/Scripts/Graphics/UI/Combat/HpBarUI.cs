using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    [Header("Name")]
    [SerializeField] private TMP_Text _nameText;

    [Header("Health")]
    [SerializeField] private Slider _hpSlider;

    [Header("Type")]
    [SerializeField] private TypeBoxUI _typeBoxUI;

    public TMP_Text NameText => _nameText;
    public Slider HpSlider => _hpSlider;

    public void PopulateCharacterInfo(BaseCharacter character)
    {
        _nameText.text = character.Name;
        _typeBoxUI.SetTypeBox(character.Type);
    }

    public void UpdateHealthBar(BaseCharacter character)
    {
        float value = (float)character.CharacterStats.HP / character.CharacterStats.MaxHP;
        double roundedValue = Math.Round(value, 2);
        _hpSlider.value = (float)roundedValue;
    }
}
