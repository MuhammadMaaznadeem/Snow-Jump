using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Camera")]
	public class PlayerCamera : MonoBehaviour
	{
		public Player player;
		public float fieldOfView = 40f;
		public Vector3 offset = new Vector3(0, 6, -12);

		protected Camera m_camera;

		protected virtual void InitializePlayer()
		{
			if (!player)
			{
				player = FindObjectOfType<Player>();
			}
		}

		protected virtual void InitializeCamera()
		{
			m_camera = GetComponent<Camera>();
			m_camera.fieldOfView = fieldOfView;
		}

		protected virtual void Start()
		{
			InitializePlayer();
			InitializeCamera();
		}

		protected virtual void LateUpdate()
		{
			var targetPosition = player.transform.position;
			transform.position = targetPosition + offset;
			transform.LookAt(player.transform);
		}
	}
}
