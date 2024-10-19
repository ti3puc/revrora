using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

namespace UI.Menu.Settings
{
    public class SliderSettingsButton : MonoBehaviour
    {
        #region Fields

        [Header("Settings")]
        [SerializeField] private int valueToIncrease = 1;
        [SerializeField] private Vector2Int minMaxRange = new Vector2Int(0, 100);
        [SerializeField] private bool quickChangeValueOnHold;
        [SerializeField, ShowIf("quickChangeValueOnHold")] private float timeToStartHold = .5f;

        [Header("References")]
        [SerializeField] private Button firstButton;
        [SerializeField] private TMP_Text valueText;
        [SerializeField] private Button secondButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int currentValue;
        [SerializeField, ReadOnly] private bool isHoldingDecrease;
        [SerializeField, ReadOnly] private bool isHoldingIncrease;
        [SerializeField, ReadOnly] private float holdingTimer;

        #endregion

        #region Properties

        public int CurrentValue
        {
            get => currentValue;
            set
            {
                // sanity checks
                if (value > minMaxRange.y)
                    currentValue = minMaxRange.y;
                else if (value < minMaxRange.x)
                    currentValue = minMaxRange.x;
                else
                    currentValue = value;

                UpdateTextValue();
            }
        }

        #endregion

        #region Unity Messages

        private void Awake()
        {
            firstButton.onClick.AddListener(DecreaseValue);
            secondButton.onClick.AddListener(IncreaseValue);

            CurrentValue = 80;
            UpdateTextValue();
        }

        private void OnDestroy()
        {
            firstButton.onClick.RemoveListener(DecreaseValue);
            secondButton.onClick.RemoveListener(IncreaseValue);
        }

        private void Update()
        {
            if (!isHoldingDecrease && !isHoldingIncrease) return;

            holdingTimer += Time.deltaTime;
            if (holdingTimer > timeToStartHold)
            {
                if (isHoldingDecrease)
                    DecreaseValue();
                else if (isHoldingIncrease)
                    IncreaseValue();
            }
        }

        #endregion

        #region Private Methods

        private void UpdateTextValue()
        {
            valueText.text = CurrentValue.ToString();
        }

        private void DecreaseValue()
        {
            CurrentValue -= valueToIncrease;
        }

        private void IncreaseValue()
        {
            CurrentValue += valueToIncrease;
        }

        #endregion

        #region Public Methods

        // method used on EventTrigger component at Unity inspector
        public void DecreaseValueOnHoldStart()
        {
            if (!quickChangeValueOnHold) return;
            isHoldingDecrease = true;
        }

        // method used on EventTrigger component at Unity inspector
        public void DecreaseValueOnHoldEnd()
        {
            if (!quickChangeValueOnHold) return;
            isHoldingDecrease = false;
            holdingTimer = 0;
        }

        // method used on EventTrigger component at Unity inspector
        public void IncreaseValueOnHoldStart()
        {
            if (!quickChangeValueOnHold) return;
            isHoldingIncrease = true;
        }

        // method used on EventTrigger component at Unity inspector
        public void IncreaseValueOnHoldEnd()
        {
            if (!quickChangeValueOnHold) return;
            isHoldingIncrease = false;
            holdingTimer = 0;
        }

        #endregion
    }
}
