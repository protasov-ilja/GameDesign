using Assets.Scripts.Controllers;
using Assets.Scripts.Objects.Enums;
using Assets.Scripts.UI.Interface;
using Assets.Scripts.Utils.LoadedObjects;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects
{
	public sealed class LevelManager : MonoBehaviour
	{
		const string ProgressKey = "LevelProgress";

		[SerializeField]
		private PlayerController _playerController;

		[SerializeField]
		private FieldGenerator _fieldGenerator;

		private Vector3 startPosition;

		public LevelData CurrLevelData { get; set; }
		private Camera _mainCamera;

		private bool _isCoinCatched = false;
		private Coin _catchedCoin;
		private Cell _catchedCell;

		private Vector3 _previousPosition;

		private int _maxStepsCount = 0;
		private int _currentStepsCount = 0;
		private bool _isGameEnd = false;
		private bool _isGameOnPouse = false;
		private int _currentLevel = 0;

		public event Action<int, int> CounterUpdated;
		public event Action<int> GameWon;
		public event Action<int, int> GameOver;

		private IInterface _levelDisplayer;

		private void Awake()
		{
			_mainCamera = Camera.main;
			_playerController.TouchStart += StartDraging;
			_playerController.TouchContinue += ContinueDragin;
			_playerController.TouchEnd += EndDragin;
		}

		public void StartLevel(int level)
		{
			_isGameEnd = false;
			_isGameOnPouse = false;
			_currentLevel = level;
			CurrLevelData = _fieldGenerator.CreateField(level);
			_maxStepsCount = CurrLevelData.MaxStepsAmount;
			_currentStepsCount = 0;
			CounterUpdated?.Invoke(_maxStepsCount, _currentStepsCount);
			_levelDisplayer?.SetCurrentLevel(_currentLevel);
		}

		public void EndLevel()
		{
			_isGameEnd = true;
			_isGameOnPouse = true;
			_fieldGenerator.ClearField();
			_currentStepsCount = 0;
			_maxStepsCount = 0;
			CounterUpdated?.Invoke(_maxStepsCount, _currentStepsCount);
			CurrLevelData = null;
		}

		public void LinkToUI(IInterface displayer)
		{
			_levelDisplayer = displayer;
			CounterUpdated += displayer.UpdateStepsCounter;
			GameWon += displayer.ShowWonPanel;
			GameOver += displayer.ShowGameOverPanel;
			displayer.PousePressed += SetGameOnPause;
			displayer.RestartGamePressed += RestartGame;
			displayer.EndGamePressed += EndLevel;
			displayer.PouseReleased += SetGameOnPlay;
		}

		private void RestartGame()
		{
			EndLevel();
			StartLevel(_currentLevel);
		}

		private void SetGameOnPause()
		{
			_isGameOnPouse = true;
		}

		private void SetGameOnPlay()
		{
			_isGameOnPouse = false;
		}

		public void CreateLevel(LevelData data)
		{
			CurrLevelData = data;
		}

		public void StartDraging(Vector3 position)
		{
			if (_isGameEnd || _isGameOnPouse) return;

			startPosition = _mainCamera.ScreenToWorldPoint(position);

			Ray ray = _mainCamera.ScreenPointToRay(position);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1000))
			{
				var coinObj = hit.collider.gameObject;
				if (coinObj.TryToGetComponent<Coin>(out var coin))
				{
					if (coin.State != CoinState.Blocked)
					{
						_isCoinCatched = true;
						startPosition = coin.transform.position;
						_catchedCoin = coin;
						_catchedCell = coin.ParentCell;
						_previousPosition = startPosition;
					}
				}
			}
		}

		public void ContinueDragin(Vector3 position)
		{
			if (_isCoinCatched)
			{
				Ray ray = _mainCamera.ScreenPointToRay(position);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 1000))
				{
					_catchedCoin.transform.position = new Vector3(hit.point.x, hit.point.y, _catchedCoin.transform.position.z);
				}
			}
		}

		public void EndDragin(Vector3 position)
		{
			if (_isCoinCatched)
			{
				_isCoinCatched = false;
				var cell = _fieldGenerator.GetNearestCell(_catchedCoin.transform.position);
				if (cell.State == CellState.Active)
				{
					cell.Coin = _catchedCoin;
					cell.State = CellState.HasCoin;
					_catchedCoin.transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y, _catchedCoin.transform.position.z);
					_catchedCoin.ParentCell = cell;
					_catchedCell.State = CellState.Available;
					_catchedCell.Coin = null;
					_fieldGenerator.IndicateBlockedStates();
					_currentStepsCount++;
					if ((_currentStepsCount <= _maxStepsCount) && _fieldGenerator.IsGridsEqual())
					{
						var progress = GetCalculatedScore();
						PlayerPrefs.SetInt($"{ProgressKey}{_currentLevel}", progress);
						var score = PlayerPrefs.GetInt($"curentScore", 0);
						var newScore = score + progress;
						PlayerPrefs.SetInt($"curentScore", newScore);
						
						GameWon?.Invoke(progress);
						_isGameEnd = true;
					}
					else if (_currentStepsCount > _maxStepsCount)
					{
						var score = PlayerPrefs.GetInt($"curentScore", 0);
						var bestScore = PlayerPrefs.GetInt($"bestScore", 0);
						if (score > bestScore)
						{
							PlayerPrefs.SetInt($"bestScore", score);
						}

						PlayerPrefs.SetInt($"curentScore", 0);

						GameOver?.Invoke(score, bestScore);
						_isGameEnd = true;
					}
					else
					{
						CounterUpdated?.Invoke(_maxStepsCount, _currentStepsCount);
					}
				}
				else
				{
					_catchedCoin.transform.position = new Vector3(_catchedCell.transform.position.x, _catchedCell.transform.position.y, _catchedCoin.transform.position.z);
				}

				_catchedCoin = null;
			}
		}

		private int GetCalculatedScore()
		{
			if (_maxStepsCount == _currentStepsCount)
			{
				return 1;
			}
			else if (_maxStepsCount - 1 == _currentStepsCount)
			{
				return 2;
			}

			return 3;
		}
	}
}
