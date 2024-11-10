using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.Settings
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        [Header("References")]
        [SerializeField] private AudioMixer _audioMixer;

        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SfxVolume";

        public int MusicVolume
        {
            get => PlayerPrefs.GetInt(MusicVolumeKey, 50);
            set
            {
                PlayerPrefs.SetInt(MusicVolumeKey, value);
                SetMusicVolume(value);
            }
        }

        public int SfxVolume
        {
            get => PlayerPrefs.GetInt(SfxVolumeKey, 50);
            set
            {
                PlayerPrefs.SetInt(SfxVolumeKey, value);
                SetSfxVolume(value);
            }
        }

        private void Start()
        {
            SetMusicVolume(MusicVolume);
            SetSfxVolume(SfxVolume);
        }

        private void SetMusicVolume(int volume)
        {
            if (volume == 0)
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
            if (volume == 0)
            {
                _audioMixer.SetFloat("SfxVolume", -80f);
            }
            else
            {
                float dbVolume = Mathf.Log10(volume / 100f) * 20;
                _audioMixer.SetFloat("SfxVolume", dbVolume);
            }
        }
    }
}
