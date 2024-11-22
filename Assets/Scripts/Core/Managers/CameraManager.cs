using Cinemachine;
using Managers.Settings;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Cameras
{
	public class CameraManager : Singleton<CameraManager>
	{
		[Header("Vcams")]
		[SerializeField] private float _closeDistance = 10f;
		[SerializeField] private float _farDistance = 15f;

		[Header("Debug")]
		[SerializeField, ReadOnly] private Camera mainCamera;
		[SerializeField, ReadOnly] private CinemachineVirtualCamera _playerCameraFollow;

		public static Camera MainCamera => Instance.mainCamera;

		protected override void Awake()
		{
			base.Awake();
			mainCamera = Camera.main;
			var findCamera = FindObjectOfType<CinemachineVirtualCamera>();
			_playerCameraFollow = findCamera.name == "Camera Player Follow" ? findCamera : null;

			UpdateCameraDistance();
			SettingsManager.OnSettingsChanged += UpdateCameraDistance;
		}

		private void OnDestroy()
		{
			SettingsManager.OnSettingsChanged -= UpdateCameraDistance;
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

		private void UpdateCameraDistance()
		{
			if (_playerCameraFollow == null)
			{
				Debug.LogError("Could not find player camera follow vcam");
				return;
			}

			_playerCameraFollow.m_Lens.OrthographicSize = SettingsManager.Instance.CameraDistance == 0 ? _farDistance : _closeDistance;
		}
	}
}