using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Spring")]
	public class Spring : MonoBehaviour
	{
		public float force = 25f;
		public AudioClip clip;

		private AudioSource m_audio;

		/// <summary>
		/// Applies spring force to a given Player.
		/// </summary>
		/// <param name="player">The Player you want to apply force to.</param>
		public void ApplyForce(Player player)
		{
			if (player.verticalVelocity.y <= 0)
			{
				m_audio.PlayOneShot(clip);
				player.verticalVelocity = Vector3.up * force;
			}
		}

		private void Start()
		{
			if (!TryGetComponent(out m_audio))
			{
				m_audio = gameObject.AddComponent<AudioSource>();
			}
		}
	}
}
