using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Enums;

using System;

using UnityEngine;

namespace Assets.Scripts.Utils.LoadedObjects
{
	[Serializable]
	public sealed class CellData
	{
		[SerializeField] private CellState _cellState;

		public Cell Cell { get; set; }

		public CellState CellState
		{
			get => _cellState;
			set => _cellState = value;
		}
	}
}
