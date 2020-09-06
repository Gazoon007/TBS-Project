using System.Collections;
using Game.Scripts.Unit;
using Game.Scripts.World;
using UnityEngine;

namespace Game.Scripts.Core
{
	public class AttackHandler : MonoBehaviour
	{
		private bool hasOpponentInitiatedCounter;

		public void AttackToNode(MoveReservable moveReservable, Unit.Unit originUnit)
		{
			var opponentUnit = moveReservable.CurrentUnit;
			Attack(originUnit, opponentUnit.GetComponent<Unit.Unit>());
		}

		public void Attack(Unit.Unit originUnit, Unit.Unit opponentUnit)
		{
			if (UnitStateManager.currentState == State.Attack)
			{
				OriginUnitFaceTowardsToOpponent(originUnit, opponentUnit);
				var unitMotion = originUnit.GetComponentInChildren<UnitMotion>();
				unitMotion.AttackMotion();

				StartCoroutine(OpponentUnitImpact(originUnit, opponentUnit,
					unitMotion.GetLengthOfCurrentClip()));
			}
		}

		private static void OriginUnitFaceTowardsToOpponent(Unit.Unit originUnit, Unit.Unit opponentUnit)
		{
			var difference = opponentUnit.transform.position - originUnit.transform.position;
			var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			originUnit.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90f);
		}

		// This is a temporary solution because if i use unity animation event, it has issue with the go reference  
		// I have no damn idea :((
		private IEnumerator OpponentUnitImpact(Unit.Unit originUnit, Unit.Unit opponentUnit, float time)
		{
			yield return new WaitForSeconds(time);

			// The magic number is temporary, will implement skillset if there's enough time
			opponentUnit.GetComponent<Health.Health>().GetDamaged(1);
			opponentUnit.GetComponentInChildren<UnitMotion>().GetDamageMotion();
		
			if (!hasOpponentInitiatedCounter)
			{
				hasOpponentInitiatedCounter = true;
				yield return new WaitForSeconds(time);
				Attack(opponentUnit, originUnit);
			} else
			{
				UnitStateManager.currentState = State.Done;
				hasOpponentInitiatedCounter = false;
			}
		}
	}
}