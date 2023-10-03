using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Pole Climbing Player State")]
	public class PoleClimbingPlayerState : PlayerState
	{
		private const float k_poleOffset = 0.01f;

		protected override void OnEnter(Player player)
		{
			player.velocity = Vector3.zero;
		}

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			var center = player.pole.center;
			center.y = transform.position.y;

			var distance = player.pole.radius + player.radius * 0.5f + k_poleOffset;
			var poleDirection = player.pole.GetDirectionToPole(transform);
			var inputDirection = player.inputs.GetLeftThumbCameraDirection();

			player.FaceDirection(poleDirection);
			player.lateralVelocity = transform.right * inputDirection.x * player.stats.current.climbRotationSpeed;

			if (inputDirection.z != 0)
			{
				var speed = inputDirection.z > 0 ? player.stats.current.climbUpSpeed : -player.stats.current.climbDownSpeed;
				player.verticalVelocity = Vector3.up * speed;
			}
			else
			{
				player.verticalVelocity = Vector3.zero;
			}

			if (player.inputs.GetADown())
			{
				player.FaceDirection(-poleDirection);
				player.DirectionalJump(-poleDirection, player.stats.current.poleJumpHeight, player.stats.current.poleJumpDistance);
				player.states.Change<FallPlayerState>();
			}

			if (player.isGrounded)
			{
				player.states.Change<IdlePlayerState>();
			}


			var position = center - poleDirection * distance;
			transform.position = player.pole.ClampPointToPoleHeight(position);
		}
	}
}
