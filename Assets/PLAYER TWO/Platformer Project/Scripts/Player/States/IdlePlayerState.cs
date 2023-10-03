using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Idle Player State")]
	public class IdlePlayerState : PlayerState
	{
		protected override void OnEnter(Player player) { }

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.SnapToGround();
			player.Jump();
			player.Fall();

			player.inputs.GetLeftThumbDirection(out var magnitude);

			if (magnitude > 0 || player.lateralVelocity.sqrMagnitude > 0)
			{
				player.states.Change<WalkPlayerState>();
			}
			else if (player.inputs.GetLeftTrigger() > 0)
			{
				player.states.Change<CrouchPlayerState>();
			}
		}
	}
}
