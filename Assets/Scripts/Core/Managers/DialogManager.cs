﻿using System;
using Infra.Handler;
using NaughtyAttributes;
using Player.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Managers
{
    public class DialogManager : MonoBehaviour
    {
        #region Fields

        [Header("Debug")]
        [SerializeField, ReadOnly] private Canvas _canvas;
        [SerializeField, ReadOnly] private TMP_Text _nameText;
        [SerializeField, ReadOnly] private TMP_Text _dialogText;
        [SerializeField, ReadOnly] private Image _nextPage;
        [SerializeField, ReadOnly] private Dialogue _dialogue;
        [SerializeField, ReadOnly] private int _dialogueIndex = 0;
        [SerializeField, ReadOnly] private bool _isInteracting = false;

        [Header("Debug: Test")]
        [SerializeField] private Dialogue _debugDialogue;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            PlayerInput.OnInteractionStarted += ReceivedInteraction;
            PlayerInput.OnInteractionCanceled += CanceledInteraction;
            SetupComponents();
        }

        private void OnDestroy()
        {
            PlayerInput.OnInteractionStarted -= ReceivedInteraction;
            PlayerInput.OnInteractionCanceled -= CanceledInteraction;
        }

        private void LateUpdate()
        {
            if (_dialogue == null
                || !_dialogue.HasSentences
                || _nameText == null || _dialogText == null || _nextPage == null)
            {
                _canvas.enabled = false;
                _dialogueIndex = 0;
                return;
            }

            _canvas.enabled = true;
            _nextPage.enabled = _dialogue.Sentences.Count > 1;

            if (_isInteracting)
            {
                if (_dialogueIndex < _dialogue.Sentences.Count)
                {
                    _dialogText.text = _dialogue.Sentences[_dialogueIndex];
                    _dialogueIndex++;
                }
                else
                {
                    _dialogueIndex = 0;
                    _dialogue = null;
                }

                _isInteracting = false;
            }
        }

        #endregion

        #region Public Methods

        public void AddDialogue(Dialogue dialogue)
        {
            if (_dialogue != null)
                return;

            _dialogue = dialogue;
            _nameText.text = _dialogue.Name;
            _dialogText.text = _dialogue.Sentences[_dialogueIndex];
        }

        public void ClearDialogue()
        {
            _dialogue = null;
            _nameText.text = string.Empty;
            _dialogText.text = string.Empty;
            _dialogueIndex = 0;
        }

        #endregion

        #region Private Methods

        private void SetupComponents()
        {
            _canvas = GetComponent<Canvas>();

            TMP_Text[] tmpTexts = GetComponentsInChildren<TMP_Text>();
            foreach (var tmpText in tmpTexts)
            {
                if (tmpText.name.Equals("Name Text"))
                    _nameText = tmpText;
                else if (tmpText.name.Equals("Text Text"))
                    _dialogText = tmpText;
            }

            Image[] images = GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                if (image.name.Equals("Next Page"))
                    _nextPage = image;
            }

            if (_nameText == null || _dialogText == null || _nextPage == null)
            {
                GameLog.Warning(this, "Alguém mudou o nome dos componentes de texto/next page do diálogo!");
            }
        }

        private void ReceivedInteraction()
        {
            _isInteracting = true;
        }

        private void CanceledInteraction()
        {
            _isInteracting = false;
        }

        [Button]
        private void DebugShowDialogue()
        {
            AddDialogue(_debugDialogue);
        }

        [Button]
        private void DebugClearDialogue()
        {
            ClearDialogue();
        }

        #endregion
    }
}