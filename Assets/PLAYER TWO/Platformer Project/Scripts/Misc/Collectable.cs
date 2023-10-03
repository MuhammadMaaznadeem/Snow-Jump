using System.Collections;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Collectable")]
	public class Collectable : MonoBehaviour
	{
		public int times = 1;
		public GameObject display;
		public AudioClip clip;

		/// <summary>
		/// Called when it has been collected.
		/// </summary>
		public PlayerEvent onCollect;

		private Collider m_collider;
		private AudioSource m_audio;

		/// <summary>
		/// The collection routine which is trigger the callbacks and activate the reactions.
		/// </summary>
		/// <param name="player">The Player which collected.</param>
		protected virtual IEnumerator CollectRoutine(Player player)
		{
			for (int i = 0; i < times; i++)
			{
				m_audio.Stop();
				m_audio.PlayOneShot(clip);
				onCollect.Invoke(player);
				yield return new WaitForSeconds(0.1f);
			}
		}

		/// <summary>
		/// Triggers the collection of this Collectable.
		/// </summary>
		/// <param name="player">The Player which collected.</param>
		public virtual void Collect(Player player)
		{
			display.SetActive(false);
			m_collider.enabled = false;
			StartCoroutine(CollectRoutine(player));
		}

		private void Awake()
		{
			m_audio = gameObject.AddComponent<AudioSource>();
			m_collider = GetComponent<Collider>();
			m_collider.isTrigger = true;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(Tags.Player))
			{
				if (other.TryGetComponent<Player>(out var player))
				{
					Collect(player);
				}
			}
		}
	}
}
