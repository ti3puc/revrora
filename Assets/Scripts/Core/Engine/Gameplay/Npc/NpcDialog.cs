using System.Collections;
using System.Collections.Generic;
using Environment.Interaction;
using Infra.Handler;
using UI;
using UnityEngine;

namespace Npc.Dialog
{
    public class NpcDialog : Interactable
    {
        [Header("Dialogue Reference")]
        [SerializeField] private Dialogue _dialogue;

        public override void ReceiveInteraction()
        {
            if (_dialogue == null)
            {
                GameLog.Error(this, "Dialogue is missing");
                return;
            }

            CanvasManager.Instance.DialogCanvas.AddDialogue(_dialogue);
        }

        public override void UndoInteraction()
        {
            CanvasManager.Instance.DialogCanvas.ClearDialogue();
        }
    }
}