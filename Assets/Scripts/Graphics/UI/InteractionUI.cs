using Environment.Interaction;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Interaction
{
	public class InteractionUI : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private bool startsHidden = true;

		[Header("References")]
		[SerializeField, ReadOnly] private Canvas canvas;
		[SerializeField, ReadOnly] private RectTransform rectTransform;
		[SerializeField, ReadOnly] private Interactor currentInteractor;

		private void Awake()
		{
			Interactable.OnRequestToShowUI += ShowUI;
			Interactable.OnRequestToHideUI += HideUI;
			ElementInteractable.OnInteractableDestroyed += HideUI;

			canvas = GetComponentInParent<Canvas>();
			rectTransform = GetComponent<RectTransform>();

			if (startsHidden)
				HideUI(null, null);
		}

		private void OnDestroy()
		{
			Interactable.OnRequestToShowUI -= ShowUI;
			Interactable.OnRequestToHideUI -= HideUI;
			ElementInteractable.OnInteractableDestroyed -= HideUI;
		}

		private void FixedUpdate()
		{
			if (currentInteractor != null)
			{
				Vector3 interactorWorldPosition = currentInteractor.transform.position;
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, interactorWorldPosition);

				RectTransformUtility.ScreenPointToLocalPointInRectangle(
					canvas.transform as RectTransform,
					screenPoint,
					canvas.worldCamera,
					out Vector2 canvasLocalPosition);

				rectTransform.anchoredPosition = new Vector2(canvasLocalPosition.x, rectTransform.anchoredPosition.y);
			}
		}

		private void ShowUI(Interactor interactor, Interactable interactable)
		{
			currentInteractor = interactor;

			foreach (Transform child in transform)
				child.gameObject.SetActive(true);
		}

		private void HideUI(Interactor interactor, Interactable interactable) => HideUI();
		private void HideUI()
		{
			foreach (Transform child in transform)
				child.gameObject.SetActive(false);
		}
	}
}