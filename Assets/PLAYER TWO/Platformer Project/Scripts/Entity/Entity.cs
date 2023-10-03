using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	public abstract class Entity<T> : MonoBehaviour where T : Entity<T>
	{
		/// <summary>
		/// Called when the Entity lands on the ground.
		/// </summary>
		public UnityEvent OnGroundEnter;

		/// <summary>
		/// Called when the Entity leaves the ground.
		/// </summary>
		public UnityEvent OnGroundExit;

		private Collider[] m_contacts = new Collider[5];

		/// <summary>
		/// Returns the State Manager of this Entity.
		/// </summary>
		public EntityStateManager<T> states { get; private set; }

		/// <summary>
		/// Returns the Character Controller of this Entity.
		/// </summary>
		public CharacterController controller { get; private set; }

		/// <summary>
		/// The current velocity of this Entity.
		/// </summary>
		public Vector3 velocity { get; set; }

		/// <summary>
		/// The current XZ velocity of this Entity.
		/// </summary>
		public Vector3 lateralVelocity
		{
			get { return new Vector3(velocity.x, 0, velocity.z); }
			set { velocity = new Vector3(value.x, velocity.y, value.z); }
		}

		/// <summary>
		/// The current Y velocity of this Entity.
		/// </summary>
		public Vector3 verticalVelocity
		{
			get { return new Vector3(0, velocity.y, 0); }
			set { velocity = new Vector3(velocity.x, value.y, velocity.z); }
		}

		/// <summary>
		/// Returns the bottom position of this Entity considering the stepOffset.
		/// </summary>
		public Vector3 stepPosition => transform.position + center - transform.up * (height * 0.5f - controller.stepOffset);

		/// <summary>
		/// Returns the last frame this Entity was grounded.
		/// </summary>
		public float lastGroundTime { get; private set; }

		/// <summary>
		/// Returns true if the Entity is on the ground.
		/// </summary>
		public bool isGrounded { get; private set; } = true;

		/// <summary>
		/// Returns the original height of this Entity.
		/// </summary>
		public float originalHeight { get; private set; }

		/// <summary>
		/// Returns the collider height of this Entity.
		/// </summary>
		public float height => controller.height;

		/// <summary>
		/// Returns the collider radius of this Entity.
		/// </summary>
		public float radius => controller.radius;

		/// <summary>
		/// The center of the Character Controller collider.
		/// </summary>
		public Vector3 center => controller.center;

		protected virtual void InitializeController()
		{
			controller = GetComponent<CharacterController>();

			if (!controller)
			{
				controller = gameObject.AddComponent<CharacterController>();
				controller.skinWidth = 0.005f;
				controller.minMoveDistance = 0;
			}

			originalHeight = controller.height;
		}

		protected virtual void InitializeStateManager() => states = GetComponent<EntityStateManager<T>>();

		/// <summary>
		/// Resizes the Character Controller to a given height.
		/// </summary>
		/// <param name="height">The desired height.</param>
		public virtual void ResizeCollider(float height)
		{
			var delta = height - this.height;
			controller.height = height;
			controller.center += Vector3.up * delta * 0.5f;
		}

		public virtual bool CapsuleCast(Vector3 direction, float distance, int layer = Physics.DefaultRaycastLayers,
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
		{
			return CapsuleCast(direction, distance, out _, layer, queryTriggerInteraction);
		}

		public virtual bool CapsuleCast(Vector3 direction, float distance,
			out RaycastHit hit, int layer = Physics.DefaultRaycastLayers,
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
		{
			var origin = transform.position + center;
			var offset = transform.up * (height * 0.5f - radius);
			var top = origin + offset;
			var bottom = origin - offset;
			return Physics.CapsuleCast(top, bottom, radius, direction,
				out hit, distance, layer, queryTriggerInteraction);
		}

		/// <summary>
		/// Returns true if a given point is bellow the Entity's step position.
		/// </summary>
		/// <param name="point">The point you want to evaluate.</param>
		public virtual bool IsPointUnderStep(Vector3 point) => stepPosition.y > point.y;

		protected virtual void HandleStates() => states.Step();

		protected virtual void HandleController() => controller.Move(velocity * Time.deltaTime);

		protected virtual void HandleGround()
		{
			var origin = transform.position + center;
			var distance = (height * 0.5f) - controller.radius + 0.1f;

			if (Physics.SphereCast(origin, radius, -transform.up, out var hit,
				distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
			{
				if (!isGrounded && EvaluateLanding(hit) && (verticalVelocity.y <= 0))
				{
					isGrounded = true;
					OnGroundEnter?.Invoke();
				}

				if (isGrounded)
				{
					if (IsPointUnderStep(hit.point))
					{
						transform.parent = hit.collider.CompareTag(Tags.Platform) ? hit.transform : null;

						if (Physics.Raycast(origin, -transform.up, out var hit1,
							height, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
						{
							if (Vector3.Angle(hit1.normal, Vector3.up) >= controller.slopeLimit)
							{
								HandleSlopeLimit(hit1);
							}
						}
					}
					else
					{
						HandleHighLedge(hit);
					}
				}
			}
			else if (isGrounded)
			{
				isGrounded = false;
				transform.parent = null;
				lastGroundTime = Time.time;
				verticalVelocity = Vector3.Max(verticalVelocity, Vector3.zero);
				OnGroundExit?.Invoke();
			}
		}

		protected virtual void HandleCeiling()
		{
			var origin = transform.position + center;
			var distance = (height * 0.5f) - controller.radius + 0.1f;

			if (Physics.SphereCast(origin, radius, transform.up, out var hit,
				distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
			{
				if (verticalVelocity.y > 0)
				{
					verticalVelocity = Vector3.zero;
				}

				HandleCeilingCollision(hit);
			}
		}

		protected virtual bool EvaluateLanding(RaycastHit hit) => true;

		protected virtual void HandleSlopeLimit(RaycastHit hit) { }

		protected virtual void HandleHighLedge(RaycastHit hit) { }

		protected virtual void HandleCeilingCollision(RaycastHit hit) { }

		protected virtual void OnUpdate() { }

		/// <summary>
		/// Moves the Player smoothly in a given direction.
		/// </summary>
		/// <param name="direction">The direction you want to move.</param>
		/// <param name="turningDrag">How fast it will turn towards the new direction.</param>
		/// <param name="acceleration">How fast it will move over time.</param>
		/// <param name="topSpeed">The max movement magnitude.</param>
		public virtual void Accelerate(Vector3 direction, float turningDrag, float acceleration, float topSpeed)
		{
			if (direction.sqrMagnitude > 0)
			{
				var speed = Vector3.Dot(direction, lateralVelocity);
				var velocity = direction * speed;
				var turningVelocity = lateralVelocity - velocity;
				var turningDelta = turningDrag * Time.deltaTime;
				speed += acceleration * Time.deltaTime;
				speed = Mathf.Clamp(speed, -topSpeed, topSpeed);
				velocity = direction * speed;
				turningVelocity = Vector3.MoveTowards(turningVelocity, Vector3.zero, turningDelta);
				lateralVelocity = velocity + turningVelocity;
			}
		}

		/// <summary>
		/// Smoothly moves Lateral Velocity to zero.
		/// </summary>
		/// <param name="deceleration">How fast it will decelerate over time.</param>
		public virtual void Decelerate(float deceleration)
		{
			var delta = deceleration * Time.deltaTime;
			lateralVelocity = Vector3.MoveTowards(lateralVelocity, Vector3.zero, delta);
		}

		/// <summary>
		/// Smoothly moves vertical velocity to zero.
		/// </summary>
		/// <param name="gravity">How fast it will move over time.</param>
		public virtual void Gravity(float gravity)
		{
			if (!isGrounded)
			{
				verticalVelocity += Vector3.down * gravity * Time.deltaTime;
			}
		}

		/// <summary>
		/// Applies a force towards the ground.
		/// </summary>
		/// <param name="force">The force you want to apply.</param>
		public virtual void SnapToGround(float force)
		{
			if (isGrounded && (verticalVelocity.y <= 0))
			{
				verticalVelocity = Vector3.down * force;
			}
		}

		/// <summary>
		/// Rotate the Player towards to a given direction.
		/// </summary>
		/// <param name="direction">The direction you want to face.</param>
		public virtual void FaceDirection(Vector3 direction)
		{
			if (direction.sqrMagnitude > 0)
			{
				var rotation = Quaternion.LookRotation(direction, Vector3.up);
				transform.rotation = rotation;
			}
		}

		/// <summary>
		/// Rotate the Player towards to a given direction.
		/// </summary>
		/// <param name="direction">The direction you want to face.</param>
		/// <param name="degreesPerSecond">How fast it should rotate over time.</param>
		public virtual void FaceDirection(Vector3 direction, float degreesPerSecond)
		{
			if (direction.sqrMagnitude > 0)
			{
				var rotation = transform.rotation;
				var rotationDelta = degreesPerSecond * Time.deltaTime;
				var target = Quaternion.LookRotation(direction, Vector3.up);
				transform.rotation = Quaternion.RotateTowards(rotation, target, rotationDelta);
			}
		}

		protected virtual void Awake()
		{
			InitializeController();
			InitializeStateManager();
		}

		protected virtual void Update()
		{
			if (controller.enabled)
			{
				HandleStates();
				HandleController();
				HandleGround();
				HandleCeiling();
				OnUpdate();
			}
		}
	}
}
