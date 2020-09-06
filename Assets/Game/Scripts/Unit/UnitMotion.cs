using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Scripts.Unit
{
	public class UnitMotion : MonoBehaviour
	{
		private static readonly int Attack = Animator.StringToHash("Attack");
		private static readonly int GetDamaged = Animator.StringToHash("Get Damaged");

		public void AttackMotion()
		{
			GetComponentInChildren<Animator>().SetTrigger(Attack);
		}

		public void GetDamageMotion()
		{
			GetComponentInChildren<Animator>().SetTrigger(GetDamaged);
		}

		public float GetLengthOfCurrentClip()
		{
			return GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
		}
	}
}