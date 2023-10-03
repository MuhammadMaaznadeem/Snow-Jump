using UnityEngine;
using System.Collections.Generic;

namespace PLAYERTWO.PlatformerProject
{
	/// <summary>
	/// The Gamepad is an custom input manager based on Unity's Cross Platform Input.
	/// </summary>
	public static class Gamepad
	{
		public class Axis
		{
			private float m_value;

			public void SetValue(float value) => m_value = value;
			public float GetValue() => m_value;
		}

		public class Button
		{
			private bool m_hold;

			private int m_pressedFrame;
			private int m_releasedFrame;

			public void Pressed()
			{
				if (!m_hold)
				{
					m_hold = true;
				}

				m_pressedFrame = Time.frameCount;
			}

			public void Released()
			{
				if (m_hold)
				{
					m_hold = false;
				}

				m_releasedFrame = Time.frameCount;
			}

			public bool GetButton() => m_hold;
			public bool GetButtonDown() => m_pressedFrame != 0 && m_pressedFrame - Time.frameCount == -1;
			public bool GetButtonUp() => m_releasedFrame != 0 && m_releasedFrame == Time.frameCount - 1;
		}

		private static readonly Dictionary<string, Axis> m_axis = new Dictionary<string, Axis>();
		private static readonly Dictionary<string, Button> m_buttons = new Dictionary<string, Button>();

		public static void SetAxis(string axisName, float value)
		{
			if (!m_axis.ContainsKey(axisName))
			{
				m_axis.Add(axisName, new Axis());
			}

			m_axis[axisName].SetValue(value);
		}

		/// <summary>
		/// Returns the value of the axis by its name.
		/// </summary>
		public static float GetAxis(string axisName)
		{
			if (!m_axis.ContainsKey(axisName))
			{
				m_axis.Add(axisName, new Axis());
			}

			return (m_axis[axisName].GetValue() != 0) ? m_axis[axisName].GetValue() : Input.GetAxis(axisName);
		}

		/// <summary>
		/// Returns the value of the axis by its name with no smoothing.
		/// </summary>
		public static float GetAxisRaw(string axisName)
		{
			float value = GetAxis(axisName);

			if (value != 0)
			{
				value = (value < 0) ? -1 : 1;
			}

			return value;
		}

		public static void PressButton(string buttonName)
		{
			if (!m_buttons.ContainsKey(buttonName))
			{
				m_buttons.Add(buttonName, new Button());
			}

			m_buttons[buttonName].Pressed();
		}

		public static void ReleaseButton(string buttonName)
		{
			if (!m_buttons.ContainsKey(buttonName))
			{
				m_buttons.Add(buttonName, new Button());
			}

			m_buttons[buttonName].Released();
		}

		/// <summary>
		/// Returns true if the button is being held in the current frame.
		/// </summary>
		public static bool GetButton(string buttonName)
		{
			if (!m_buttons.ContainsKey(buttonName))
			{
				m_buttons.Add(buttonName, new Button());
			}

			return m_buttons[buttonName].GetButton() || Input.GetButton(buttonName);
		}

		/// <summary>
		/// Returns true if the button is pressed in the current frame.
		/// </summary>
		public static bool GetButtonDown(string buttonName)
		{
			if (!m_buttons.ContainsKey(buttonName))
			{
				m_buttons.Add(buttonName, new Button());
			}

			return m_buttons[buttonName].GetButtonDown() || Input.GetButtonDown(buttonName);
		}

		/// <summary>
		/// Returns true if the button was released in the current frame.
		/// </summary>
		public static bool GetButtonUp(string buttonName)
		{
			if (!m_buttons.ContainsKey(buttonName))
			{
				m_buttons.Add(buttonName, new Button());
			}

			return m_buttons[buttonName].GetButtonUp() || Input.GetButtonUp(buttonName);
		}
	}
}
