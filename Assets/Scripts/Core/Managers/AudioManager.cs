using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

namespace Managers.Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		public interface ISound
		{
			public string Id { get; }
			public AudioClip AudioClip { get; }
		}

		[System.Serializable]
		public class Sound : ISound
		{
			[SerializeField] private string _id;
			[SerializeField] private AudioClip _audioClip;

			public string Id => _id;
			public AudioClip AudioClip => _audioClip;
		}

		[InfoBox("Track 0 is for Music. Track 1 is for Ambience. Tracks 2 and above are for Sound Effects.")]
		[Header("Audios")]
		[SerializeField] private Sound[] _musics;
		[SerializeField] private Sound[] _soundEffects;
		[Header("Settings")]
		[SerializeField, Tooltip("Duration in seconds of the audio fade ins and outs.")] private float _fadeTime = 1f;
		[Header("Debug")]
		[SerializeField, ReadOnly] private AudioSource[] _tracks;

		public AudioSource[] Tracks => _tracks;
		#region Unity Messages
		protected override void Awake()
		{
			base.Awake();
			_tracks = GetComponents<AudioSource>();
		}
		#endregion

		#region Public Methods
		public void PauseTrack(int trackNumber)
		{
			AudioSource audioSource = GetTrack(trackNumber);

			if (audioSource != null)
				audioSource.Pause();
		}

		public void ResumeTrack(int trackNumber)
		{
			AudioSource audioSource = GetTrack(trackNumber);

			if (audioSource != null)
				audioSource.UnPause();
		}

		public void StopTrack(int trackNumber)
		{
			AudioSource audioSource = GetTrack(trackNumber);

			if (audioSource != null)
				audioSource.Stop();
		}

		public void PauseAllTracks()
		{
			foreach (AudioSource track in FindObjectsOfType<AudioSource>())
				PauseTrack(track);
		}

		public void ResumeAllTracks()
		{
			foreach (AudioSource track in FindObjectsOfType<AudioSource>())
				ResumeTrack(track);
		}

		public void StopAllTracks()
		{
			foreach (AudioSource track in FindObjectsOfType<AudioSource>())
				StopTrack(track);
		}

		public bool IsTrackPlayingSound(string soundName, int trackNumber)
		{
			AudioSource audioSource = GetTrack(trackNumber);
			if (audioSource.isPlaying == false)
				return false;

			return audioSource.clip.name == GetAudioClip(soundName).name;
		}

		public void PlaySound(string soundName, int trackNumber)
		{
			AudioSource audioSource = GetTrack(trackNumber);
			audioSource.clip = GetAudioClip(soundName);
			audioSource.loop = true;

			if (!audioSource.isPlaying)
				audioSource.Play();
		}

		public void PlaySoundOneShot(string soundName, int trackNumber)
		{
			AudioSource audioSource = GetTrack(trackNumber);
			if (audioSource != null)
				audioSource.PlayOneShot(GetAudioClip(soundName));
		}

		public IEnumerator WaitPlaySoundOneShot(string soundName, int trackNumber, Action action = null)
		{
			AudioSource audioSource = GetTrack(trackNumber);
			if (audioSource != null)
				audioSource.PlayOneShot(GetAudioClip(soundName));

			yield return new WaitWhile(() => audioSource.isPlaying);
			action();
		}

		public float GetTrackVolume(int trackNumber)
		{
			return GetTrack(trackNumber).volume;
		}

		public void ChangeTrackVolume(int trackNumber, float volume)
		{
			AudioSource audioSource = GetTrack(trackNumber);
			if (audioSource != null)
				audioSource.volume = volume;
		}

		public void ChangeSoundWithFade(string soundName, int trackNumber)
		{
			StartCoroutine(StartChangeSoundWithFade(soundName, trackNumber, GetTrackVolume(trackNumber)));
		}
		#endregion

		#region Private Methods
		private AudioSource GetTrack(int trackNumber)
		{
			if (trackNumber == 0)
			{
				Debug.LogError("Tracks start at number 1.");
			}

			return _tracks[trackNumber - 1];
		}

		private AudioClip GetAudioClip(string soundName)
		{
			foreach (ISound sound in _soundEffects)
			{
				if (sound.Id == soundName)
					return sound.AudioClip;
			}

			foreach (ISound music in _musics)
			{
				if (music.Id == soundName)
					return music.AudioClip;
			}

			Debug.LogError("Audio " + soundName + " not found.");
			return null;
		}

		private void PauseTrack(AudioSource track) => track.Pause();

		private void ResumeTrack(AudioSource track) => track.UnPause();

		private void StopTrack(AudioSource track) => track.Stop();

		private IEnumerator StartChangeSoundWithFade(string soundName, int trackNumber, float targetVolume)
		{
			AudioSource audioSource = GetTrack(trackNumber);

			yield return Fade(audioSource, 0);

			audioSource.clip = GetAudioClip(soundName);
			audioSource.loop = true;

			if (!audioSource.isPlaying)
				audioSource.Play();

			yield return Fade(audioSource, targetVolume);
		}

		private IEnumerator Fade(AudioSource track, float targetVolume)
		{
			float currentTime = 0;
			float start = track.volume;

			while (currentTime < _fadeTime)
			{
				currentTime += Time.deltaTime;
				track.volume = Mathf.Lerp(start, targetVolume, currentTime / _fadeTime);
				yield return null;
			}

			yield break;
		}
		#endregion
	}
}