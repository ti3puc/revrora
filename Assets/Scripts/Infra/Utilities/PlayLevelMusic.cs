using Managers.Audio;
using UnityEngine;

namespace Audio
{
	public class PlayLevelMusic : MonoBehaviour
	{
		[System.Serializable]
		public struct MusicPerTrack
		{
			public string MusicId;
			public int Track;
		}

		[SerializeField] protected MusicPerTrack[] _musicsPerTrack;

		protected virtual void OnValidate()
		{
			if (_musicsPerTrack == null || _musicsPerTrack.Length <= 0)
				return;

			for (int i = 0; i < _musicsPerTrack.Length; i++)
			{
				if (_musicsPerTrack[i].Track == 0)
					_musicsPerTrack[i].Track = 1;
			}
		}

		protected virtual void Start()
		{
			foreach (var musicPerTrack in _musicsPerTrack)
			{
				int track = musicPerTrack.Track <= 0 ? 1 : musicPerTrack.Track;
				if (musicPerTrack.MusicId == string.Empty)
					AudioManager.Instance.StopTrack(track);
				else
				{
					if (AudioManager.Instance.IsTrackPlayingSound(musicPerTrack.MusicId, track) == false)
						AudioManager.Instance.ChangeSoundWithFade(musicPerTrack.MusicId, track);
				}
			}
		}
	}
}
