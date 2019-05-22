using Assets.Scripts.Controllers;
using Assets.Scripts.Objects.Enums;
using Assets.Scripts.Utils.LoadedObjects;
using UnityEngine;

namespace Assets.Scripts.Objects
{
	public sealed class LevelManager : MonoBehaviour
	{
		[SerializeField]
		private PlayerController _playerController;

		[SerializeField]
		private FieldGenerator _fieldGenerator;

		private Vector3 startPosition;

		private LevelData CurrLevelData { get; set; }
		private Camera _mainCamera;

		private bool _isCoinCatched = false;
		private Coin _catchedCoin;
		private Cell _catchedCell;

		private Vector3 _previousPosition; 

		private void Awake()
		{
			_mainCamera = Camera.main;
			_playerController.TouchStart += StartDraging;
			_playerController.TouchContinue += ContinueDragin;
			_playerController.TouchEnd += EndDragin;
		}

		public void CreateLevel(LevelData data)
		{
			CurrLevelData = data;
		}

		public void StartDraging(Vector3 position)
		{
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
				}
				else
				{
					_catchedCoin.transform.position = new Vector3(_catchedCell.transform.position.x, _catchedCell.transform.position.y, _catchedCoin.transform.position.z);
				}

				_catchedCoin = null;
			}
		}
	}
}
