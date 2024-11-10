using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

namespace UI.Menu.Settings
{
    public class SliderSettingsButton : MonoBehaviour
    {
        #region Fields

        [Header("Settings")]
        [SerializeField] private int _valueToIncrease = 1;
        [SerializeField] private Vector2Int _minMaxRange = new Vector2Int(0, 100);
        [SerializeField] private bool _quickChangeValueOnHold = true;
        [SerializeField, ShowIf("_quickChangeValueOnHold")] private float _timeToStartHold = .5f;

        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onValueChange;

        [Header("References")]
        [SerializeField] private Button _firstButton;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Button _secondButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int _currentValue;
        [SerializeField, ReadOnly] private bool _isHoldingDecrease;
        [SerializeField, ReadOnly] private bool _isHoldingIncrease;
        [SerializeField, ReadOnly] private float _holdingTimer;

        #endregion

        #region Properties

        public int CurrentValue
        {
            get => _currentValue;
            set
            {
                // sanity checks
                if (value > _minMaxRange.y)
                    _currentValue = _minMaxRange.y;
                else if (value < _minMaxRange.x)
                    _currentValue = _minMaxRange.x;
                else
                    _currentValue = value;

                UpdateTextValue();
            }
        }

        #endregion

        #region Unity Messages

        private void Awake()
        {
            _firstButton.onClick.AddListener(DecreaseValue);
            _secondButton.onClick.AddListener(IncreaseValue);

            UpdateTextValue();
        }

        private void OnDestroy()
        {
            _firstButton.onClick.RemoveListener(DecreaseValue);
            _secondButton.onClick.RemoveListener(IncreaseValue);
        }

        private void Update()
        {
            if (!_isHoldingDecrease && !_isHoldingIncrease) return;

            _holdingTimer += Time.deltaTime;
            if (_holdingTimer > _timeToStartHold)
            {
                if (_isHoldingDecrease)
                    DecreaseValue();
                else if (_isHoldingIncrease)
                    IncreaseValue();
            }
        }

        #endregion

        #region Private Methods

        private void UpdateTextValue()
        {
            _valueText.text = CurrentValue.ToString();
        }

        private void DecreaseValue()
        {
            CurrentValue -= _valueToIncrease;
            _onValueChange?.Invoke(CurrentValue);
        }

        private void IncreaseValue()
        {
            CurrentValue += _valueToIncrease;
            _onValueChange?.Invoke(CurrentValue);
        }

        #endregion

        #region Public Methods

        // method used on EventTrigger component at Unity inspector
        public void DecreaseValueOnHoldStart()
        {
            if (!_quickChangeValueOnHold) return;
            _isHoldingDecrease = true;
        }

        // method used on EventTrigger component at Unity inspector
        public void DecreaseValueOnHoldEnd()
        {
            if (!_quickChangeValueOnHold) return;
            _isHoldingDecrease = false;
            _holdingTimer = 0;
        }

        // method used on EventTrigger component at Unity inspector
        public void IncreaseValueOnHoldStart()
        {
            if (!_quickChangeValueOnHold) return;
            _isHoldingIncrease = true;
        }

        // method used on EventTrigger component at Unity inspector
        public void IncreaseValueOnHoldEnd()
        {
            if (!_quickChangeValueOnHold) return;
            _isHoldingIncrease = false;
            _holdingTimer = 0;
        }

        #endregion
    }
}
