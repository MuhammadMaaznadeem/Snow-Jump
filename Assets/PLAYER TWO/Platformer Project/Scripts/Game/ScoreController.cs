using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Game/Score Controller")]
	public class ScoreController : MonoBehaviour
	{
		protected virtual Score m_instance => Score.instance;

		/// <summary>
		/// Set the current amount of coins.
		/// </summary>
		public virtual void SetCoins(int amount)
		{
			if (m_instance)
			{
				m_instance.coins = amount;
			}
		}

		/// <summary>
		/// Set the current amount of lives.
		/// </summary>
		public virtual void SetLives(int amount)
		{
			if (m_instance)
			{
				m_instance.lives = amount;
			}
		}

		/// <summary>
		/// Sum a given amount of coins.
		/// </summary>
		public virtual void AddCoins(int amount)
		{
			if (m_instance)
			{
				m_instance.coins += amount;
			}
		}

		/// <summary>
		/// Sum a given amount of lives.
		/// </summary>
		public virtual void AddLives(int amount)
		{
			if (m_instance)
			{
				m_instance.lives += amount;
			}
		}
	}
}
