using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Brake Player State")]
	public class BrakePlayerState : PlayerState
	{
		protected override void OnEnter(Player player) { }

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.SnapToGround();
			player.Jump();
			player.Fall();
			player.Decelerate();

			if (player.lateralVelocity.sqrMagnitude == 0)
			{
				player.states.Change<IdlePlayerState>();
			}
		}
	}
}
