using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.World
{
	public class EndTurnHandler : MonoBehaviour
	{
		void Update()
		{
			if (UnitStateManager.currentState == State.Done || UnitStateManager.currentState == State.Begin)
				transform.GetChild(0).gameObject.SetActive(true);
			else
				transform.GetChild(0).gameObject.SetActive(false);
		}

		public void OnClick()
		{
			TurnStateManager.Instance.SwitchTurn();
			UnitStateManager.currentState = State.Begin;
		}
	}
}