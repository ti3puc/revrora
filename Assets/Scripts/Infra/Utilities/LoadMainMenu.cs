using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Menu
{
    public class LoadMainMenu : MonoBehaviour
    {
        [SerializeField, Scene] private string mainMenuSceneName;

        private void Start()
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
