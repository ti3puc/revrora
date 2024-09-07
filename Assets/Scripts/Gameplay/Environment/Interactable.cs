using NaughtyAttributes;
using UnityEngine;

namespace Environment.Interaction
{
	[RequireComponent(typeof(SphereCollider))]
	public abstract class Interactable : MonoBehaviour
	{
		public delegate void RequestUIEvent(Interactor interactor, Interactable interactable);
		public static event RequestUIEvent OnRequestToShowUI;
		public static event RequestUIEvent OnRequestToHideUI;

		[Header("References")]
		[SerializeField] private SphereCollider sphereCollider;

		[Header("Debug")]
		[SerializeField, ReadOnly] private Interactor currentInteractor;

		protected virtual void Awake()
		{
			Interactor.OnAnyInteraction += TryInteract;

			sphereCollider = GetComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
		}

		protected virtual void OnDestroy()
		{
			Interactor.OnAnyInteraction -= TryInteract;
		}

		protected virtual void Reset()
		{
			sphereCollider = GetComponent<SphereCollider>();
			sphereCollider.isTrigger = true;
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			var interactor = other.GetComponent<Interactor>();
			if (interactor != null)
			{
				currentInteractor = interactor;
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

			DoInteract();
		}

		public abstract void DoInteract();
	}
}