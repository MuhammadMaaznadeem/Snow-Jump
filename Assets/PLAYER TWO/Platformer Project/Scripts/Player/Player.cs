using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(PlayerInputManager))]
	[RequireComponent(typeof(PlayerStatsManager))]
	[RequireComponent(typeof(PlayerStateManager))]
	[RequireComponent(typeof(Health))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player")]
	public class Player : Entity<Player>
	{
		/// <summary>
		/// Called when the Player jumps.
		/// </summary>
		public UnityEvent OnJump;

		/// <summary>
		/// Called when the Player gets damage.
		/// </summary>
		public UnityEvent OnHurt;

		/// <summary>
		/// Called when the Player died.
		/// </summary>
		public UnityEvent OnDie;

		private Vector3 m_respawnPosition;
		private Quaternion m_respawnRotation;

		/// <summary>
		/// Returns the Player Input Manager instance.
		/// </summary>
		public PlayerInputManager inputs { get; private set; }

		/// <summary>
		/// Returns the Player Stats Manager instance.
		/// </summary>
		public PlayerStatsManager stats { get; private set; }

		/// <summary>
		/// Returns the Health instance.
		/// </summary>
		public Health health { get; private set; }

		/// <summary>
		/// Returns true if the Player is on water.
		/// </summary>
		public bool onWater { get; protected set; }

		/// <summary>
		/// Returns how many times the Player jumped.
		/// </summary>
		public int jumpCounter { get; protected set; }

		/// <summary>
		/// Returns the normal of the last wall the Player touched.
		/// </summary>
		public Vector3 lastWallNormal { get; protected set; }

		/// <summary>
		/// Returns the Pole instance in which the Player is colliding with.
		/// </summary>
		public Pole pole { get; protected set; }

		/// <summary>
		/// Returns the Water instance in which the Player is colliding with.
		/// </summary>
		public Water water { get; protected set; }

		protected virtual void InitializeInputs() => inputs = GetComponent<PlayerInputManager>();
		protected virtual void InitializeStats() => stats = GetComponent<PlayerStatsManager>();
		protected virtual void InitializeHealth() => health = GetComponent<Health>();
		protected virtual void InitializeTag() => tag = Tags.Player;

		/// <summary>
		/// Resets Player state, health, position, and rotation.
		/// </summary>
		public virtual void Respawn()
		{
			health.Reset();
			transform.SetPositionAndRotation(m_respawnPosition, m_respawnRotation);
			states.Change<IdlePlayerState>();
		}

		/// <summary>
		/// Sets the position and rotation of the Player for the next respawn.
		/// </summary>
		public virtual void SetRespawn(Vector3 position, Quaternion rotation)
		{
			m_respawnPosition = position;
			m_respawnRotation = rotation;
		}

		/// <summary>
		/// Applies damage to this Player decreasing its health with proper reaction.
		/// </summary>
		/// <param name="amount">The amount of health you want to decrease.</param>
		public virtual void ApplyDamage(int amount)
		{
			if (!health.isEmpty && !health.recovering)
			{
				health.Damage(amount);
				lateralVelocity = -transform.forward * stats.current.hurtBackwardsForce;

				if (!onWater)
				{
					verticalVelocity = Vector3.up * stats.current.hurtUpwardForce;
					states.Change<HurtPlayerState>();
				}

				OnHurt?.Invoke();

				if (health.isEmpty)
				{
					OnDie?.Invoke();
				}
			}
		}

		/// <summary>
		/// Kills the Player.
		/// </summary>
		public virtual void Die()
		{
			health.Set(0);
			OnDie?.Invoke();
		}

		/// <summary>
		/// Makes the Player transition to the Swim State.
		/// </summary>
		/// <param name="water">The instance of the water.</param>
		public virtual void EnterWater(Water water)
		{
			if (!onWater && !health.isEmpty)
			{
				onWater = true;
				this.water = water;
				states.Change<SwimPlayerState>();
			}
		}

		/// <summary>
		/// Makes the Player exit the current water instance.
		/// </summary>
		public virtual void ExitWater()
		{
			if (onWater)
			{
				onWater = false;
			}
		}

		protected override void HandleSlopeLimit(RaycastHit hit)
		{
			var slopeDirection = Vector3.Cross(hit.normal, Vector3.Cross(hit.normal, Vector3.up));
			slopeDirection = slopeDirection.normalized;
			controller.Move(slopeDirection * stats.current.slideForce * Time.deltaTime);
		}

		protected override void HandleHighLedge(RaycastHit hit)
		{
			var edgeNormal = hit.point - transform.position;
			var edgePushDirection = Vector3.Cross(edgeNormal, Vector3.Cross(edgeNormal, Vector3.up));
			controller.Move(edgePushDirection * stats.current.gravity * Time.deltaTime);
		}

		/// <summary>
		/// Moves the Player smoothly in a given direction.
		/// </summary>
		/// <param name="direction">The direction you want to move.</param>
		public virtual void Accelerate(Vector3 direction)
		{
			var turningDrag = inputs.GetX() ? stats.current.runningTurningDrag : stats.current.turningDrag;
			var acceleration = inputs.GetX() ? stats.current.runningAcceleration : stats.current.acceleration;
			var topSpeed = inputs.GetX() ? stats.current.runningTopSpeed : stats.current.topSpeed;
			Accelerate(direction, turningDrag, acceleration, topSpeed);
		}

		/// <summary>
		/// Moves the Player smoothly in a given direction with water stats.
		/// </summary>
		/// <param name="direction">The direction you want to move.</param>
		public virtual void WaterAcceleration(Vector3 direction) =>
			Accelerate(direction, stats.current.waterTurningDrag, stats.current.swimAcceleration, stats.current.swimTopSpeed);

		/// <summary>
		/// Moves the Player smoothly in a given direction with crawling stats.
		/// </summary>
		/// <param name="direction">The direction you want to move.</param>
		public virtual void CrawlingAccelerate(Vector3 direction) =>
			Accelerate(direction, stats.current.crawlingTurningSpeed, stats.current.crawlingAcceleration, stats.current.crawlingTopSpeed);

		/// <summary>
		/// Smoothly sets Lateral Velocity to zero by its deceleration stats.
		/// </summary>
		public virtual void Decelerate() => Decelerate(stats.current.deceleration);

		/// <summary>
		/// Smoothly sets Lateral Velocity to zero by its friction stats.
		/// </summary>
		public virtual void Friction() => Decelerate(stats.current.friction);

		/// <summary>
		/// Applies a downward force by its gravity stats.
		/// </summary>
		public virtual void Gravity() => Gravity(stats.current.gravity);

		/// <summary>
		/// Applies a downward force when ground by its snap stats.
		/// </summary>
		public virtual void SnapToGround() => SnapToGround(stats.current.snapForce);

		/// <summary>
		/// Rotate the Player forward to a given direction.
		/// </summary>
		/// <param name="direction">The direction you want it to face.</param>
		public virtual void FaceDirectionSmooth(Vector3 direction) => FaceDirection(direction, stats.current.rotationSpeed);

		/// <summary>
		/// Rotates the Player forward to a given direction with water stats.
		/// </summary>
		/// <param name="direction">The direction you want it to face.</param>
		public virtual void WaterFaceDirection(Vector3 direction) => FaceDirection(direction, stats.current.waterRotationSpeed);

		/// <summary>
		/// Makes a transition to the Fall State if the Player is not grounded.
		/// </summary>
		public virtual void Fall()
		{
			if (!isGrounded)
			{
				states.Change<FallPlayerState>();
			}
		}

		/// <summary>
		/// Handles ground jump with proper evaluations and height control.
		/// </summary>
		public virtual void Jump()
		{
			var canMultiJump = (jumpCounter > 0) && (jumpCounter < stats.current.multiJumps);
			var canCoyoteJump = (jumpCounter == 0) && (Time.time < lastGroundTime + stats.current.coyoteJumpThreshold);

			if (isGrounded || canMultiJump || canCoyoteJump)
			{
				if (inputs.GetADown())
				{
					Jump(stats.current.maxJumpHeight);
				}
			}

			if (inputs.GetAUp() && (jumpCounter > 0) && (verticalVelocity.y > stats.current.minJumpHeight))
			{
				verticalVelocity = Vector3.up * stats.current.minJumpHeight;
			}
		}

		/// <summary>
		/// Applies an upward force to the Player.
		/// </summary>
		/// <param name="height">The force you want to apply.</param>
		public virtual void Jump(float height)
		{
			jumpCounter++;
			verticalVelocity = Vector3.up * stats.current.maxJumpHeight;
			OnJump?.Invoke();
		}

		/// <summary>
		/// Applies jump force to the Player in a given direction.
		/// </summary>
		/// <param name="direction">The direction that you want to jump.</param>
		/// <param name="height">The upward force that you want to apply.</param>
		/// <param name="distance">The force towards the direction that you want to apply.</param>
		public virtual void DirectionalJump(Vector3 direction, float height, float distance)
		{
			jumpCounter++;
			verticalVelocity = Vector3.up * height;
			lateralVelocity = direction * distance;
			OnJump?.Invoke();
		}

		/// <summary>
		/// Sets the jump counter to zero affecting further jump evaluations.
		/// </summary>
		public virtual void ResetJumps() => jumpCounter = 0;

		protected override void Awake()
		{
			base.Awake();
			InitializeInputs();
			InitializeStats();
			InitializeHealth();
			InitializeTag();
			OnGroundEnter.AddListener(ResetJumps);
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (hit.rigidbody)
			{
				var above = stepPosition.y > hit.collider.bounds.max.y;

				if (!above && CapsuleCast(hit.moveDirection, 0.25f))
				{
					var force = lateralVelocity * stats.current.pushForce;
					hit.rigidbody.velocity += force / hit.rigidbody.mass * Time.deltaTime;
				}
			}

			if (hit.collider.CompareTag(Tags.Enemy))
			{
				if (hit.collider.TryGetComponent<Enemy>(out var enemy))
				{
					if ((velocity.y <= 0) && IsPointUnderStep(hit.point))
					{
						var rebound = Mathf.Max(-verticalVelocity.y, stats.current.minReboundForce);
						verticalVelocity = Vector3.up * rebound;
						enemy.ApplyDamage(stats.current.jumpDamage);
					}
					else
					{
						ApplyDamage(enemy.stats.current.contactDamage);
					}
				}
			}
			else if (!health.isEmpty)
			{
				if ((hit.normal.y > 0.5f) && hit.collider.TryGetComponent<Spring>(out var spring))
				{
					verticalVelocity = Vector3.zero;
					spring.ApplyForce(this);
				}
				else if (!isGrounded && !onWater && (hit.normal.y < 0.5f && hit.normal.y > 0))
				{
					if (hit.collider.CompareTag(Tags.Pole) && stats.current.canPoleClimb)
					{
						if (hit.collider.TryGetComponent<Pole>(out var pole))
						{
							this.pole = pole;
							states.Change<PoleClimbingPlayerState>();
						}
					}
					else if ((velocity.y <= 0) && stats.current.canWallDrag)
					{
						lastWallNormal = hit.normal;
						states.Change<WallDragPlayerState>();
					}
				}
			}
		}
	}
}
