using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Cameras
{
	public class CameraManager : Singleton<CameraManager>
	{
		[Header("Debug")]
		[SerializeField, ReadOnly] private Camera mainCamera;

		public static Camera MainCamera => Instance.mainCamera;

		protected override void Awake()
		{
			base.Awake();
			mainCamera = Camera.main;
		}

		public static void GetCameraVectors(out Vector3 camForwardVector, out Vector3 camRightVector)
		{
			camForwardVector = MainCamera.transform.forward;
			camForwardVector.y = 0f;
			camForwardVector.Normalize();

			camRightVector = MainCamera.transform.right;
			camRightVector.y = 0f;
			camRightVector.Normalize();
		}
	}
}