using Assets.Scripts.Objects.Enums;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Utils.LoadedObjects
{
	[Serializable]
	public sealed class CellRow
	{
		[SerializeField] private List<CellState> _cellsRow;

		public List<CellState> CellsList
		{
			get => _cellsRow;
			set => _cellsRow = value;
		}

		public int Count => _cellsRow.Count;

		public CellState this[int index]
		{
			get => _cellsRow[index];
			set => _cellsRow[index] = value;
		}
	}
}
