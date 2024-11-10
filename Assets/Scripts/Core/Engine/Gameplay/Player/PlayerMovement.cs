using System;
using Managers.Cameras;
using Managers.Scenes;
using NaughtyAttributes;
using Player.Input;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player.Movement
{
	public class PlayerMovement : MonoBehaviour
	{
		#region Fields

		[Header("Movement")]
		[SerializeField] private bool moveAccordingToCameraView = true;
		[SerializeField] private float speed = 15f;
		[SerializeField] private float acceleration = 5f;
		[SerializeField] private float deceleration = 5f;
		[SerializeField] private bool movementStartsDisabled = false;

		[Header("Air Movement")]
		[SerializeField] private float speedOnAir = 8f;
		[SerializeField] private float accelerationOnAir = 5f;

		[Header("Rotation")]
		[SerializeField] private float rotateSpeed = 10f;

		[Header("Physics Interaction")]
		[SerializeField] private float pushPower = 5f;

		[Header("Gravity")]
		[SerializeField] private Vector3 gravityVector = new Vector3(0f, -9.81f, 0f);
		[SerializeField] private float fallAcceleration = -50f;
		[SerializeField] private float maxFallSpeed = -20f;

		[Header("Ground Check")]
		[SerializeField] private float groundCheckDistance = 1;
		[SerializeField] private float groundCheckRadius = .5f;
		[SerializeField] private LayerMask groundMask;

		[Header("Input Actions")]
		[SerializeField] private string moveActionName = "Move";

		[Header("References")]
		[SerializeField] private CharacterController characterController;
		[SerializeField] private Transform visualToRotate;
		[SerializeField] private PlayerPlatformParenter platformParenter;

		[Header("Debug")]
		[SerializeField, ReadOnly] private Vector3 moveVector;
		[SerializeField, ReadOnly] private Vector3 lastMoveDirection;
		[SerializeField, ReadOnly] private Vector2 inputVector;
		[SerializeField, ReadOnly] private float currentSpeed;
		[SerializeField, ReadOnly] private bool isGrounded;
		[SerializeField, ReadOnly] private float fallSpeed;
		[SerializeField, ReadOnly] private bool isMovementDisabled;

		#endregion

		private void Awake()
		{
			Assert.IsNotNull(characterController);
			Assert.IsNotNull(visualToRotate);
			Assert.IsNotNull(platformParenter);

			PlayerInput.OnMovePerformed += OnMove;
			PlayerInput.OnMoveCanceled += OnMove;
			ScenesManager.OnSceneStartedLoading += DisableMovementForSceneLoad;

			if (movementStartsDisabled)
			{
				Debug.LogWarning("Player movement was disabled from awake.");
				DisableMovement();
			}
		}

		private void OnDestroy()
		{
			PlayerInput.OnMovePerformed -= OnMove;
			PlayerInput.OnMoveCanceled -= OnMove;
			ScenesManager.OnSceneStartedLoading -= DisableMovementForSceneLoad;
		}

        private void OnDrawGizmos()
		{
			// debug ground check
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckRadius);
		}

		private void FixedUpdate()
		{
			if (isMovementDisabled) return;
			isGrounded = Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckRadius, groundMask) || platformParenter.HasPlatformBellow;

			// move character, and changes speed if on air or grounded
			float targetSpeed = isGrounded ? speed : speedOnAir;
			float targetAcceleration = isGrounded ? acceleration : accelerationOnAir;

			if (moveVector.magnitude > 0)
			{
				lastMoveDirection = moveVector.normalized;
				currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * targetAcceleration);
			}
			else
			{
				// avoid infinite lerping since true 0 never happens
				if (currentSpeed < 0.01f)
					currentSpeed = 0f;
				else
					currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.fixedDeltaTime * deceleration);
			}

			Vector3 moveDirection = lastMoveDirection * currentSpeed * Time.fixedDeltaTime;

			// adjust movement based on platform offset
			if (platformParenter.IsOnPlatform)
			{
				Vector3 platformMovement = platformParenter.GhostToParent.position - transform.position + moveDirection;
				characterController.Move(platformMovement);
				platformParenter.GhostToParent.position = transform.position;
			}
			else
				characterController.Move(moveDirection);

			// apply gravity with acceleration
			if (!isGrounded)
			{
				fallSpeed += fallAcceleration * Time.fixedDeltaTime;
				fallSpeed = Mathf.Clamp(fallSpeed, maxFallSpeed, 0f);
			}
			else
				fallSpeed = 0f;

			if (fallSpeed < 0)
			{
				Vector3 fallVector = new Vector3(0f, fallSpeed, 0f);
				characterController.Move(fallVector * Time.fixedDeltaTime);
			}
			else
				characterController.Move(gravityVector * Time.fixedDeltaTime);

			// rotates visual
			if (moveVector.magnitude != 0)
			{
				Quaternion rotation = Quaternion.LookRotation(moveVector);
				visualToRotate.rotation = Quaternion.RotateTowards(visualToRotate.rotation, rotation, rotateSpeed);
			}
		}

		// simulates physics collision
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody body = hit.collider.attachedRigidbody;
			if (body == null || body.isKinematic) return;

			Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
			body.AddForceAtPosition(pushDirection * pushPower, hit.point, ForceMode.Impulse);
		}

		// get input vectors
		private void OnMove(Vector2 moveData)
		{
			inputVector = moveData;

			if (moveAccordingToCameraView)
			{
				// usando os vetores da camera pra mover o player de acordo com a camera, ao inves de usar vetores globais
				// *out* permite criar variaveis locais direto na fun�ao
				CameraManager.GetCameraVectors(out Vector3 cameraForward, out Vector3 cameraRight);
				moveVector = (inputVector.x * cameraRight) + (inputVector.y * cameraForward);
			}
			else
			{
				moveVector.x = inputVector.x;
				moveVector.z = inputVector.y;
			}
		}

		public void DisableMovement()
		{
			isMovementDisabled = true;
			characterController.enabled = false;
		}

		public void EnableMovement()
		{
			characterController.enabled = true;
			isMovementDisabled = false;
		}

        private void DisableMovementForSceneLoad(string lastScene, string sceneName)
        {
			DisableMovement();
        }
	}
}
