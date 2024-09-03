using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private bool moveAccordingToCameraView = true;
	[SerializeField] private float speed = 15f;
	[SerializeField] private float rotateSpeed = 10f;
	[SerializeField] private float pushPower = 5f;
	[SerializeField] private Vector3 gravityVector = new Vector3(0f, -9.81f, 0f);

	[Header("Input Actions")]
	[SerializeField] private string moveActionName = "Move";

	[Header("References")]
	[SerializeField] private PlayerInput playerInput;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private Transform visualToRotate;

	[Header("Debug")]
	[SerializeField, ReadOnly] private Vector3 moveVector;
	[SerializeField, ReadOnly] private Vector2 inputVector;

	private InputAction moveAction;

	private void Awake()
	{
		Assert.IsNotNull(playerInput);
		Assert.IsNotNull(characterController);
		Assert.IsNotNull(visualToRotate);

		moveAction = playerInput.actions[moveActionName];
		moveAction.performed += OnMove;
		moveAction.canceled += OnMove;
	}

	private void OnDestroy()
	{
		moveAction.performed -= OnMove;
		moveAction.canceled -= OnMove;
	}

	private void FixedUpdate()
	{
		// move character
		characterController.Move(moveVector * speed * Time.fixedDeltaTime);

		// apply gravity
		characterController.Move(gravityVector * Time.fixedDeltaTime);

		// rotates visual
		if (moveVector.magnitude != 0)
		{
			Quaternion rotation = Quaternion.LookRotation(moveVector);
			visualToRotate.rotation = Quaternion.RotateTowards(visualToRotate.rotation, rotation, rotateSpeed);
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;

		Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.AddForceAtPosition(pushDirection * pushPower, hit.point, ForceMode.Impulse);
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		inputVector = context.ReadValue<Vector2>();

		if (moveAccordingToCameraView)
		{
			// usando os vetores da camera pra mover o player de acordo com a camera, ao inves de usar vetores globais
			// *out* permite criar variaveis locais direto na funçao
			CameraManager.GetCameraVectors(out Vector3 cameraForward, out Vector3 cameraRight);
			moveVector = (inputVector.x * cameraRight) + (inputVector.y * cameraForward);
		}
		else
		{
			moveVector.x = inputVector.x;
			moveVector.z = inputVector.y;
		}
	}
}
