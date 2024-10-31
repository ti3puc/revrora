using System;
using System.Collections;
using System.Collections.Generic;
using Core.Managers;
using Managers;
using NaughtyAttributes;
using Player.Input;
using UnityEngine;

namespace UI
{
    public class CanvasManager : Singleton<CanvasManager>
    {
        [Header("References")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Canvas inventoryCanvas;
        [SerializeField] private Canvas pauseCanvas;
        [SerializeField] private DialogManager dialogCanvas;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool isInventoryOpen;

        public DialogManager DialogCanvas => dialogCanvas;

        protected override void Awake()
        {
            base.Awake();
            PlayerInput.OnToggleInventoryStarted += ToggleInventory;
        }

        private void OnDestroy()
        {
            PlayerInput.OnToggleInventoryStarted -= ToggleInventory;
        }

        public void ShowMainCanvas()
        {
            mainCanvas.enabled = true;
            inventoryCanvas.enabled = false;
            pauseCanvas.enabled = false;

            if (isInventoryOpen)
                isInventoryOpen = !isInventoryOpen;
        }

        public void ToggleInventory()
        {
            isInventoryOpen = !isInventoryOpen;

            mainCanvas.enabled = !isInventoryOpen;
            inventoryCanvas.enabled = isInventoryOpen;
            pauseCanvas.enabled = false;
        }
    }
}
