using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Player))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Lean")]
	public class PlayerLean : MonoBehaviour
	{
		public Transform target;
		public float maxTilt = 15;
		public float duration = 0.2f;

		private Player m_player;
		private Quaternion m_initialRotation;

		private float m_velocity;

		/// <summary>
		/// Returns true if the Player should be able to lean.
		/// </summary>
		public virtual bool CanLean()
		{
			var grounded = m_player.isGrounded;
			var onWater = m_player.onWater;
			var crouched = m_player.states.IsCurrentOfType(typeof(CrouchPlayerState));
			var crawling = m_player.states.IsCurrentOfType(typeof(CrawlingPlayerState));
			return (grounded || onWater) && !crouched && !crawling;
		}

		private void Awake()
		{
			m_player = GetComponent<Player>();
		}

		private void LateUpdate()
		{
			var inputDirection = m_player.inputs.GetLeftThumbCameraDirection();
			var moveDirection = m_player.lateralVelocity.normalized;
			var angle = Vector3.SignedAngle(inputDirection, moveDirection, Vector3.up);
			var amount = CanLean() ? Mathf.Clamp(angle, -maxTilt, maxTilt) : 0;
			var rotation = target.localEulerAngles;
			rotation.z = Mathf.SmoothDampAngle(rotation.z, amount, ref m_velocity, duration);
			target.localEulerAngles = rotation;
		}
	}
}
