using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Player))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Audio")]
	public class PlayerAudio : MonoBehaviour
	{
		public AudioClip jump;
		public AudioClip hurt;

		protected Player m_player;
		protected AudioSource m_audio;

		protected virtual void InitializePlayer() => m_player = GetComponent<Player>();

		protected virtual void InitializeAudio()
		{
			if (!TryGetComponent(out m_audio))
			{
				m_audio = gameObject.AddComponent<AudioSource>();
			}
		}

		protected virtual void InitializeCallbacks()
		{
			m_player.OnJump.AddListener(() => m_audio.PlayOneShot(jump));
			m_player.OnHurt.AddListener(() => m_audio.PlayOneShot(hurt));
		}

		protected virtual void Start()
		{
			InitializeAudio();
			InitializePlayer();
			InitializeCallbacks();
		}
	}
}
