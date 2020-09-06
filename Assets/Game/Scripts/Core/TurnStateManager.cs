using System;
using System.Collections;
using Game.Scripts.World;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Core
{
	public class TurnStateManager : MonoBehaviour
	{
		private static TurnStateManager _instance;
		public static TurnStateManager Instance => _instance;
		public TurnState currentTurn { get; set; } = TurnState.Player1;
		private BackdropFader backdropFader;
		private Text currentTurnText;
		
		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
			}
			else
			{
				DontDestroyOnLoad(gameObject);
				_instance = this;
			}

			backdropFader = FindObjectOfType<BackdropFader>();
			currentTurnText = backdropFader.GetComponentInChildren<Text>();
		}

		private IEnumerator Start()
		{
			backdropFader.FadeOutImmediately();
			yield return new WaitForSeconds(2f);
			backdropFader.FadeIn(1f);
		}

		public void SwitchTurn()
		{
			switch (currentTurn)
			{
				case TurnState.Player1:
					currentTurn = TurnState.Player2;
					currentTurnText.text = "Player 2 Turn";
					break;
				case TurnState.Player2:
					currentTurn = TurnState.Player1;
					currentTurnText.text = "Player 1 Turn";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			StartCoroutine(Transition());
		}

		private IEnumerator Transition()
		{
			yield return backdropFader.FadeOut(1f);
			yield return new WaitForSeconds(2f);
			backdropFader.FadeIn(1f);
		}
	}

	public enum TurnState
	{
		Player1,
		Player2
	}
}