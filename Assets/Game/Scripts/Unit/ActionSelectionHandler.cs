using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Unit
{
    public class ActionSelectionHandler : MonoBehaviour
    {
        public void Attack()
        {
            UnitStateManager.currentState = State.SelectAttackTarget;
        }
    
        public void Move()
        {
            UnitStateManager.currentState = State.SelectMoveTarget;
        }
    }
}