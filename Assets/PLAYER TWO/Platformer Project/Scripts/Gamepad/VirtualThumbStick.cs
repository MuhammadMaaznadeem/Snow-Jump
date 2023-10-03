using UnityEngine;
using UnityEngine.EventSystems;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Gamepad/Virtual Thumb Stick")]
	public class VirtualThumbStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		public bool snapToFinger = false;
		public string horizontalAxisName = "LeftThumbHorizontal";
		public string verticalAxisName = "LeftThumbVertical";
		public RectTransform stick;

		[Range(0f, 1f)]
		public float stickRadius = 0.25f;

		[Range(0f, 1f)]
		public float deadZone = 0.1f;

		private Vector2 m_inputDirection;
		private Vector2 m_startPosition;

		private RectTransform m_transform;

		private void Start()
		{
			m_transform = GetComponent<RectTransform>();
		}

		private void UpdateManagerAxis()
		{
			float horizontal = ClampToDeadZone(m_inputDirection.x, deadZone);
			float vertical = ClampToDeadZone(m_inputDirection.y, deadZone);

			Gamepad.SetAxis(horizontalAxisName, horizontal);
			Gamepad.SetAxis(verticalAxisName, vertical);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_transform,
				eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
			{
				localPoint.x /= m_transform.sizeDelta.x;
				localPoint.y /= m_transform.sizeDelta.y;

				m_inputDirection = localPoint * 2.0f;
				m_inputDirection = Vector2.ClampMagnitude(m_inputDirection, 1.0f);

				stick.anchoredPosition = new Vector2(m_inputDirection.x * m_transform.sizeDelta.x *
					stickRadius, m_inputDirection.y * m_transform.sizeDelta.y * stickRadius);

				UpdateManagerAxis();
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			m_startPosition = m_transform.position;

			if (snapToFinger)
			{
				m_transform.position = eventData.position;
			}

			OnDrag(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			m_inputDirection = Vector2.zero;
			m_transform.position = m_startPosition;
			stick.anchoredPosition = Vector2.zero;
			UpdateManagerAxis();
		}

		private float ClampToDeadZone(float value, float deadZone)
		{
			if (value != 0)
			{
				float absolute = Mathf.Abs(value);

				if (absolute > deadZone)
				{
					float rescale = (absolute - deadZone) / (1f - deadZone);
					return value * rescale;
				}
			}

			return 0;
		}
	}
}
