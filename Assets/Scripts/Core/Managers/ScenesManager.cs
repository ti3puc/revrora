using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using NaughtyAttributes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.Scenes
{
    public class ScenesManager : Singleton<ScenesManager>
    {
        public delegate void SceneEvent(string lastSceneName, string sceneName);
        public static event SceneEvent OnSceneStartedLoading;
        public static event Action OnAnySceneLoading;

        [Header("Settings")]
        [SerializeField] private float _sceneTransitionTime = 1f;
        [SerializeField, Scene] private string firstScene;

        [Header("Debug")]
        [SerializeField, ReadOnly] private string _lastScene;

        public static string LastScene => Instance._lastScene;
        public static string CurrentScene => SceneManager.GetActiveScene().name;

        #region Load Scene
        public static void LoadFirstScene()
        {
            LoadScene(Instance.firstScene);
        }

        public static void LoadLastScene()
        {
            LoadScene(Instance._lastScene);
        }

        public static void ReloadScene()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LoadScene(int buildIndex)
        {
            string sceneName = SceneManager.GetSceneByBuildIndex(buildIndex).name;
            LoadScene(sceneName);
        }

        public static void LoadScene(string sceneName)
        {
            Instance._lastScene = SceneManager.GetActiveScene().name;
            OnSceneStartedLoading?.Invoke(Instance._lastScene, sceneName);
            OnAnySceneLoading?.Invoke();

            if (sceneName.Contains("Combat"))
            {
                var combatTransition = FindObjectOfType<CombatTransition>();
                if (combatTransition != null)
                {
                    combatTransition.StartCombatTransition(() => SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single));
                    return;
                }
            }

            // load after Transition
            FadeIn(() => SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single));
        }

        #endregion

        #region Transition

        public static void FadeIn(Action transitionCallback = null)
        {
            Instance.StartCoroutine(Instance.FadeInCoroutine(transitionCallback));
        }

        public static void FadeOut(Action transitionCallback = null)
        {
            Instance.StartCoroutine(Instance.FadeOutCoroutine(transitionCallback));
        }

        private IEnumerator FadeInCoroutine(Action transitionCallback)
        {
            float elapsedTime = 0f;
            while (elapsedTime < _sceneTransitionTime)
            {
                elapsedTime += Time.deltaTime;

                if (CanvasManager.InstanceIsValid)
                    CanvasManager.TransitionCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / _sceneTransitionTime);

                yield return null;
            }

            transitionCallback?.Invoke();
        }

        private IEnumerator FadeOutCoroutine(Action transitionCallback)
        {
            float elapsedTime = 0f;
            while (elapsedTime < _sceneTransitionTime)
            {
                elapsedTime += Time.deltaTime;

                if (CanvasManager.InstanceIsValid)
                    CanvasManager.TransitionCanvasGroup.alpha = Mathf.Clamp01(1 - elapsedTime / _sceneTransitionTime);

                yield return null;
            }

            transitionCallback?.Invoke();
        }

        #endregion
    }
}