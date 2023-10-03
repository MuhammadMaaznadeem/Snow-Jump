using System.Collections;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Game/Respawner")]
	public class Respawner : Singleton<Respawner>
	{
		public float fadeDelay = 1f;

		private Fader m_fader;
		private Score m_score;
		private Player m_player;

		private IEnumerator RespawnRoutine()
		{
			yield return new WaitForSeconds(fadeDelay);

			m_fader.FadeOut(() =>
			{
				m_score.lives--;
				m_score.coins = 0;
				m_player.Respawn();
				m_fader.FadeIn();
			});
		}

		private void HandlePlayerDeath()
		{
			StopAllCoroutines();
			StartCoroutine(RespawnRoutine());
		}

		private void Start()
		{
			m_fader = Fader.instance;
			m_score = Score.instance;
			m_player = FindObjectOfType<Player>();
			m_player.OnDie.AddListener(HandlePlayerDeath);
		}
	}
}
