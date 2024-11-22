using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Creatures;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public delegate void PauseEvent(bool isPaused);
		public static event PauseEvent OnPauseStateChanged;

		[Header("Debug")]
		[SerializeField, ReadOnly] private List<CharacterDefinition> _charactersDefinitions;
		[SerializeField, ReadOnly] private bool _isGamePaused;

		public static List<CharacterDefinition> Characters => Instance._charactersDefinitions;
		public static bool IsGamePaused => Instance._isGamePaused;

		protected override void Awake()
		{
			base.Awake();
			Resume();
		}

		public static void Pause()
		{
			Time.timeScale = 0f;
			Instance._isGamePaused = true;
			OnPauseStateChanged?.Invoke(true);
		}

		public static void Resume()
		{
			Time.timeScale = 1f;
			Instance._isGamePaused = false;
			OnPauseStateChanged?.Invoke(false);
		}

#if UNITY_EDITOR
		[Button]
		public void GetAllCharacterDefinitions()
		{
			_charactersDefinitions = new List<CharacterDefinition>();

			var guids = UnityEditor.AssetDatabase.FindAssets("t:CharacterDefinition");

			foreach (var guid in guids)
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				var characterDefinition = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterDefinition>(path);

				_charactersDefinitions.Add(characterDefinition);
			}
		}
#endif
	}
}
