using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Interaction
{
	public class BreakableFixedJoint : Interactable
	{
		[Header("Settings")]
		[SerializeField] private float breakForce = 10;

		[Header("References")]
		[SerializeField] private Rigidbody rigidbodyRef;
		[SerializeField] private FixedJoint fixedJointRef;

		public override void ReceiveInteraction()
		{
			rigidbodyRef.isKinematic = false;
			rigidbodyRef.useGravity = true;
			rigidbodyRef.AddForce(Vector3.down * breakForce, ForceMode.Impulse);

			if (fixedJointRef != null)
				Destroy(fixedJointRef);
		}
	}
}