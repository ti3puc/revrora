using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
	public class PlayerInput : Singleton<PlayerInput>
	{
		public delegate void Vector2Event(Vector2 value);
		public static event Vector2Event OnMoveStarted;
		public static event Vector2Event OnMovePerformed;
		public static event Vector2Event OnMoveCanceled;

		public static event Action OnInteractionStarted;
		public static event Action OnInteractionCanceled;
		public static event Action OnToggleInventoryStarted;
		public static event Action OnPauseStarted;

		public static event Action OnRunStarted;
		public static event Action OnRunPressed;
		public static event Action OnRunCanceled;

		[Header("Inputs")]
		[SerializeField] private InputActionAsset inputActionAsset;
		[SerializeField] private string actionMapName = "Main";
		[SerializeField] private string moveActionName = "Move";
		[SerializeField] private string interactActionName = "Interact";
		[SerializeField] private string toggleInventoryActionName = "ToggleInventory";
		[SerializeField] private string pauseActionName = "Pause";
		[SerializeField] private string runActionName = "Run";

		private InputActionMap actionMap;
		private InputAction moveAction;
		private InputAction interactAction;
		private InputAction toggleInventoryAction;
		private InputAction pauseAction;
		private InputAction runAction;

		protected override void Awake()
		{
			base.Awake();

			inputActionAsset.Enable();

			actionMap = inputActionAsset.FindActionMap(actionMapName);
			moveAction = actionMap.FindAction(moveActionName);
			interactAction = actionMap.FindAction(interactActionName);
			toggleInventoryAction = actionMap.FindAction(toggleInventoryActionName);
			pauseAction = actionMap.FindAction(pauseActionName);
			runAction = actionMap.FindAction(runActionName);

			moveAction.started += MoveStarted;
			moveAction.performed += MovePerformed;
			moveAction.canceled += MoveCanceled;

			interactAction.started += InteractionStarted;
			interactAction.canceled += InteractionCanceled;

			toggleInventoryAction.started += ToggleInventoryStarted;

			pauseAction.started += PauseStarted;

			runAction.started += RunStarted;
			runAction.canceled += RunCanceled;
		}

		private void OnDestroy()
		{
			moveAction.started -= MoveStarted;
			moveAction.performed -= MovePerformed;
			moveAction.canceled -= MoveCanceled;

			interactAction.started -= InteractionStarted;
			interactAction.canceled -= InteractionCanceled;

			toggleInventoryAction.started -= ToggleInventoryStarted;

			pauseAction.started -= PauseStarted;

			runAction.started -= RunStarted;
			runAction.canceled -= RunCanceled;

			inputActionAsset.Disable();
		}

		private void FixedUpdate()
		{
			if (runAction.IsPressed())
				OnRunPressed?.Invoke();
		}

		private void MoveStarted(InputAction.CallbackContext context) => OnMoveStarted?.Invoke(context.ReadValue<Vector2>());
		private void MovePerformed(InputAction.CallbackContext context) => OnMovePerformed?.Invoke(context.ReadValue<Vector2>());
		private void MoveCanceled(InputAction.CallbackContext context) => OnMoveCanceled?.Invoke(context.ReadValue<Vector2>());

		private void InteractionStarted(InputAction.CallbackContext context) => OnInteractionStarted?.Invoke();
		private void InteractionCanceled(InputAction.CallbackContext context) => OnInteractionCanceled?.Invoke();

		private void ToggleInventoryStarted(InputAction.CallbackContext context) => OnToggleInventoryStarted?.Invoke();

		private void PauseStarted(InputAction.CallbackContext context) => OnPauseStarted?.Invoke();

		private void RunStarted(InputAction.CallbackContext context) => OnRunStarted?.Invoke();
		private void RunCanceled(InputAction.CallbackContext context) => OnRunCanceled?.Invoke();
	}
}
