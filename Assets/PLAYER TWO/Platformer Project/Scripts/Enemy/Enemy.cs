using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(EnemyStatsManager))]
	[RequireComponent(typeof(EnemyStateManager))]
	[RequireComponent(typeof(WaypointManager))]
	[RequireComponent(typeof(Health))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Enemy/Enemy")]
	public class Enemy : Entity<Enemy>
	{
		/// <summary>
		/// Called when the Player enters this Enemy sight.
		/// </summary>
		public UnityEvent OnPlayerSpotted;

		/// <summary>
		/// Called when the Player leaves this Enemy sight.
		/// </summary>
		public UnityEvent OnPlayerScaped;

		/// <summary>
		/// Called when this Enemy touches a Player.
		/// </summary>
		public UnityEvent OnPlayerContact;

		/// <summary>
		/// Called when this Enemy takes damage.
		/// </summary>
		public UnityEvent OnDamage;

		/// <summary>
		/// Called when this Enemy loses all health.
		/// </summary>
		public UnityEvent OnDie;

		private Player m_player;

		private Collider[] m_overlaps = new Collider[5];

		/// <summary>
		/// Returns the Enemy Stats Manager instance.
		/// </summary>
		public EnemyStatsManager stats { get; private set; }

		/// <summary>
		/// Returns the Waypoint Manager instance.
		/// </summary>
		public WaypointManager waypoints { get; private set; }

		/// <summary>
		/// Returns the Health instance.
		/// </summary>
		public Health health { get; private set; }

		/// <summary>
		/// Returns the instance of the Player on the Enemies sight.
		/// </summary>
		public Player player { get; private set; }

		protected virtual void InitializeStatsManager() => stats = GetComponent<EnemyStatsManager>();
		protected virtual void InitializeWaypointsManager() => waypoints = GetComponent<WaypointManager>();
		protected virtual void InitializeHealth() => health = GetComponent<Health>();
		protected virtual void InitializeTag() => tag = Tags.Enemy;

		/// <summary>
		/// Applies damage to this Enemy decreasing its health with proper reaction.
		/// </summary>
		/// <param name="amount">The amount of health you want to decrease.</param>
		public virtual void ApplyDamage(int amount)
		{
			if (!health.isEmpty && !health.recovering)
			{
				health.Damage(amount);
				OnDamage?.Invoke();

				if (health.isEmpty)
				{
					controller.enabled = false;
					OnDie?.Invoke();
				}
			}
		}

		public virtual void Accelerate(Vector3 direction, float acceleration, float topSpeed) =>
			Accelerate(direction, stats.current.turningDrag, acceleration, topSpeed);

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
		/// Rotate the Enemy forward to a given direction.
		/// </summary>
		/// <param name="direction">The direction you want it to face.</param>
		public virtual void FaceDirectionSmooth(Vector3 direction) => FaceDirection(direction, stats.current.rotationSpeed);

		/// <summary>
		/// Handles the view sight and Player detection behaviour.
		/// </summary>
		protected virtual void HandleSight()
		{
			if (!player)
			{
				var overlaps = Physics.OverlapSphereNonAlloc(transform.position, stats.current.spotRange, m_overlaps);

				for (int i = 0; i < overlaps; i++)
				{
					if (m_overlaps[i].CompareTag(Tags.Player))
					{
						if (m_overlaps[i].TryGetComponent<Player>(out var player))
						{
							this.player = player;
							OnPlayerSpotted?.Invoke();
							return;
						}
					}
				}
			}
			else
			{
				var distance = Vector3.Distance(transform.position, player.transform.position);

				if ((player.health.current == 0) || (distance > stats.current.viewRange))
				{
					player = null;
					OnPlayerScaped?.Invoke();
				}
			}
		}

		protected override void OnUpdate() => HandleSight();

		protected override void Awake()
		{
			base.Awake();
			InitializeTag();
			InitializeStatsManager();
			InitializeWaypointsManager();
			InitializeHealth();
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if (hit.collider.CompareTag(Tags.Player))
			{
				if (hit.collider.TryGetComponent<Player>(out var player))
				{
					if (!player.IsPointUnderStep(hit.point))
					{
						if (stats.current.contactPushback)
						{
							lateralVelocity = -transform.forward * stats.current.contactPushBackForce;
						}

						player.ApplyDamage(stats.current.contactDamage);
						OnPlayerContact?.Invoke();
					}
				}
			}
		}
	}
}
