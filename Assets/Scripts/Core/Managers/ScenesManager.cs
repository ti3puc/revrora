using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.Scenes
{
    public class ScenesManager : Singleton<ScenesManager>
    {
        [Header("Scenes")]
        [SerializeField, Scene] private string firstScene;

        public static void LoadFirstScene()
        {
            SceneManager.LoadSceneAsync(Instance.firstScene, LoadSceneMode.Single);
        }

		public static void LoadScene(string sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName);
		}

		public static void ReloadScene()
		{
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}
    }
}