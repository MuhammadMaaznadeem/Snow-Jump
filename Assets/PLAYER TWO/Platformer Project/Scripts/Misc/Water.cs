using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Water")]
	public class Water : MonoBehaviour
	{
		public float exitOffset = 0.25f;
		public AudioClip enterClip;
		public AudioClip exitClip;

		private Player m_player;
		private AudioSource m_audio;
		private Collider m_collider;

		/// <summary>
		/// The water bounds.
		/// </summary>
		public virtual Bounds bounds => m_collider.bounds;

		/// <summary>
		/// Clamp a point to the water bounds.
		/// </summary>
		/// <param name="point">The point you want to clamp.</param>
		/// <returns>A point clamped to the water volume.</returns>
		public virtual Vector3 ClampToVolume(Vector3 point)
		{
			var min = bounds.min;
			var max = bounds.max;
			var x = Mathf.Clamp(point.x, min.x, max.x);
			var y = Mathf.Clamp(point.y, min.y, max.y);
			var z = Mathf.Clamp(point.z, min.z, max.z);
			return new Vector3(x, y, z);
		}

		private void InitializeCollider()
		{
			m_collider = GetComponent<Collider>();
			m_collider.isTrigger = true;
		}

		private void InitializeAudioSource()
		{
			if (!TryGetComponent(out m_audio))
			{
				m_audio = gameObject.AddComponent<AudioSource>();
			}

			m_audio.spatialBlend = 0.5f;
		}

		private void Start()
		{
			InitializeCollider();
			InitializeAudioSource();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag(Tags.Player))
			{
				m_audio.PlayOneShot(enterClip);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.CompareTag(Tags.Player))
			{
				m_audio.PlayOneShot(exitClip);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag(Tags.Player))
			{
				if (other.TryGetComponent<Player>(out m_player))
				{
					if (!m_player.onWater && bounds.Contains(other.transform.position))
					{
						m_player.EnterWater(this);
						m_audio.PlayOneShot(enterClip);
					}
					else if (m_player.onWater)
					{
						var offset = Vector3.down * exitOffset;
						var exitPoint = other.transform.position + offset;

						if (!bounds.Contains(exitPoint))
						{
							m_player.ExitWater();
							m_audio.PlayOneShot(exitClip);
						}
					}
				}
			}
			else if (other.TryGetComponent<Rigidbody>(out var rigidbody))
			{
				var position = rigidbody.position;
				rigidbody.MovePosition(ClampToVolume(position));
				rigidbody.AddForce(Vector3.up * 10f);
			}
		}
	}
}
