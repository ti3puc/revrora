using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformParenter : MonoBehaviour
{
	[Header("Platform Check")]
	[SerializeField] private float groundCheckDistance = 1;
	[SerializeField] private float groundCheckRadius = .5f;
	[SerializeField] private LayerMask groundMask;

	[Header("References")]
	[SerializeField] private Transform ghostToParent;
	[SerializeField] private CharacterController characterController;

	[Header("Debug")]
	[SerializeField, ReadOnly] private bool isOnGround;
	[SerializeField, ReadOnly] private bool isOnPlatform;
	[SerializeField, ReadOnly] private Transform currentPlatform;

	public Transform CurrentPlatform => currentPlatform;
	public bool IsOnPlatform => isOnPlatform = isOnGround && currentPlatform != null;
	public Transform GhostToParent => ghostToParent;

	private void OnDrawGizmos()
	{
		// debug platform check
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckRadius);
	}

	private void FixedUpdate()
	{
		isOnGround = Physics.SphereCast(transform.position, groundCheckDistance, Vector3.down, out RaycastHit hitInfo, groundCheckRadius, groundMask);

		if (isOnGround && hitInfo.collider.CompareTag("Platform"))
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
