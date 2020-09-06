using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Health
{
	public class Health : MonoBehaviour
	{
		[SerializeField] private float healthPoints = 2;

		private float fullHealthPoints;

		public float HealthPointsInPercentage => 100 * (healthPoints / fullHealthPoints);

		private void Awake()
		{
			fullHealthPoints = healthPoints;
		}

		public void GetDamaged(int damagePoint)
		{
			healthPoints -= damagePoint;
			if (healthPoints <= 0)
				Die();
		}

		private void Die()
		{
			UnitStateManager.currentState = State.Done;
			Destroy(gameObject);
		}
	}
}