namespace Game.Scripts.Core
{
	public static class UnitStateManager
	{
		public static State currentState { get; set; } = State.Begin;
	}

	public enum State
	{
		Begin,
		Attack,
		ActionSelectionOpened,
		SelectMoveTarget,
		SelectAttackTarget,
		Move,
		Done
	}
}