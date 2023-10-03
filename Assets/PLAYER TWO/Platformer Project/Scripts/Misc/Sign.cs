using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(Collider))]
	public class Sign : MonoBehaviour
	{
		[TextArea(15, 20)]
		public string text = "Hello World";
		public float scaleDuration = 0.25f;

		public Canvas canvas;
		public Text uiText;

		private Vector3 m_initialScale;

		private IEnumerator Scale(Vector3 from, Vector3 to)
		{
			var elapsedTime = 0f;
			var scale = canvas.transform.localScale;

			while (elapsedTime < scaleDuration)
			{
				scale = Vector3.Lerp(from, to, (elapsedTime / scaleDuration));
				canvas.transform.localScale = scale;
				elapsedTime += Time.deltaTime;
				yield return null;
			}

			canvas.transform.localScale = to;
		}

		private void Awake()
		{
			uiText.text = text;
			m_initialScale = canvas.transform.localScale;
			canvas.transform.localScale = Vector3.zero;
			canvas.gameObject.SetActive(true);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(Tags.Player))
			{
				StopAllCoroutines();
				StartCoroutine(Scale(Vector3.zero, m_initialScale));
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag(Tags.Player))
			{
				StopAllCoroutines();
				StartCoroutine(Scale(canvas.transform.localScale, Vector3.zero));
			}
		}
	}
}
