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
		[Header("Debug")]
        [SerializeField, ReadOnly] private List<CharacterDefinition> _charactersDefinitions;

		public static List<CharacterDefinition> Characters => Instance._charactersDefinitions;

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
