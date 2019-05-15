using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Enums;

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class FieldGenerator : MonoBehaviour
	{
		[Header("Links")]
		[SerializeField] private LevelsManager _levelsManager;
		[SerializeField] private GameObject _cellPrefab;
		[SerializeField] private GameObject _coinPrefab;

		[Header("Parameters")]
		[SerializeField] private int _levelNumber;

		[SerializeField] private float _stepHeight;
		[SerializeField] private float _stepWidth;

		[Header("Colors")]
		[SerializeField] private Color _activeCellColor;
		[SerializeField] private Color _availableCellColor;
		[SerializeField] private Color _blockedCellColor;
		[SerializeField] private Color _hasCoinCellColor;
		[SerializeField] private Color _simpleCoinColor;
		[SerializeField] private Color _blcokedCoinColor;
		[SerializeField] private Color _activeCoinColor;

		private List<List<Cell>> _cells = new List<List<Cell>>();

		private void Awake()
		{
			_levelsManager.LoadLevels();
			CreateField(_levelNumber);
		}

		public void CreateField(int level)
		{
			var levelData = _levelsManager.Levels[level];
			var grid = levelData.InitialGrid;
			_cells = new List<List<Cell>>();
			for (var i = 0; i < grid.Count; ++i)
			{
				var row = grid[i].CellsList;
				var listOfCells = new List<Cell>();
				for (var j = 0; j < row.Count; ++j)
				{
					var offset = 0f;
					if (i % 2 != 0)
					{
						offset += _cellPrefab.transform.lossyScale.x / 2;
					}

					var prefabPosition = _cellPrefab.transform.position;
					var cellPostion = new Vector3(prefabPosition.x + j * _stepWidth + offset, prefabPosition.y - i * _stepHeight, prefabPosition.z);
					var cell = Instantiate(_cellPrefab, cellPostion, Quaternion.identity).GetComponent<Cell>();
					cell.State = row[j];
					Debug.Log($"{i} {cell.State}");
					if (cell.State == CellState.HasCoin)
					{
						var coinPostion = cellPostion + Vector3.back * 0.2f;
						cell.Coin = Instantiate(_coinPrefab, coinPostion, Quaternion.identity).GetComponent<Coin>();
						cell.Coin.ParentCell = cell;
					}

					listOfCells.Add(cell);
				}

				_cells.Add(listOfCells);
			}

			IndicateBlcokedState();
		}

		public void IndicateBlcokedState()
		{
			for (var i = 0; i < _cells.Count; ++i)
			{
				for (var j = 0; j < _cells[i].Count; ++j)
				{
					var cell = _cells[i][j];
					cell.Color = _blockedCellColor;
					if (cell.State == CellState.HasCoin)
					{
						var isCoinBlocked = DetectIntersaction(CellState.HasCoin, i, j, true);
						if (isCoinBlocked)
						{
							cell.Coin.State = CoinState.Blocked;
							cell.Coin.Color = _blcokedCoinColor;
						}
						else
						{
							cell.Coin.State = CoinState.Available;
							cell.Coin.Color = _activeCoinColor;
						}

						cell.Coin.Initilize();
					}
					else if (cell.State != CellState.Blocked)
					{
						var isNextToCoin = DetectIntersaction(CellState.HasCoin, i, j, false);
						if (isNextToCoin)
						{
							cell.State = CellState.Active;
							cell.Color = _activeCellColor;
						}
						else
						{
							cell.State = CellState.Available;
							cell.Color = _availableCellColor;
						}
					}

					cell.Initilize();
				}
			}
		}

		private bool DetectIntersaction(CellState expectedState, int row, int cell, bool isNeedAll)
		{
			var coeff = cell % 2 == 0 ? -1 : 1;
			var intersectionCounter = 0;
			var availableIntersaction = 0;

			// i - 1
			if (row - 1 >= 0)
			{
				if (cell < _cells[row - 1].Count)
				{
					availableIntersaction++;
					if (_cells[row - 1][cell].State == expectedState)
					{
						intersectionCounter++;
					}
				}

				if (coeff >= 0 && cell + coeff < _cells[row - 1].Count)
				{
					availableIntersaction++;
					if (_cells[row - 1][cell + coeff].State == expectedState)
					{
						intersectionCounter++;
					}
				}
				else if (coeff < 0 && cell + coeff >= 0)
				{
					availableIntersaction++;
					if (_cells[row - 1][cell + coeff].State == expectedState)
					{
						intersectionCounter++;
					}
				}
			}

			// i
			if (cell + 1 < _cells[row].Count)
			{
				availableIntersaction++;
				if (_cells[row][cell + 1].State == expectedState)
				{
					intersectionCounter++;
				}
			}

			if (cell - 1 >= 0)
			{
				availableIntersaction++;
				if (_cells[row][cell - 1].State == expectedState)
				{
					intersectionCounter++;
				}
			}

			// i + 1
			if (row + 1 < _cells.Count)
			{
				if (cell < _cells[row + 1].Count)
				{
					availableIntersaction++;
					if (_cells[row + 1][cell].State == expectedState)
					{
						intersectionCounter++;
					}
				}

				if (coeff >= 0 && cell + coeff < _cells[row + 1].Count)
				{
					availableIntersaction++;
					if (_cells[row + 1][cell + coeff].State == expectedState)
					{
						intersectionCounter++;
					}
				}
				else if (coeff < 0 && cell + coeff >= 0)
				{
					availableIntersaction++;
					if (_cells[row + 1][cell + coeff].State == expectedState)
					{
						intersectionCounter++;
					}
				}
			}

			if (isNeedAll && intersectionCounter == availableIntersaction)
			{
				return true;
			}

			if (!isNeedAll && intersectionCounter > 0)
			{
				return true;
			}

			return false;
		}

		public Cell GetNearestCell(Vector3 coinPosition)
		{
			var minDistance = float.MaxValue;
			var cellRowIndex = 0;
			var cellCollIndex = 0;
			for (var i = 0; i < _cells.Count; ++i)
			{
				for (var j = 0; j < _cells[i].Count; ++j)
				{
					var cellPos = _cells[i][j].transform.position;
					var distance = Vector3.Distance(cellPos, coinPosition);
					if (distance < minDistance)
					{
						cellCollIndex = j;
						cellRowIndex = i;
						minDistance = distance;
					}
				}
			}

			return _cells[cellRowIndex][cellCollIndex];
		}
	}
}
