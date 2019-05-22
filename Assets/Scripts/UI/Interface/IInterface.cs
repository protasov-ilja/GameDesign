namespace Assets.Scripts.UI.Interface
{
	public interface IInterface
	{
		void UpdateStepsCounter(int maxCount, int currentCount);
		void ShowWonPanel();
		void ShowGameOverPanel();
	}
}
