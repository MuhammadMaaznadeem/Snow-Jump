using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(CapsuleCollider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Pole")]
	public class Pole : MonoBehaviour
	{
		/// <summary>
		/// Returns the Collider of this Pole.
		/// </summary>
		public new CapsuleCollider collider { get; private set; }

		/// <summary>
		/// The radius of this Pole.
		/// </summary>
		public float radius => collider.radius;

		/// <summary>
		/// The center point of this Pole.
		/// </summary>
		public Vector3 center => transform.position;

		/// <summary>
		/// Returns the direction of a given Transform to face this Pole.
		/// </summary>
		/// <param name="other">The transform you want to use.</param>
		/// <returns>The direction from the Transform to the Pole.</returns>
		public Vector3 GetDirectionToPole(Transform other)
		{
			var target = new Vector3(center.x, other.position.y, center.z) - other.position;
			return target.normalized;
		}

		/// <summary>
		/// Returns a point clamped to the Pole height.
		/// </summary>
		/// <param name="point">The point you want to clamp.</param>
		/// <returns>The point within the Pole height.</returns>
		public Vector3 ClampPointToPoleHeight(Vector3 point)
		{
			var minHeight = collider.bounds.min.y;
			var maxHeight = collider.bounds.max.y;
			var clampedHeight = Mathf.Clamp(point.y, minHeight, maxHeight);
			return new Vector3(point.x, clampedHeight, point.z);
		}

		private void Awake()
		{
			tag = Tags.Pole;
			collider = GetComponent<CapsuleCollider>();
		}
	}
}
