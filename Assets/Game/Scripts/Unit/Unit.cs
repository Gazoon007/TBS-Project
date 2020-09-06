using System.Collections.Generic;
using Game.Scripts.Core;
using Pathfinding;
using UnityEngine;

namespace Game.Scripts.Unit
{
	public class Unit : VersionedMonoBehaviour
	{
		[SerializeField] private int movementPoints = 2;
		[SerializeField] private int attackRangePoints = 1;
		[SerializeField] private BlockManager blockManager;
		[SerializeField] private SingleNodeBlocker blocker;
		private GraphNode targetNode;
		private BlockManager.TraversalProvider traversalProvider;
		
		public int MovementPoints => movementPoints;
		public int AttackRangePoints => attackRangePoints;

		public BlockManager.TraversalProvider TraversalProvider => traversalProvider;

		public SingleNodeBlocker Blocker
		{
			get => blocker;
		}

		public GraphNode TargetNode
		{
			set => targetNode = value;
		}

		void Start()
		{
			blocker.BlockAtCurrentPosition();
		}

		protected override void Awake()
		{
			base.Awake();
			// Set the traversal provider to block all nodes that are blocked by a SingleNodeBlocker
			// except the SingleNodeBlocker owned by this AI (we don't want to be blocked by ourself)
			traversalProvider = new BlockManager.TraversalProvider(blockManager,
				BlockManager.BlockMode.AllExceptSelector, new List<SingleNodeBlocker>() {blocker});
		}

		private void Update()
		{
			// This is a special case to fixing bug when it reach the end of animation it doesn't get reset to the
			// origin rotation, so this is the temporary workaround
			if (UnitStateManager.currentState == State.Done)
				gameObject.transform.rotation = Quaternion.identity;
		}
	}
}