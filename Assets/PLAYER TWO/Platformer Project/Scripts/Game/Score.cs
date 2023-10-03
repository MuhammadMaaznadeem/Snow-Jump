using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Game/Score")]
	public class Score : Singleton<Score>
	{
		public int initialLives = 3;
		public int maxLives = 99;
		public int maxCoins = 999;

		/// <summary>
		/// Called when the coins counter chenged.
		/// </summary>
		public UnityEvent OnCoinsUpdated;

		/// <summary>
		/// Called when the lives counter chenged.
		/// </summary>
		public UnityEvent OnLivesUpdated;

		private int m_coins;
		private int m_lives;

		/// <summary>
		/// Current coins amount.
		/// </summary>
		public int coins
		{
			get { return m_coins; }

			set
			{
				var target = Mathf.Clamp(value, 0, maxCoins);

				if (m_coins != target)
				{
					m_coins = target;
					OnCoinsUpdated?.Invoke();
				}
			}
		}

		/// <summary>
		/// Current lives amount.
		/// </summary>
		public int lives
		{
			get { return m_lives; }

			set
			{
				var target = Mathf.Clamp(value, 0, maxLives);

				if (value == 0)
				{
					SceneManager.LoadScene(0);
				}

				if (m_lives != target)
				{
					m_lives = target;
					OnLivesUpdated?.Invoke();
				}
			}
		}

		private void Awake() => lives = initialLives;
	}
}
