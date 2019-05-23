using Assets.Scripts;
using Assets.Scripts.Objects;
using UnityEngine;

namespace Assers.Scripts
{
	public class GameController : MonoBehaviour
	{
		#region Editor Fields
		[SerializeField] private UIManager _uIManager;
		[SerializeField] private LevelManager _levelManager;
		[SerializeField] private LevelsManager _levelsManager;

		[Header("Current Loading Level")]
		[SerializeField] private int _currentLevel;

		#endregion

		#region Private Fields
		#endregion

		#region Public Fields
		#endregion

		#region Unity Methods
		private void Awake()
		{
			_uIManager.LevelsMenu.LinkToGameManager(this);
			_levelManager.LinkToUI(_uIManager.Interface);
			_uIManager.Interface.LoadNextLevelPressed += LoadNextLevel;
			_uIManager.MainMenu.StartGameButtonPressed += StartGame;
			//DataSaver.DataSave(1);
		}
		#endregion

		#region Public Methods
		public void StartLevel(int level)
		{
			_currentLevel = level;
			_levelManager.StartLevel(level);
		}

		public void LoadNextLevel()
		{
			_currentLevel++;
			if (_currentLevel >= _levelsManager.Levels.Count)
			{
				_currentLevel = 0;
			}

			_levelManager.EndLevel();
			_levelManager.StartLevel(_currentLevel);
		}
		#endregion

		#region Private Methods
		private void StartGame()
		{
			_levelManager.StartLevel(_currentLevel);
		}

		
		#endregion
	}
}

