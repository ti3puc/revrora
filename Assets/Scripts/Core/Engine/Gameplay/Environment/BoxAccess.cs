using System;
using System.Collections;
using System.Collections.Generic;
using Environment.Interaction;
using UnityEngine;

namespace Environment.Box
{
    public class BoxAccess : Interactable
    {
        public static event Action OnBoxAccessed;
        public static event Action OnBoxExit;

        public override void ReceiveInteraction()
        {
            OnBoxAccessed?.Invoke();
        }

        public override void UndoInteraction()
        {
            base.UndoInteraction();
            OnBoxExit?.Invoke();
        }
    }
}
