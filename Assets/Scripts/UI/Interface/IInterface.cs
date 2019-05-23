using System;

namespace Assets.Scripts.UI.Interface
{
	public interface IInterface
	{
		void UpdateStepsCounter(int maxCount, int currentCount);
		void ShowWonPanel();
		void ShowGameOverPanel();
		event Action PousePressed;
		event Action PouseReleased;
		event Action RestartGamePressed;
		event Action EndGamePressed;
	}
}
