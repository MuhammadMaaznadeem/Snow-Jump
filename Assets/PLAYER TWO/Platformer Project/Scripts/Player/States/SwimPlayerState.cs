using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Swim Player State")]
	public class SwimPlayerState : PlayerState
	{
		protected override void OnEnter(Player player) => player.velocity *= player.stats.current.waterConversion;

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			var inputDirection = player.inputs.GetLeftThumbCameraDirection(out var magnitude);

			if (player.transform.position.y < player.water.bounds.max.y)
			{
				if (player.isGrounded)
				{
					player.verticalVelocity = Vector3.zero;
				}

				player.verticalVelocity += Vector3.up * player.stats.current.waterUpwardsForce * Time.deltaTime;
			}
			else if (player.isGrounded)
			{
				player.states.Change<WalkPlayerState>();
			}
			else
			{
				player.verticalVelocity = Vector3.zero;

				if (player.inputs.GetADown())
				{
					player.Jump(player.stats.current.waterJumpHeight);
					player.states.Change<FallPlayerState>();
				}
			}

			if (!player.isGrounded && player.inputs.GetX())
			{
				player.verticalVelocity += Vector3.down * player.stats.current.swimDiveForce * Time.deltaTime;
			}

			if (magnitude == 0)
			{
				player.Decelerate(player.stats.current.swimDeceleration);
			}

			player.WaterAcceleration(inputDirection);
			player.WaterFaceDirection(player.lateralVelocity);
		}
	}
}
