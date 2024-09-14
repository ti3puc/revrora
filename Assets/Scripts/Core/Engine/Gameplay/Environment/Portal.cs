using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Interaction
{
	public class Portal : Interactable
	{
		[SerializeField] private Portal otherPortal;

		public override void ReceiveInteraction()
		{
			if (currentInteractor != null && otherPortal != null)
			{
				var controller = currentInteractor.GetComponent<CharacterController>();

				controller.enabled = false;
				currentInteractor.transform.position = otherPortal.transform.position;
				controller.enabled = true;

				currentInteractor = null;
			}
		}
	}
}
