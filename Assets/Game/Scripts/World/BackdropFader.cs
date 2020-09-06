using System.Collections;
using UnityEngine;

namespace Game.Scripts.World
{
	public class BackdropFader : MonoBehaviour
	{
		#region Fields

		private CanvasGroup canvasGroup;
		private Coroutine currentlyActiveCoroutine;

		#endregion

		#region MonoBehaviour Callbacks

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		#endregion

		#region Public Methods

		public void FadeOutImmediately()
		{
			DisableInput(1);
			canvasGroup.alpha = 1;
		}

		#endregion

		#region Private Methods

		private void DisableInput(int target)
		{
			canvasGroup.blocksRaycasts = target == 1;
			Cursor.visible = target != 1;
			Cursor.lockState = target == 1 ? CursorLockMode.Locked : CursorLockMode.None;
		}

		#endregion

		#region IEnumerators

		public Coroutine FadeOut(float time)
		{
			return Fade(1, time);
		}

		public Coroutine FadeIn(float time)
		{
			return Fade(0, time);
		}

		private Coroutine Fade(int target, float time)
		{
			if (currentlyActiveCoroutine != null) StopCoroutine(currentlyActiveCoroutine);

			currentlyActiveCoroutine = StartCoroutine(FadeRoutine(target, time));
			DisableInput(target);

			return currentlyActiveCoroutine;
		}

		private IEnumerator FadeRoutine(float target, float time)
		{
			while (!Mathf.Approximately(canvasGroup.alpha, target))
			{
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target,
					Time.deltaTime / time);
				yield return null;
			}
		}

		#endregion
	}
}