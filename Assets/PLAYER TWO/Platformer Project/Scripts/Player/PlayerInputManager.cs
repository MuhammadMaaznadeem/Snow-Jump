using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Input Manager")]
	public class PlayerInputManager : MonoBehaviour
	{
		[Header("General")]
		public bool active = true;

		[Header("Left Thumb")]
		public string leftThumbHorizontalName = "LeftThumbHorizontal";
		public string leftThumbVerticalName = "LeftThumbVertical";

		[Header("Right Thumb")]
		public string rightThumbHorizontalName = "RightThumbHorizontal";
		public string rightThumbVerticalName = "RightThumbVertical";

		[Header("D-Pad")]
		public string dPadHorizontalName = "DPadHorizontal";
		public string dPadVerticalName = "DPadVertical";

		[Header("Trigger")]
		public string rightTriggerName = "RightTrigger";
		public string leftTriggerName = "LeftTrigger";

		[Header("Buttons")]
		public string aName = "A";
		public string bName = "B";
		public string xName = "X";
		public string yName = "Y";
		public string rightBumperName = "RightBumper";
		public string leftBumperName = "LeftBumper";
		public string rightThumbName = "RightThumb";
		public string leftThumbName = "LeftThumb";
		public string startName = "Start";
		public string backName = "Back";

		private Camera m_camera;

		public virtual Vector3 GetLeftThumbCameraDirection() => GetLeftThumbCameraDirection(out _);

		public virtual Vector3 GetLeftThumbCameraDirection(out float magnitude) =>
			GlobalToCameraDirection(GetLeftThumbDirection(out magnitude));

		public virtual Vector3 GetLeftThumbDirection() => GetLeftThumbDirection(out _);

		public virtual Vector3 GetLeftThumbDirection(out float magnitude) =>
			GetAxisDirection(leftThumbHorizontalName, leftThumbVerticalName, out magnitude);

		public virtual Vector3 GetRightThumbCameraDirection() => GetRightThumbCameraDirection(out _);

		public virtual Vector3 GetRightThumbCameraDirection(out float magnitude) =>
			GlobalToCameraDirection(GetRightThumbDirection(out magnitude));

		public virtual Vector3 GetRightThumbDirection() => GetRightThumbDirection(out _);

		public virtual Vector3 GetRightThumbDirection(out float magnitude) =>
			GetAxisDirection(rightThumbHorizontalName, rightThumbVerticalName, out magnitude);

		public virtual Vector3 GetDPadDirection() =>
			GetAxisDirection(dPadHorizontalName, dPadVerticalName, out _);

		public virtual float GetRightTrigger() => GetAxis(rightTriggerName);
		public virtual float GetLeftTrigger() => GetAxis(leftTriggerName);

		public virtual bool GetA() => GetButton(aName);
		public virtual bool GetAUp() => GetButtonUp(aName);
		public virtual bool GetADown() => GetButtonDown(aName);

		public virtual bool GetB() => GetButton(bName);
		public virtual bool GetBUp() => GetButtonUp(bName);
		public virtual bool GetBDown() => GetButtonDown(bName);

		public virtual bool GetX() => GetButton(xName);
		public virtual bool GetXUp() => GetButtonUp(xName);
		public virtual bool GetXDown() => GetButtonDown(xName);

		public virtual bool GetY() => GetButton(yName);
		public virtual bool GetYUp() => GetButtonUp(yName);
		public virtual bool GetYDown() => GetButtonDown(yName);

		public virtual bool GetRightBumper() => GetButton(rightBumperName);
		public virtual bool GetRightBumperUp() => GetButtonUp(rightBumperName);
		public virtual bool GetRightBumperDown() => GetButtonDown(rightBumperName);

		public virtual bool GetLeftBumper() => GetButton(leftBumperName);
		public virtual bool GetLeftBumperUp() => GetButtonUp(leftBumperName);
		public virtual bool GetLeftBumperDown() => GetButtonDown(leftBumperName);

		public virtual bool GetRightThumb() => GetButton(rightThumbName);
		public virtual bool GetRightThumbUp() => GetButtonUp(rightThumbName);
		public virtual bool GetRightThumbDown() => GetButtonDown(rightThumbName);

		public virtual bool GetLeftThumb() => GetButton(leftThumbName);
		public virtual bool GetLeftThumbUp() => GetButtonUp(leftThumbName);
		public virtual bool GetLeftThumbDown() => GetButtonDown(leftThumbName);

		public virtual bool GetStart() => GetButton(startName);
		public virtual bool GetStartUp() => GetButtonUp(startName);
		public virtual bool GetStartDown() => GetButtonDown(startName);

		public virtual bool GetBack() => GetButton(backName);
		public virtual bool GetBackUp() => GetButtonUp(backName);
		public virtual bool GetBackDown() => GetButtonDown(backName);

		protected virtual float GetAxis(string name) => active ? Gamepad.GetAxis(name) : 0;

		protected virtual bool GetButton(string name) => active && Gamepad.GetButton(name);

		protected virtual bool GetButtonUp(string name) => active && Gamepad.GetButtonUp(name);

		protected virtual bool GetButtonDown(string name) => active && Gamepad.GetButtonDown(name);

		protected virtual Vector3 GetAxisDirection(string horizontalName, string verticalName, out float magnitude)
		{
			var horizontal = GetAxis(horizontalName);
			var vertical = GetAxis(verticalName);
			var direction = new Vector3(horizontal, 0, vertical);
			magnitude = direction.magnitude;
			return direction.normalized;
		}

		protected virtual Vector3 GlobalToCameraDirection(Vector3 direction)
		{
			if (direction.sqrMagnitude > 0)
			{
				var rotation = Quaternion.AngleAxis(m_camera.transform.eulerAngles.y, Vector3.up);
				direction = rotation * direction;
				direction = direction.normalized;
			}

			return direction;
		}

		private void Start() => m_camera = Camera.main;
	}
}
