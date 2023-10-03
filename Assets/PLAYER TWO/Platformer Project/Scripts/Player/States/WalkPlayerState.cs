using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Walk Player State")]
	public class WalkPlayerState : PlayerState
	{
		protected override void OnEnter(Player player) { }

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.SnapToGround();
			player.Jump();
			player.Fall();

			var inputDirection = player.inputs.GetLeftThumbCameraDirection(out var magnitude);

			if (magnitude > 0)
			{
				var dot = Vector3.Dot(inputDirection, player.lateralVelocity);

				if (dot >= -0.8f)
				{
					player.Accelerate(inputDirection);
					player.FaceDirectionSmooth(player.lateralVelocity);
				}
				else
				{
					player.states.Change<BrakePlayerState>();
				}
			}
			else
			{
				player.Friction();

				if (player.lateralVelocity.sqrMagnitude <= 0)
				{
					player.states.Change<IdlePlayerState>();
				}
			}

			if (player.inputs.GetLeftTrigger() > 0)
			{
				player.states.Change<CrouchPlayerState>();
			}
		}
	}
}
