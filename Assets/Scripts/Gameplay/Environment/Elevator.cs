using UnityEngine;

namespace Environment.Interaction
{
	public class Elevator : Interactable
	{
		[Header("References: Elevator")]
		[SerializeField] private MovingPlatform platform;

		public override void DoInteract()
		{
			if (platform != null)
				platform.DoSingleMovement();
		}
	}
}
