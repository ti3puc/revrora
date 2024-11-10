using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Environment.Interaction
{
	public abstract class Interactable : MonoBehaviour
	{
		public delegate void RequestUIEvent(Interactor interactor, Interactable interactable);
		public static event RequestUIEvent OnRequestToShowUI;
		public static event RequestUIEvent OnRequestToHideUI;

		[Header("References")]
		[SerializeField] protected bool _automaticInteraction;

		[Header("References")]
		[SerializeField] protected bool _useBoxCollider = true;
		[SerializeField, ShowIf("_useBoxCollider")] protected BoxCollider _boxCollider;
		[SerializeField, HideIf("_useBoxCollider")] protected SphereCollider _sphereCollider;

		[Header("Debug")]
		[SerializeField, ReadOnly] protected Interactor _currentInteractor;

		protected virtual void Awake()
		{
			Interactor.OnAnyInteraction += TryInteract;

			if (_useBoxCollider)
			{
				_boxCollider = GetComponent<BoxCollider>();
				_boxCollider.isTrigger = true;
			}
			else
			{
				_sphereCollider = GetComponent<SphereCollider>();
				_sphereCollider.isTrigger = true;
			}
		}

		protected virtual void OnDestroy()
		{
			Interactor.OnAnyInteraction -= TryInteract;
		}

		protected virtual void Reset()
		{
			if (_useBoxCollider)
			{
				_boxCollider = GetComponent<BoxCollider>();
				_boxCollider.isTrigger = true;
			}
			else
			{
				_sphereCollider = GetComponent<SphereCollider>();
				_sphereCollider.isTrigger = true;
			}
		}

		protected virtual void OnTriggerEnter(Collider other)
		{
			var interactor = other.GetComponent<Interactor>();
			if (interactor != null)
			{
				_currentInteractor = interactor;

				if (_automaticInteraction)
					_currentInteractor.DoInteract();
				else
					OnRequestToShowUI?.Invoke(_currentInteractor, this);
			}
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			var interactor = other.GetComponent<Interactor>();
			if (interactor != null)
			{
				UndoInteraction();
				OnRequestToHideUI?.Invoke(_currentInteractor, this);
				_currentInteractor = null;
			}
		}

		protected virtual void TryInteract(Interactor interactor)
		{
			if (interactor != _currentInteractor)
				return;

			ReceiveInteraction();
		}

		public abstract void ReceiveInteraction();
		public virtual void UndoInteraction() { }
	}
}