using Managers.Audio;
using UnityEngine;

namespace Audio
{
	public class PlayNextLevelMusic : PlayLevelMusic
	{
		protected override void Start()
		{
			foreach (var musicPerTrack in _musicsPerTrack)
			{
				int track = musicPerTrack.Track <= 0 ? 1 : musicPerTrack.Track;
				if (musicPerTrack.MusicId == string.Empty)
					AudioManager.Instance.StopTrack(track);
				else
				{
					if (AudioManager.Instance.IsTrackPlayingSound(musicPerTrack.MusicId, track) == false)
						AudioManager.Instance.QueueSound(musicPerTrack.MusicId, track);
				}
			}
		}
	}
}
