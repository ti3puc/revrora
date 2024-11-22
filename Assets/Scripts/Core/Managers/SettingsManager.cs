using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace Managers.Settings
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        public static event Action OnSettingsChanged;

        #region Fields

        [Header("References")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private UniversalRendererData _lowRenderer;
        [SerializeField] private UniversalRendererData _mediumRenderer;
        [SerializeField] private UniversalRendererData _highRenderer;
        [SerializeField] private PostProcessData _postProcessData;

        private const string AutosaveKey = "Autosave";
        private const string TextSpeedKey = "TextSpeed";
        private const string CameraDistanceKey = "CameraDistance";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SfxVolume";
        private const string MusicKey = "Music";
        private const string SfxKey = "Sfx";
        private const string GraphicsKey = "Graphics";
        private const string FullscreenKey = "Fullscreen";
        private const string FramerateKey = "Framerate";
        private const string PostProcessKey = "PostProcess";
        private const string VSyncKey = "VSync";

        #endregion

        #region Properties

        public int Music
        {
            get => PlayerPrefs.GetInt(MusicKey, 0);
            set
            {
                PlayerPrefs.SetInt(MusicKey, value);
                SetMusicVolume(MusicVolume);
                OnSettingsChanged?.Invoke();
            }
        }

        public int Sfx
        {
            get => PlayerPrefs.GetInt(SfxKey, 0);
            set
            {
                PlayerPrefs.SetInt(SfxKey, value);
                SetSfxVolume(SfxVolume);
                OnSettingsChanged?.Invoke();
            }
        }

        public int MusicVolume
        {
            get => PlayerPrefs.GetInt(MusicVolumeKey, 50);
            set
            {
                PlayerPrefs.SetInt(MusicVolumeKey, value);
                SetMusicVolume(value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int SfxVolume
        {
            get => PlayerPrefs.GetInt(SfxVolumeKey, 50);
            set
            {
                PlayerPrefs.SetInt(SfxVolumeKey, value);
                SetSfxVolume(value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int Graphics
        {
            get => PlayerPrefs.GetInt(GraphicsKey, 2);
            set
            {
                PlayerPrefs.SetInt(GraphicsKey, value);
                SetGraphics(value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int Fullscreen
        {
            get => PlayerPrefs.GetInt(FullscreenKey, 0);
            set
            {
                PlayerPrefs.SetInt(FullscreenKey, value);
                SetFullscreen(value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int Framerate
        {
            get => PlayerPrefs.GetInt(FramerateKey, 1);
            set
            {
                PlayerPrefs.SetInt(FramerateKey, value);
                SetFramerate(value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int PostProcess
        {
            get => PlayerPrefs.GetInt(PostProcessKey, 0);
            set
            {
                PlayerPrefs.SetInt(PostProcessKey, value);
                SetPostProcess(value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int VSync
        {
            get => PlayerPrefs.GetInt(VSyncKey, 0);
            set
            {
                PlayerPrefs.SetInt(VSyncKey, value);
                SetVsync(value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int Autosave
        {
            get => PlayerPrefs.GetInt(AutosaveKey, 0);
            set
            {
                PlayerPrefs.SetInt(AutosaveKey, value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int TextSpeed
        {
            get => PlayerPrefs.GetInt(TextSpeedKey, 1);
            set
            {
                PlayerPrefs.SetInt(TextSpeedKey, value);
                OnSettingsChanged?.Invoke();
            }
        }

        public int CameraDistance
        {
            get => PlayerPrefs.GetInt(CameraDistanceKey, 0);
            set
            {
                PlayerPrefs.SetInt(CameraDistanceKey, value);
                OnSettingsChanged?.Invoke();
            }
        }

        #endregion

        #region Unity Messages

        private void Start()
        {
            SetMusicVolume(MusicVolume);
            SetSfxVolume(SfxVolume);

            SetGraphics(Graphics);
            SetPostProcess(PostProcess);
            SetFullscreen(Fullscreen);
            SetFramerate(Framerate);
            SetVsync(VSync);
        }

        #endregion

        #region Private Methods

        private void SetMusicVolume(int volume)
        {
            if (Music == 1)
            {
                _audioMixer.SetFloat("MusicVolume", -80f);
            }
            else
            {
                float dbVolume = Mathf.Log10(volume / 100f) * 20;
                _audioMixer.SetFloat("MusicVolume", dbVolume);
            }
        }

        private void SetSfxVolume(int volume)
        {
            if (Sfx == 1)
            {
                _audioMixer.SetFloat("SfxVolume", -80f);
            }
            else
            {
                float dbVolume = Mathf.Log10(volume / 100f) * 20;
                _audioMixer.SetFloat("SfxVolume", dbVolume);
            }
        }

        private void SetGraphics(int quality)
        {
            QualitySettings.SetQualityLevel(quality);
        }

        private void SetFullscreen(int value)
        {
            if (value == 0)
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.fullScreen = false;
            }
        }

        private void SetFramerate(int value)
        {
            Application.targetFrameRate = value switch
            {
                0 => 30,
                1 => 60,
                2 => 0,
                _ => 60
            };
        }

        private void SetPostProcess(int value)
        {
            bool hasPostprocess = value == 0;
            if (hasPostprocess)
            {
                _lowRenderer.postProcessData = _postProcessData;
                _mediumRenderer.postProcessData = _postProcessData;
                _highRenderer.postProcessData = _postProcessData;
            }
            else
            {
                _lowRenderer.postProcessData = null;
                _mediumRenderer.postProcessData = null;
                _highRenderer.postProcessData = null;
            }
        }

        private void SetVsync(int value)
        {
            QualitySettings.vSyncCount = value;
        }

        #endregion
    }
}
