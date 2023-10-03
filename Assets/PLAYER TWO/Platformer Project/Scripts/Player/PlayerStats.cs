using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "PLAYER TWO/Platformer Project/Player/New Player Stats")]
	public class PlayerStats : EntityStats<PlayerStats>
	{
		[Header("General Stats")]
		public float pushForce = 4f;
		public float snapForce = 15f;
		public float slideForce = 10f;
		public float rotationSpeed = 970f;
		public float minReboundForce = 10f;
		public float gravity = 38f;

		[Header("Motion Stats")]
		public float acceleration = 13f;
		public float deceleration = 28f;
		public float friction = 16f;
		public float topSpeed = 6f;
		public float turningDrag = 28f;

		[Header("Running Stats")]
		public float runningAcceleration = 16f;
		public float runningTopSpeed = 7.5f;
		public float runningTurningDrag = 14f;

		[Header("Jump Stats")]
		public int multiJumps = 1;
		public int jumpDamage = 1;
		public float coyoteJumpThreshold = 0.15f;
		public float maxJumpHeight = 17f;
		public float minJumpHeight = 10f;

		[Header("Crouch Stats")]
		public float crouchHeight = 1f;
		public float crouchFriction = 10f;

		[Header("Crawling Stats")]
		public float crawlingAcceleration = 8f;
		public float crawlingFriction = 32f;
		public float crawlingTopSpeed = 2.5f;
		public float crawlingTurningSpeed = 3f;

		[Header("Wall Drag Stats")]
		public bool canWallDrag = true;
		public float wallDragGravity = 12f;
		public float wallJumpDistance = 8f;
		public float wallJumpHeight = 15f;

		[Header("Pole Climb Stats")]
		public bool canPoleClimb = true;
		public float climbUpSpeed = 3f;
		public float climbDownSpeed = 8f;
		public float climbRotationSpeed = 2f;
		public float poleJumpDistance = 8f;
		public float poleJumpHeight = 15f;

		[Header("Swimming Stats")]
		public float waterConversion = 0.35f;
		public float waterRotationSpeed = 360f;
		public float waterUpwardsForce = 8f;
		public float waterJumpHeight = 15f;
		public float waterTurningDrag = 2.5f;
		public float swimAcceleration = 4f;
		public float swimDeceleration = 3f;
		public float swimTopSpeed = 4f;
		public float swimDiveForce = 15f;

		[Header("Hurt Stats")]
		public float hurtUpwardForce = 10f;
		public float hurtBackwardsForce = 5f;
	}
}
