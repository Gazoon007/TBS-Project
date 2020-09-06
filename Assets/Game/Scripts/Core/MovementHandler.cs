using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

namespace Game.Scripts.Core
{
	public class MovementHandler : MonoBehaviour
	{
		[SerializeField] private float movementSpeed;

		public IEnumerator MoveToNode(Unit.Unit unit, GraphNode node)
		{
			var path = ABPath.Construct(unit.transform.position, (Vector3) node.position);

			path.traversalProvider = unit.TraversalProvider;

			// Schedule the path for calculation
			AstarPath.StartPath(path);

			// Wait for the path calculation to complete
			yield return StartCoroutine(path.WaitForPath());

			if (path.error)
			{
				// Not obvious what to do here, but show the possible moves again
				// and let the player choose another target node
				// Likely a node was blocked between the possible moves being
				// generated and the player choosing which node to move to
				Debug.LogError("Path failed:\n" + path.errorLog);
				UnitStateManager.currentState = State.SelectMoveTarget;
				
				var pathGeneration = GetComponent<PathGenerationHandler>();
				pathGeneration.GeneratePossibleMoves(unit);
				yield break;
			}

			// Set the target node so other scripts know which
			// node is the end point in the path
			unit.TargetNode = path.path[path.path.Count - 1];

			yield return StartCoroutine(MoveAlongPath(unit, path, movementSpeed));

			unit.Blocker.BlockAtCurrentPosition();

			// Select a new unit to move
			UnitStateManager.currentState = State.Done;
		}

		/// <summary>Interpolates the unit along the path</summary>
		static IEnumerator MoveAlongPath(Unit.Unit unit, ABPath path, float speed)
		{
			if (path.error || path.vectorPath.Count == 0)
				throw new ArgumentException("Cannot follow an empty path");

			// Very simple movement, just interpolate using a catmull rom spline
			float distanceAlongSegment = 0;
			for (var i = 0; i < path.vectorPath.Count - 1; i++)
			{
				var p0 = path.vectorPath[Mathf.Max(i - 1, 0)];
				// Start of current segment
				var p1 = path.vectorPath[i];
				// End of current segment
				var p2 = path.vectorPath[i + 1];
				var p3 = path.vectorPath[Mathf.Min(i + 2, path.vectorPath.Count - 1)];

				var segmentLength = Vector3.Distance(p1, p2);

				while (distanceAlongSegment < segmentLength)
				{
					var interpolatedPoint =
						AstarSplines.CatmullRom(p0, p1, p2, p3, distanceAlongSegment / segmentLength);
					unit.transform.position = interpolatedPoint;
					yield return null;
					distanceAlongSegment += Time.deltaTime * speed;
				}

				distanceAlongSegment -= segmentLength;
			}

			unit.transform.position = path.vectorPath[path.vectorPath.Count - 1];
		}
	}
}
