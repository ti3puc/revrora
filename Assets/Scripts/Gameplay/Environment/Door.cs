using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Interaction
{
	using NaughtyAttributes;
	using UnityEngine;

	public class Door : Interactable
	{
		[Header("Settings")]
		[SerializeField] private Transform hingePoint;
		[SerializeField] private float rotationSpeed = 1f;
		[SerializeField] private Vector3 rotateDegrees = new Vector3(0f, 90f, 0);

		[Header("Debug")]
		[SerializeField, ReadOnly] private bool shouldRotate = false;
		[SerializeField, ReadOnly] private bool isOpening = true;
		[SerializeField, ReadOnly] private float rotationProgress = 0f;
		[SerializeField, ReadOnly] private Quaternion startRotation;

		public Quaternion TargetRotation => Quaternion.Euler(rotateDegrees);

		private void Start()
		{
			startRotation = transform.localRotation;
		}

		public override void ReceiveInteraction()
		{
			shouldRotate = true;
			isOpening = !isOpening;
		}

		private void FixedUpdate()
		{
			if (!shouldRotate) return;
			
			rotationProgress += Time.fixedDeltaTime * rotationSpeed;
			if (!isOpening)
			{
				// lerp to 90 degrees
				hingePoint.transform.localRotation = Quaternion.Lerp(startRotation, TargetRotation, rotationProgress);
				if (rotationProgress >= 1f)
				{
					rotationProgress = 1f;
					shouldRotate = false;
				}
			}
			else
			{
				// lerp back to 0 degrees (original rotation)
				hingePoint.transform.localRotation = Quaternion.Lerp(TargetRotation, startRotation, rotationProgress);
				if (rotationProgress >= 1f)
				{
					rotationProgress = 1f;
					shouldRotate = false;
				}
			}

			// reset rotation progress for next interaction
			if (!shouldRotate)
				rotationProgress = 0f;
		}
	}

}
