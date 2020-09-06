using UnityEngine;

namespace Game.Scripts.World
{
	public class HighlightTile : MonoBehaviour
	{
		private SpriteRenderer spriteRenderer;

		private void Start()
		{
			spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
			spriteRenderer.enabled = false;
		}

		// The mesh goes red when the mouse is over it...
		void OnMouseEnter()
		{
			spriteRenderer.enabled = true;
		}

		private void OnMouseExit()
		{
			spriteRenderer.enabled = false;
		}
	}
}