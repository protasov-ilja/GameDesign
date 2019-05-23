using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Enums;
using Assets.Scripts.UI.Interface;
using Assets.Scripts.Utils.LoadedObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class FieldGenerator : MonoBehaviour
	{
		[Header("Links")]
		[SerializeField] private LevelsManager _levelsManager;
		[SerializeField] private LevelManager _levelManager;
		[SerializeField] private GameObject _cellPrefab;
		[SerializeField] private GameObject _coinPrefab;

		[SerializeField] private GameObject _uiCellPrefab;
		[SerializeField] private GameObject _uiField;


		[Header("Parameters")]
		[SerializeField] private int _levelNumber;

		[SerializeField] private float _stepHeight;
		[SerializeField] private float _stepWidth;
		[SerializeField] private float _uiStepHeight;
		[SerializeField] private float _uiStepWidth;

		[Header("Colors")]
		[SerializeField] private Color _activeCellColor;
		[SerializeField] private Color _availableCellColor;
		[SerializeField] private Color _blockedCellColor;
		[SerializeField] private Color _hasCoinCellColor;
		[SerializeField] private Color _simpleCoinColor;
		[SerializeField] private Color _blcokedCoinColor;
		[SerializeField] private Color _activeCoinColor;

		private List<List<Cell>> _cells = new List<List<Cell>>();
		private List<List<UICell>> _expectedImage = new List<List<UICell>>();

		private bool _isOffsetInFistRow;

		private void Awake()
		{
			_levelsManager.LoadLevels();
		}

		public LevelData CreateField(int level)
		{
			var levelData = _levelsManager.Levels[level];
			var dataGrid = levelData.InitialGrid;
			_isOffsetInFistRow = levelData.IsFirstHasOffset;

			var prefabPosition = _cellPrefab.transform.position;
			_cells = new List<List<Cell>>();
			for (var i = 0; i < dataGrid.Count; ++i)
			{
				var dataRow = dataGrid[i].CellsList;
				var rowOfCells = new List<Cell>();
			
				var offset = 0f;
				if (_isOffsetInFistRow)
				{
					if (i % 2 == 0) // has offset
					{
						offset += _cellPrefab.transform.lossyScale.x / 2;
					}
				}
				else
				{
					if (i % 2 != 0) // has offset
					{
						offset += _cellPrefab.transform.lossyScale.x / 2;
					}
				}

				for (var j = 0; j < dataRow.Count; ++j)
				{
					var cellPostion = new Vector3(prefabPosition.x + j * _stepWidth + offset, prefabPosition.y - i * _stepHeight, prefabPosition.z);
					var cell = Instantiate(_cellPrefab, cellPostion, Quaternion.identity).GetComponent<Cell>();
					cell.State = dataRow[j];
					if (cell.State == CellState.HasCoin)
					{
						var coinPostion = cellPostion + Vector3.back * 0.2f;
						cell.Coin = Instantiate(_coinPrefab, coinPostion, Quaternion.identity).GetComponent<Coin>();
						cell.Coin.ParentCell = cell;
					}

					rowOfCells.Add(cell);
				}

				_cells.Add(rowOfCells);
			}

			var expectedGrid = levelData.ExpectedGrid;
			_expectedImage = new List<List<UICell>>();
			var uiPrefabPosition = _uiCellPrefab.transform.position;
			var offsetGridUI = _uiField.transform as RectTransform;
			var startPointX =(_uiField.transform as RectTransform).position.x - offsetGridUI.sizeDelta.x / 4;
			var startPointY = (_uiField.transform as RectTransform).position.y + offsetGridUI.sizeDelta.y / 4;

			var uiStepWidth = (_uiCellPrefab.transform as RectTransform).sizeDelta.x / 2;
			var uiStepHeight = (_uiCellPrefab.transform as RectTransform).sizeDelta.y / 2;

			for (var i = 0; i < expectedGrid.Count; ++i)
			{
				var row = expectedGrid[i].CellsList;
				var listOfCells = new List<UICell>();
				var offset = 0f;
				if (_isOffsetInFistRow)
				{
					if (i % 2 == 0) // has offset
					{
						offset += (_uiCellPrefab.transform as RectTransform).sizeDelta.x / 4;
					}
				}
				else
				{
					if (i % 2 != 0) // has offset
					{
						offset += (_uiCellPrefab.transform as RectTransform).sizeDelta.x / 4;
					}
				}

				for (var j = 0; j < row.Count; ++j)
				{
					var cellPostion = new Vector3(startPointX + j * uiStepWidth + offset, startPointY - i * uiStepHeight, uiPrefabPosition.z);
					var cell = Instantiate(_uiCellPrefab, cellPostion, Quaternion.identity, _uiField.transform).GetComponent<UICell>();
					cell.State = row[j];
					if (row[j] == CellState.HasCoin)
					{
						cell.ImgaeColor = _simpleCoinColor;
					}
					else
					{
						cell.ImgaeColor = _blockedCellColor;
					}

					listOfCells.Add(cell);
				}

				_expectedImage.Add(listOfCells);
			}

			IndicateBlockedStates();

			return levelData;
		}

		public void ClearField()
		{
			// cleare GamePlay Field
			for (var i = 0; i < _cells.Count; ++i)
			{
				for (var j = 0; j < _cells[i].Count; ++j)
				{
					var coin = _cells[i][j].Coin?.gameObject;
					if (coin != null)
					{
						Destroy(coin);
					}

					Destroy(_cells[i][j].gameObject);
				}
			}

			_cells.Clear();

			// claer UI Image
			for (var i = 0; i < _expectedImage.Count; ++i)
			{
				for (var j = 0; j < _expectedImage[i].Count; ++j)
				{
					Destroy(_expectedImage[i][j].gameObject);
				}
			}

			_expectedImage.Clear();
		}

		public void IndicateBlockedStates()
		{
			for (var i = 0; i < _cells.Count; ++i)
			{
				for (var j = 0; j < _cells[i].Count; ++j)
				{
					var cell = _cells[i][j];
					StayCellState(cell, i, j);
					cell.Initilize();
				}
			}
		}

		private void StayCellState(Cell cell, int i, int j)
		{
			cell.Color = _blockedCellColor;
			if (cell.State == CellState.HasCoin)
			{
				var isCoinBlocked = IsCoinBlocked(i, j, CellState.HasCoin);
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
				var isNextToCoin = IsCellActive(i, j, CellState.HasCoin);
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
		}

		private bool IsCoinBlocked(int y, int x, CellState expectedCellState)
		{
			var coordinates = GenerateAvailableCellsCoordinates(y, x);
			var counter = 0;
			foreach (var cellCoordinates in coordinates)
			{
				var currentY = cellCoordinates.Y;
				var currentX = cellCoordinates.X;
				if (_cells[currentY][currentX].State == expectedCellState) counter++;
			}

			return (counter == coordinates.Count);
		}

		private bool IsCellActive(int y, int x, CellState expectedCellState)
		{
			var coordinates = GenerateAvailableCellsCoordinates(y, x);
			foreach (var cellCoordinates in coordinates)
			{
				var currentY = cellCoordinates.Y;
				var currentX = cellCoordinates.X;
				if (_cells[currentY][currentX].State == expectedCellState) return true;
			}

			return false;
		}

		private List<CellCoordinates> GenerateAvailableCellsCoordinates(int y, int x)
		{
			int coeff;
			if (_isOffsetInFistRow)
			{
				coeff = y % 2 == 0 ? 1 : -1;
			}
			else
			{
				coeff = y % 2 != 0 ? 1 : -1;
			}

			var availableCells = new List<CellCoordinates>();
			if (y - 1 >= 0)
			{
				AddAvailableCellsCoordinatesFromNearbyRow(availableCells, y - 1, x, coeff);
			}

			// i
			if (x - 1 >= 0)
			{
				availableCells.Add(new CellCoordinates { Y = y, X = x - 1 });
			}

			if (x + 1 < _cells[y].Count)
			{
				availableCells.Add(new CellCoordinates { Y = y, X = x + 1 });
			}

			// i + 1
			if (y + 1 < _cells.Count)
			{
				AddAvailableCellsCoordinatesFromNearbyRow(availableCells, y + 1, x, coeff);
			}

			return availableCells;
		}

		private void AddAvailableCellsCoordinatesFromNearbyRow(List<CellCoordinates> cellsCoordinates, int rowYCoordinates, int x, int coeff)
		{
			cellsCoordinates.Add(new CellCoordinates { Y = rowYCoordinates, X = x });

			if (coeff == -1 && x + coeff >= 0)
			{
				cellsCoordinates.Add(new CellCoordinates { Y = rowYCoordinates, X = x + coeff });
			}

			if (coeff == 1 && x + coeff < _cells[rowYCoordinates].Count)
			{
				cellsCoordinates.Add(new CellCoordinates { Y = rowYCoordinates, X = x + coeff });
			}
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

		public bool IsGridsEqual()
		{
			for (var i = 0; i < _cells.Count; ++i)
			{
				for (var j = 0; j < _cells[i].Count; ++j)
				{
					
					if (_cells[i][j].State == CellState.HasCoin)
					{
						
						return CompareGrids(i);
					}
				}
			}

			return false;
		}

		public bool CompareGrids(int y)
		{
			for (var i = 0; i < _expectedImage.Count; ++i)
			{
				var offset = 0;
				for (var j = 0; j < _expectedImage[j].Count; ++j)
				{
					if (_expectedImage[i][j].State != CellState.HasCoin) offset++;
					else
					{
						
						var x = 0;
						while (_cells[y][x].State != CellState.HasCoin)
						{
							x++;
						}
						var startX = x - offset;
						if (startX >= 0)
						{
							x = startX; 
							for (var yi = 0; yi < _expectedImage.Count; ++ yi)
							{
								if (y >= _cells.Count) return false;
								for (var xi = 0; xi < _expectedImage[yi].Count; ++xi)
								{
									//Debug.Log($"<color=green> found  {yi},{xi} : {y},{x} </color>");
									if (x >= _cells[y].Count) return false;
									//Debug.Log($"<color=green> Couunt true {yi},{xi} : {y},{x} </color>");
									if (_expectedImage[yi][xi].State == CellState.HasCoin && _expectedImage[yi][xi].State != _cells[y][x].State) return false;
									//Debug.Log($"<color=yellow> State true {yi},{xi} : {y},{x} </color>");
									x++;
								}

								x = startX;
								y++;
							}

							return true;
						}
						else
						{
							return false;
						}
					}
				}
			}

			return false;
		}
	}
}
