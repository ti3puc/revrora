using System;
using Infra.Handler;
using Inventory;
using Inventory.Items;
using Managers.Player;
using NaughtyAttributes;
using UI;
using UnityEngine;

namespace Environment.Interaction
{
	public abstract class ElementInteractable : Interactable
	{
		public static event Action OnInteractableDestroyed;

		[Header("Settings: Element")]
		[SerializeField] private bool _isOneTimeUse = true;
		[SerializeField] private bool _hasItemRequirement = true;
		[SerializeField, ShowIf("_hasItemRequirement")] private ItemData _requiredItem;
		[SerializeField] private bool _hasDialogForMissingItem = true;
		[SerializeField, ShowIf("_hasDialogForMissingItem")] private Dialogue _missingItemDialogue;

		public override void ReceiveInteraction()
		{
			if (_hasItemRequirement && _requiredItem != null && PlayerManager.Instance.PlayerInventory.GetItem(_requiredItem) == null)
			{
				if (_hasDialogForMissingItem)
					CanvasManager.DialogCanvas.AddDialogue(_missingItemDialogue, null);

				return;
			}

			DoInteraction();

			if (_isOneTimeUse)
			{
				UndoInteraction();
				OnInteractableDestroyed?.Invoke();
				Destroy(this);
			}
		}

		public override void UndoInteraction()
		{
			CanvasManager.DialogCanvas.ClearDialogue();
		}

		public abstract void DoInteraction();
	}
}
