using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public void ReloadScene()
		{
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
