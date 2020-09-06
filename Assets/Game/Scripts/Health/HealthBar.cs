using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Health
{
	public class HealthBar : MonoBehaviour
	{
		private Health health;
		private Canvas canvas;
		private Slider slider;

		private void Awake()
		{
			health = GetComponent<Health>();
			canvas = transform.GetChild(2).GetComponent<Canvas>();
			slider = canvas.GetComponentInChildren<Slider>();
		}

		private void Update()
		{
			slider.value = health.HealthPointsInPercentage / 100;
			canvas.enabled = !(slider.value <= 0);
		}
	}
}