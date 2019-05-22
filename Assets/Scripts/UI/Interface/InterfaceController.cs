using Assets.Scripts.UI.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour, IInterface
{
	#region Editor Fields
	[SerializeField] private GameObject _pausePanel;
	[SerializeField] private GameObject _wonPanel;
	[SerializeField] private GameObject _losePanel;
	[SerializeField] private TextMeshProUGUI _currentCount;
	[SerializeField] private TextMeshProUGUI _maxCount;
	#endregion

	#region Private Fields
	#endregion

	#region Public Fields
	#endregion

	#region Unity Methods
	#endregion

	#region Public Methods
	public void PressPauseButton()
	{
		_pausePanel.SetActive(true);
	}

	public void ShowGameOverPanel()
	{
		_losePanel.SetActive(true);
	}

	public void ShowWonPanel()
	{
		_wonPanel.SetActive(true);
	}

	public void UpdateStepsCounter(int maxCount, int currentCount)
	{
		_currentCount.text = currentCount.ToString();
		_maxCount.text = maxCount.ToString();
	}
	#endregion

	#region Private Methods
	#endregion
}
