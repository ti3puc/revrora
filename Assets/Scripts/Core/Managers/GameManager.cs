using System;
using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Creatures;
using Inventory.Items;
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

		[Serializable]
		public struct NamePerScene
		{
			public string Name;
			[Scene] public string Scene;
		}

		[Header("Scene Names")]
		[SerializeField] private List<NamePerScene> _namesPerScene = new List<NamePerScene>();

		[Header("Debug")]
		[SerializeField, ReadOnly] private List<CharacterDefinition> _charactersDefinitions;
		[SerializeField, ReadOnly] private List<ItemData> _itemsData;
		[SerializeField, ReadOnly] private bool _isGamePaused;

		public static List<CharacterDefinition> Characters => Instance._charactersDefinitions;
		public static List<ItemData> Items => Instance._itemsData;
		public static List<NamePerScene> NamesPerScene => Instance._namesPerScene;
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

			_charactersDefinitions.Sort((a, b) => a.Id.CompareTo(b.Id));
			CheckDuplicatedIds();
		}

		// [Button]
		// public void FixIdsBasedOnListOrder()
		// {
		// 	for (var i = 0; i < _charactersDefinitions.Count; i++)
		// 		_charactersDefinitions[i].Id = i;
		// }

		[Button]
		public void CheckDuplicatedIds()
		{
			var idCounts = new Dictionary<int, List<string>>();

			foreach (var characterDefinition in _charactersDefinitions)
			{
				if (!idCounts.ContainsKey(characterDefinition.Id))
					idCounts[characterDefinition.Id] = new List<string>();

				idCounts[characterDefinition.Id].Add(characterDefinition.name);
			}

			foreach (var kvp in idCounts)
			{
				if (kvp.Value.Count > 1)
					Debug.LogError($"Duplicated ID: {kvp.Key} found in the following objects: {string.Join(", ", kvp.Value)}");
			}
		}

		[Button]
		public void GetAllItems()
		{
			_itemsData = new List<ItemData>();

			var guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData");

			foreach (var guid in guids)
			{
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				var itemData = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(path);

				_itemsData.Add(itemData);
			}
		}
#endif
	}
}
