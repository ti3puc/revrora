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
			if (_currentInteractor != null && otherPortal != null)
			{
				var controller = _currentInteractor.GetComponent<CharacterController>();

				controller.enabled = false;
				_currentInteractor.transform.position = otherPortal.transform.position;
				controller.enabled = true;

				_currentInteractor = null;
			}
		}
	}
}
