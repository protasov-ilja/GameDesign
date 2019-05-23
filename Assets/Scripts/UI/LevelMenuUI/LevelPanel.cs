using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.LevelMenuUI
{
	public sealed class LevelPanel : MonoBehaviour
	{
		[SerializeField] private int _levelNumber;
		[SerializeField] private TextMeshProUGUI _levelText;

		public event Action<int> ChooseLevelButtonPressed;

		public void Initilize(int levelNumber)
		{
			_levelNumber = levelNumber;	
		}

		public void UpdatePanel(int levelNumber)
		{
			_levelText.text = levelNumber.ToString();
		}

		public void PressStartThisLevel()
		{
			ChooseLevelButtonPressed?.Invoke(_levelNumber);
		}
	}
}
