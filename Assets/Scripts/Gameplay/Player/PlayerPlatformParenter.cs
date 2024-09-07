using NaughtyAttributes;
using UnityEngine;

namespace Player.Movement
{
	public class PlayerPlatformParenter : MonoBehaviour
	{
		[Header("Platform Check")]
		[SerializeField] private float groundCheckDistance = 1;
		[SerializeField] private float groundCheckRadius = .5f;
		[SerializeField] private float platformDistance = 1.5f;
		[SerializeField] private LayerMask groundMask;

		[Header("References")]
		[SerializeField] private Transform ghostToParent;
		[SerializeField] private CharacterController characterController;

		[Header("Debug")]
		[SerializeField, ReadOnly] private bool isOnGround;
		[SerializeField, ReadOnly] private bool isOnPlatform;
		[SerializeField, ReadOnly] private bool hasPlatformBellow;
		[SerializeField, ReadOnly] private Transform currentPlatform;

		public Transform CurrentPlatform => currentPlatform;
		public bool IsOnPlatform => isOnPlatform = isOnGround && currentPlatform != null;
		public bool HasPlatformBellow => hasPlatformBellow;
		public Transform GhostToParent => ghostToParent;

		private void OnDrawGizmos()
		{
			// debug platform check
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckRadius);
			Gizmos.DrawLine(transform.position, transform.position + Vector3.down * platformDistance);
		}

		private void FixedUpdate()
		{
			isOnGround = Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out RaycastHit hitInfo, groundCheckDistance, groundMask);

			Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayInfo, platformDistance);
			hasPlatformBellow = rayInfo.collider != null && rayInfo.collider.CompareTag("Platform");

			if (isOnGround && hitInfo.collider != null && hitInfo.collider.CompareTag("Platform"))
			{
				currentPlatform ??= hitInfo.collider.transform;

				if (ghostToParent.transform.parent != currentPlatform)
					ghostToParent.SetParent(currentPlatform);
			}
			else
			{
				if (ghostToParent.transform.parent != transform)
					ghostToParent.SetParent(transform);

				currentPlatform = null;
			}
		}
	}
}
