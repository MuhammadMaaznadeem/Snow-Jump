using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Force Field")]
	public class ForceField : MonoBehaviour
	{
		public float force = 75f;

		private Collider m_collider;

		public Vector3 direction => transform.up;

		private void Start()
		{
			m_collider = GetComponent<Collider>();
			m_collider.isTrigger = true;
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag(Tags.Player))
			{
				if (other.TryGetComponent<Player>(out var player))
				{
					if (player.isGrounded)
					{
						player.verticalVelocity = Vector3.zero;
					}

					player.velocity += direction * force * Time.deltaTime;
				}
			}
		}
	}
}
