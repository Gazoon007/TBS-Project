using System;
using System.Collections.Generic;
using Game.Scripts.World;
using Pathfinding;
using UnityEngine;

namespace Game.Scripts.Core
{
	public class PathGenerationHandler : MonoBehaviour
	{
		List<GameObject> possibleMoves = new List<GameObject>();
		[SerializeField] private GameObject[] nodePrefabs;

		public static event Action<bool> OnPathHasBeenGenerated;
		
		public void DestroyPossibleMoves()
		{
			foreach (var go in possibleMoves)
			{
				Destroy(go);
			}

			possibleMoves.Clear();
			OnPathHasBeenGenerated?.Invoke(false);
		}

		public void GeneratePossibleMoves(Unit.Unit unit)
		{
			ConstantPath path = null;
			GameObject nodePrefab = null;

			if (UnitStateManager.currentState == State.SelectAttackTarget)
			{
				path = ConstantPath.Construct(unit.transform.position, unit.AttackRangePoints * 1000 + 1);
				nodePrefab = nodePrefabs[1];
			}
			else if (UnitStateManager.currentState == State.SelectMoveTarget)
			{
				path = ConstantPath.Construct(unit.transform.position, unit.MovementPoints * 1000 + 1);
				nodePrefab = nodePrefabs[0];
				path.traversalProvider = unit.TraversalProvider;
			}

			// Schedule the path for calculation
			AstarPath.StartPath(path);

			// Force the path request to complete immediately
			// This assumes the graph is small enough that
			// this will not cause any lag
			if (path == null) return;
			path.BlockUntilCalculated();

			foreach (var node in path.allNodes)
			{
				if (node != path.startNode)
				{
					// Create a new node prefab to indicate a node that can be reached
					// NOTE: If you are going to use this in a real game, you might want to
					// use an object pool to avoid instantiating new GameObjects all the time
					var go = Instantiate(nodePrefab, (Vector3) node.position, Quaternion.identity);
					possibleMoves.Add(go);

					go.GetComponent<MoveReservable>().Node = node;
				}
			}
			
			OnPathHasBeenGenerated?.Invoke(true);
		}
	}
}
