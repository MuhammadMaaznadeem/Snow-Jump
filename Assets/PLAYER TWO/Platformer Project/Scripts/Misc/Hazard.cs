using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Hazard")]
	public class Hazard : MonoBehaviour
	{
		public int damage = 1;

		private Collider m_collider;

		private void Awake()
		{
			tag = Tags.Hazard;
			m_collider = GetComponent<Collider>();
			m_collider.isTrigger = true;
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.CompareTag(Tags.Player))
			{
				if (other.TryGetComponent<Player>(out var player))
				{
					player.ApplyDamage(damage);
				}
			}
		}
	}
}
