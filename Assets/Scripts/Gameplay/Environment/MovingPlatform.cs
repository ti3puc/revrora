using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MovingPlatform : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float duration = 1f; // in seconds
	[SerializeField] private float delayBeforeMovement = 0f; // in seconds
	[SerializeField] private bool infiniteMovement = true;

	[Header("References")]
	[SerializeField] private Transform startPoint;
	[SerializeField] private Transform endPoint;

	[Header("Debug")]
	[SerializeField] private float gizmosSphereRadius = .2f;
	[SerializeField, ReadOnly] private float lerpFactor = 0;
	[SerializeField, ReadOnly] private float delayTimer = 0f;
	[SerializeField, ReadOnly] private bool isWaitingDelay = false;
	[SerializeField, ReadOnly] private bool isMovingToEnd = true;
	[SerializeField, ReadOnly] private bool hasStartedMovement = false;

	private void Awake()
	{
		Assert.IsNotNull(startPoint);
		Assert.IsNotNull(endPoint);
	}

	private void Start()
	{
		if (infiniteMovement)
			DoSingleMovement();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		if (startPoint != null)
			Gizmos.DrawSphere(startPoint.position, gizmosSphereRadius);
		if (endPoint != null)
			Gizmos.DrawSphere(endPoint.position, gizmosSphereRadius);
	}

	private void FixedUpdate()
	{
		if (isWaitingDelay)
		{
			delayTimer += Time.fixedDeltaTime;
			if (delayTimer > delayBeforeMovement)
				isWaitingDelay = false;

			return;
		}

		if (hasStartedMovement)
		{
			lerpFactor += duration * Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(startPoint.position, endPoint.position, lerpFactor);

			if (!isMovingToEnd)
				lerpFactor -= 2 * duration * Time.fixedDeltaTime; // logic to go back

			// reached end of movement
			if (lerpFactor >= 1f || lerpFactor <= 0f)
			{
				lerpFactor = Mathf.Clamp01(lerpFactor);
				isMovingToEnd = !isMovingToEnd;
				hasStartedMovement = false;

				if (delayBeforeMovement > 0)
				{
					delayTimer = 0;
					isWaitingDelay = true;
				}

				if (infiniteMovement)
					DoSingleMovement();
			}
		}
	}

	[Button(enabledMode: EButtonEnableMode.Playmode)]
	public void DoSingleMovement()
	{
		if (lerpFactor >= 1f || lerpFactor <= 0f)
		{
			if (isMovingToEnd)
				lerpFactor = 0f;
			else
				lerpFactor = 1f;
		}
		else
			isMovingToEnd = !isMovingToEnd;

		hasStartedMovement = true;
	}
}
