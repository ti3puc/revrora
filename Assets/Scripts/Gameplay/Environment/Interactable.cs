using NaughtyAttributes;
using UnityEngine;

namespace Environment.Interaction
{
	[RequireComponent(typeof(BoxCollider))]
	public abstract class Interactable : MonoBehaviour
	{
		public delegate void RequestUIEvent(Interactor interactor, Interactable interactable);
		public static event RequestUIEvent OnRequestToShowUI;
		public static event RequestUIEvent OnRequestToHideUI;

		[Header("References")]
		[SerializeField] protected bool automaticInteraction;

		[Header("References")]
		[SerializeField] protected BoxCollider boxCollider;

		[Header("Debug")]
		[SerializeField, ReadOnly] protected Interactor currentInteractor;

		protected virtual void Awake()
		{
			Interactor.OnAnyInteraction += TryInteract;

			boxCollider = GetComponent<BoxCollider>();
			boxCollider.isTrigger = true;
		}

		protected virtual void OnDestroy()
		{
			Interactor.OnAnyInteraction -= TryInteract;
		}

		protected virtual void Reset()
		{
			boxCollider = GetComponent<BoxCollider>();
			boxCollider.isTrigger = true;
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			var interactor = other.GetComponent<Interactor>();
			if (interactor != null)
			{
				currentInteractor = interactor;

				if (automaticInteraction)
					currentInteractor.DoInteract();
				else
					OnRequestToShowUI?.Invoke(currentInteractor, this);
			}
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			var interactor = other.GetComponent<Interactor>();
			if (interactor != null)
			{
				OnRequestToHideUI?.Invoke(currentInteractor, this);
				currentInteractor = null;
			}
		}

		protected virtual void TryInteract(Interactor interactor)
		{
			if (interactor != currentInteractor)
				return;

			ReceiveInteraction();
		}

		public abstract void ReceiveInteraction();
	}
}