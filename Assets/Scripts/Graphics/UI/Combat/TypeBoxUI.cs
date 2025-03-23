using System.Collections;
using System.Collections.Generic;
using Character.Class;
using Core.Domain.Character.Moves;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeBoxUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _typeBox;
    [SerializeField] private TMP_Text _typeBoxText;

    [Header("Colors")]
    [SerializeField] private Color _normalColor = Color.magenta;
    [SerializeField] private Color _fireColor = Color.red;
    [SerializeField] private Color _waterColor = Color.blue;
    [SerializeField] private Color _earthColor = Color.green;
    [SerializeField] private Color _airColor = Color.grey;

    public void SetTypeBox(CharacterTypes typeToSet)
    {
        _typeBox.color = typeToSet switch
        {
            CharacterTypes.NORMAL => _normalColor,
            CharacterTypes.FIRE => _fireColor,
            CharacterTypes.WATER => _waterColor,
            CharacterTypes.EARTH => _earthColor,
            CharacterTypes.AIR => _airColor,
            _ => _normalColor,
        };

        var type = char.ToUpper(typeToSet.ToString()[0]) + typeToSet.ToString().Substring(1).ToLower();
        _typeBoxText.text = type;
    }
}
