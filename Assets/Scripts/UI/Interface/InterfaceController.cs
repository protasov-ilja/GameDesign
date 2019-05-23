using Assets.Scripts.UI.Interface;
using System;
using TMPro;
using UnityEngine;

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

	public event Action PousePressed;
	public event Action RestartGamePressed;
	public event Action EndGamePressed;
	public event Action PouseReleased;
	public event Action LoadNextLevelPressed;

	#region Unity Methods
	#endregion

	#region Public Methods

	public void PressLoadNextLevel()
	{
		LoadNextLevelPressed?.Invoke();
		_wonPanel.SetActive(false);
		_losePanel.SetActive(false);
		_pausePanel.SetActive(false);
	}

	public void PressRestartButton()
	{
		RestartGamePressed?.Invoke();
		_wonPanel.SetActive(false);
		_losePanel.SetActive(false);
		_pausePanel.SetActive(false);
	}

	public void PressEndGameButton()
	{
		EndGamePressed?.Invoke();
		_wonPanel.SetActive(false);
		_losePanel.SetActive(false);
		_pausePanel.SetActive(false);
		gameObject.SetActive(false);
	}

	public void PressReturnToGameButton()
	{
		PouseReleased?.Invoke();
		_pausePanel.SetActive(false);
	}

	public void PressPauseButton()
	{
		PousePressed?.Invoke();
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
