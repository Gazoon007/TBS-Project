using System;
using Game.Scripts.Unit;
using Game.Scripts.World;
using UnityEngine;

namespace Game.Scripts.Core
{
	public class InputRayHandler : MonoBehaviour
	{
		private Unit.Unit selected;

		[SerializeField] private LayerMask layerMask;
		
		private Unit.Unit unitUnderMouse;
		private GameObject actionSelection;
		private PathGenerationHandler pathGenerationHandler;
		private bool _hasPathBeenGenerated;
		
		public Unit.Unit Selected => selected;

		private void Awake()
		{
			pathGenerationHandler = GetComponent<PathGenerationHandler>();
		}

		private void OnEnable()
		{
			PathGenerationHandler.OnPathHasBeenGenerated += PathGenerationStatusHandler;
		}

		void Update()
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (UnitStateManager.currentState == State.Begin && Input.GetKeyDown(KeyCode.Mouse0))
			{
				
				// Need refactor, violate OCP
				switch (TurnStateManager.Instance.currentTurn)
				{
					case TurnState.Player1:
						unitUnderMouse = GetByRay<PlayerOneUnit>(ray);
						break;
					case TurnState.Player2:
						unitUnderMouse = GetByRay<PlayerTwoUnit>(ray);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (unitUnderMouse != null)
				{
					actionSelection = unitUnderMouse.transform.GetChild(1).gameObject;
					actionSelection.SetActive(true);
					UnitStateManager.currentState = State.ActionSelectionOpened;
				}
			}

			if (_hasPathBeenGenerated)
			{
				HandleActionUnderPath(ray);
			}
			else if (UnitStateManager.currentState == State.SelectMoveTarget ||
			         UnitStateManager.currentState == State.SelectAttackTarget)
			{
				Select(unitUnderMouse);
				pathGenerationHandler.DestroyPossibleMoves();
				actionSelection.SetActive(false);
				pathGenerationHandler.GeneratePossibleMoves(selected);
			}
		}

		void PathGenerationStatusHandler(bool hasPathBeenGenerated)
		{
			_hasPathBeenGenerated = hasPathBeenGenerated;
		}
		
		// Need refactor, violate OCP
		void HandleActionUnderPath(Ray ray)
		{
			var moveReservable = GetByRay<MoveReservable>(ray);

			if (moveReservable != null && Input.GetKeyDown(KeyCode.Mouse0))
			{
				moveReservable.OnClick();
				pathGenerationHandler.DestroyPossibleMoves();

				if (UnitStateManager.currentState == State.SelectMoveTarget)
				{
					UnitStateManager.currentState = State.Move;
					StartCoroutine(GetComponent<MovementHandler>().MoveToNode(selected, moveReservable.Node));
				}
				else if (UnitStateManager.currentState == State.SelectAttackTarget)
				{
					UnitStateManager.currentState = State.Attack;
					GetComponent<AttackHandler>().AttackToNode(moveReservable, selected);
				}
			}
		}

		T GetByRay<T>(Ray ray) where T : class
		{
			var hit = Physics2D.Raycast(ray.origin, ray.direction, float.PositiveInfinity, layerMask);
			return hit ? hit.transform.GetComponent<T>() : null;
		}

		void Select(Unit.Unit unit)
		{
			selected = unit;
		}
	}
}