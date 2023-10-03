using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Crouch Player State")]
	public class CrouchPlayerState : PlayerState
	{
		protected override void OnEnter(Player player)
		{
			player.ResizeCollider(player.stats.current.crouchHeight);
		}

		protected override void OnExit(Player player)
		{
			player.ResizeCollider(player.originalHeight);
		}

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.SnapToGround();
			player.Jump();
			player.Fall();

			player.Decelerate(player.stats.current.crouchFriction);
			player.inputs.GetLeftThumbDirection(out var magnitude);

			if (player.inputs.GetLeftTrigger() > 0)
			{
				var speedMagnitude = player.lateralVelocity.sqrMagnitude;

				if ((magnitude > 0) && (speedMagnitude == 0))
				{
					player.states.Change<CrawlingPlayerState>();
				}
			}
			else
			{
				player.states.Change<IdlePlayerState>();
			}
		}
	}
}
