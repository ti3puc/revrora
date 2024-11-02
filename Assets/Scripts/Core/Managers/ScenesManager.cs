using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.Scenes
{
    public class ScenesManager : Singleton<ScenesManager>
    {
        [Header("Settings")]
        [SerializeField] private float _sceneTransitionTime = 1f;
        [SerializeField, Scene] private string firstScene;

        #region Load Scene

        public static void LoadFirstScene()
        {
            LoadScene(Instance.firstScene);
        }

        public static void ReloadScene()
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void LoadScene(string sceneName)
        {
            // load after Transition
            FadeIn(() => SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single));
        }

        public static void LoadScene(int buildIndex)
        {
            // load after Transition
            FadeIn(() => SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single));
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