using System;
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
		public static event Action OnInteractionPerformed;
		public static event Action OnInteractionCanceled;

		[Header("Inputs")]
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private string actionMapName = "Main";
        [SerializeField] private string moveActionName = "Move";
		[SerializeField] private string interactActionName = "Interact";

		private InputActionMap actionMap;
		private InputAction moveAction;
        private InputAction interactAction;

		protected override void Awake()
		{
			base.Awake();

            inputActionAsset.Enable();
			actionMap = inputActionAsset.FindActionMap(actionMapName);
			moveAction = actionMap.FindAction(moveActionName);
			interactAction = actionMap.FindAction(interactActionName);

			moveAction.started += MoveStarted;
			moveAction.performed += MovePerformed;
			moveAction.canceled += MoveCanceled;

			interactAction.started += InteractionStarted;
			interactAction.performed += InteractionPerformed;
			interactAction.canceled += InteractionCanceled;
		}

		private void OnDestroy()
		{
			moveAction.started -= MoveStarted;
			moveAction.performed -= MovePerformed;
			moveAction.canceled -= MoveCanceled;

			interactAction.started -= InteractionStarted;
			interactAction.performed -= InteractionPerformed;
			interactAction.canceled -= InteractionCanceled;

			inputActionAsset.Disable();
		}

		private void MoveStarted(InputAction.CallbackContext context) => OnMoveStarted?.Invoke(context.ReadValue<Vector2>());
		private void MovePerformed(InputAction.CallbackContext context) => OnMovePerformed?.Invoke(context.ReadValue<Vector2>());
		private void MoveCanceled(InputAction.CallbackContext context) => OnMoveCanceled?.Invoke(context.ReadValue<Vector2>());

		private void InteractionStarted(InputAction.CallbackContext context) => OnInteractionStarted?.Invoke();
		private void InteractionPerformed(InputAction.CallbackContext context) => OnInteractionPerformed?.Invoke();
		private void InteractionCanceled(InputAction.CallbackContext context) => OnInteractionCanceled?.Invoke();
	}
}
