using Pathfinding;
using UnityEngine;

namespace Game.Scripts.World
{
	public class MoveReservable : MonoBehaviour
	{
		private GameObject currentUnit;
		public GameObject CurrentUnit => currentUnit;

		public GraphNode Node { get; set; }

		public void OnClick()
		{
			// Something
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			if (other.IsTouchingLayers(LayerMask.GetMask("Unit")))
			{
				currentUnit = other.gameObject;
			}
		}
	}
}