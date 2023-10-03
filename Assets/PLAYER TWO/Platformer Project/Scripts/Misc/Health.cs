using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Health")]
	public class Health : MonoBehaviour
	{
		public int initial = 3;
		public int max = 3;
		public float coolDown = 1f;

		public UnityEvent OnChanged;

		private float m_lastDamageTime;

		private int m_current;

		/// <summary>
		/// Returns the current amount of health.
		/// </summary>
		public int current
		{
			get { return m_current; }

			private set
			{
				var last = m_current;

				if (value != last)
				{
					m_current = value;
					OnChanged?.Invoke();
				}
			}
		}

		/// <summary>
		/// Returns true if the Health is empty.
		/// </summary>
		public bool isEmpty => current == 0;

		/// <summary>
		/// Returns true if it's still recovering from the last damage.
		/// </summary>
		public virtual bool recovering => Time.time < m_lastDamageTime + coolDown;

		/// <summary>
		/// Sets the current health to a given amount.
		/// </summary>
		/// <param name="amount">The total health you want to set.</param>
		public virtual void Set(int amount) => current = Mathf.Clamp(current, 0, max);

		/// <summary>
		/// Increases the amount of health.
		/// </summary>
		/// <param name="amount">The amount you want to increase.</param>
		public virtual void Increase(int amount) => current = Mathf.Clamp(current + Mathf.Abs(amount), 0, max);

		/// <summary>
		/// Decreases the amount of health.
		/// </summary>
		/// <param name="amount">The amount you want to decrease.</param>
		public virtual void Damage(int amount)
		{
			if (!recovering)
			{
				current -= Mathf.Abs(amount);
				current = Mathf.Max(current, 0);
				m_lastDamageTime = Time.time;
			}
		}

		/// <summary>
		/// Set the current health back to its initial value.
		/// </summary>
		public virtual void Reset() => current = initial;

		private void Awake() => current = initial;
	}
}
