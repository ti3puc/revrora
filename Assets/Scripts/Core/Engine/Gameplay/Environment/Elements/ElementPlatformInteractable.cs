using UnityEngine;

namespace Environment.Interaction
{
	public class ElementPlatformInteractable : ElementInteractable
	{
		[Header("References: Platform")]
		[SerializeField] private MovingPlatform platform;

		public override void DoInteraction()
		{
			if (platform != null)
				platform.DoSingleMovement();
		}
	}
}
