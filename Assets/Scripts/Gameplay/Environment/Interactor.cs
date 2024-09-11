using Player.Input;
using UnityEngine;

namespace Environment.Interaction
{
	public class Interactor : MonoBehaviour
	{
		public delegate void InteractionEvent(Interactor interactor);
		public static event InteractionEvent OnAnyInteraction;

		private void Awake()
		{
			PlayerInput.OnInteractionStarted += DoInteract;
		}

		private void OnDestroy()
		{
			PlayerInput.OnInteractionStarted -= DoInteract;
		}

		public void DoInteract() => OnAnyInteraction?.Invoke(this);
	}
}