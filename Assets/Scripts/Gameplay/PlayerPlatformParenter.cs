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
	[SerializeField] private CharacterController characterController;

	[Header("Debug")]
	[SerializeField, ReadOnly] private bool isOnGround;
	[SerializeField, ReadOnly] private bool isOnPlatform;
	[SerializeField, ReadOnly] private Transform currentPlatform;
	[SerializeField, ReadOnly] private Vector3 platformOffset;

	public Transform CurrentPlatform => currentPlatform;
	public bool IsOnPlatform => isOnPlatform = isOnGround && currentPlatform != null;
	public Vector3 PlatformOffset
	{
		get => platformOffset;
		set => platformOffset = value;
	}

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
			platformOffset = currentPlatform.position - transform.position;
		}
		else
			currentPlatform = null;
	}
}
