using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Fall Player State")]
	public class FallPlayerState : PlayerState
	{
		protected override void OnEnter(Player player) { }

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.SnapToGround();
			player.Jump();

			if (!player.isGrounded)
			{
				var inputDirection = player.inputs.GetLeftThumbCameraDirection(out var magnitude);

				if (magnitude > 0)
				{
					var dot = Vector3.Dot(inputDirection, player.lateralVelocity);

					if (dot >= 0)
					{
						player.Accelerate(inputDirection);
						player.FaceDirectionSmooth(player.lateralVelocity);
					}
					else
					{
						player.Decelerate();
					}
				}
			}
			else
			{
				player.states.Change<IdlePlayerState>();
			}
		}
	}
}
