using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Player))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Animator")]
	public class PlayerAnimator : MonoBehaviour
	{
		public Animator animator;

		private Player m_player;

		private void Start()
		{
			m_player = GetComponent<Player>();
		}

		private void LateUpdate()
		{
			var state = m_player.states.index;
			var lateralSpeed = m_player.lateralVelocity.magnitude;
			var verticalSpeed = m_player.verticalVelocity.y;
			var lateralAnimationSpeed = Mathf.Max(0.3f, lateralSpeed / m_player.stats.current.topSpeed);

			animator.SetInteger("State", state);
			animator.SetFloat("Lateral Speed", lateralSpeed);
			animator.SetFloat("Vertical Speed", verticalSpeed);
			animator.SetFloat("Lateral Animation Speed", lateralAnimationSpeed);
		}
	}
}
