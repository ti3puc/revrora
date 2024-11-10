using System.Collections.Generic;
using Managers.Audio;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
	public class PlayClickSound : MonoBehaviour
	{
		[SerializeField] private string _clickSoundName = "ui_click";
		[SerializeField, Min(1)] private int _trackNumber = 3;

		[Header("Exclude Buttons")]
		[SerializeField] private List<Button> _excludeButtons = new List<Button>();

		[Header("Debug")]
		[SerializeField, ReadOnly] private List<Button> _buttons = new List<Button>();

		private void Awake()
		{
			_buttons.Clear();

			_buttons.AddRange(GetComponents<Button>());
			_buttons.AddRange(GetComponentsInChildren<Button>(true));

			foreach (var button in _buttons)
			{
				if (_excludeButtons.Contains(button)) continue;
				button.onClick.AddListener(PlaySound);
			}
		}

		private void OnDestroy()
		{
			foreach (var button in _buttons)
			{
				if (_excludeButtons.Contains(button)) continue;
				button.onClick.RemoveListener(PlaySound);
			}
		}

		[Button(enabledMode: EButtonEnableMode.Playmode)]
		public void PlaySound()
		{
			AudioManager.Instance.PlaySoundOneShot(_clickSoundName, _trackNumber);
		}
	}
}
