using System.Collections;
using System.Collections.Generic;
using Character.Base;
using Creatures;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
        [SerializeField] private CreatureHashtable _creaturesList;

		public static List<BaseCharacter> Creatures => Instance._creaturesList.Creatures;
	}
}
