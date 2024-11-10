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

		[SerializeField] private MusicPerTrack[] _musicsPerTrack;

		private void Start()
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
