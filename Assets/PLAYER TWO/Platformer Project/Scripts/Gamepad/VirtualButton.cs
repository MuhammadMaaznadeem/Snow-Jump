using UnityEngine;
using UnityEngine.EventSystems;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Gamepad/Virtual Button")]
	public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public string buttonName;

		public void OnPointerDown(PointerEventData eventData) => Gamepad.PressButton(buttonName);
		public void OnPointerUp(PointerEventData eventData) => Gamepad.ReleaseButton(buttonName);
	}
}
