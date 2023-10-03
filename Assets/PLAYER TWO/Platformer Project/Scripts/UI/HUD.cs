using UnityEngine;
using UnityEngine.UI;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/UI/HUD")]
	public class HUD : MonoBehaviour
	{
		public Text lives;
		public Text coins;
		public Text health;

		public string livesFormat = "00";
		public string coinsFormat = "000";
		public string healthFormat = "0";

		private Score m_score;
		private Player m_player;

		/// <summary>
		/// Called when the coins Score changed.
		/// </summary>
		protected virtual void OnCoinsUpdated()
		{
			coins.text = m_score.coins.ToString(coinsFormat);
		}

		/// <summary>
		/// Called when the lives Score changed.
		/// </summary>
		protected virtual void OnLivesUpdated()
		{
			lives.text = m_score.lives.ToString(livesFormat);
		}

		/// <summary>
		/// Called when the Player Health changed.
		/// </summary>
		protected virtual void OnHealthUpdated()
		{
			health.text = m_player.health.current.ToString(healthFormat);
		}

		/// <summary>
		/// Called to force an updated on the HUD.
		/// </summary>
		public virtual void Refresh()
		{
			OnCoinsUpdated();
			OnLivesUpdated();
			OnHealthUpdated();
		}

		private void Start()
		{
			m_score = Score.instance;
			m_player = FindObjectOfType<Player>();
			m_score.OnCoinsUpdated.AddListener(OnCoinsUpdated);
			m_score.OnLivesUpdated.AddListener(OnLivesUpdated);
			m_player.health.OnChanged.AddListener(OnHealthUpdated);
			Refresh();
		}
	}
}
