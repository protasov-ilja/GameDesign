using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils.LoadedObjects
{
	[Serializable]
	public sealed class LevelData
	{
		[SerializeField] private int _maxStepsAmount;
		[SerializeField] private int _timeLimit;
		[SerializeField] private bool _isFistHasOffset;
		[SerializeField] private List<CellRow> _initialGrid;
		[SerializeField] private List<CellRow> _expectedGrid;
		[SerializeField] private bool _isFistImagRowHasOffset;

		public bool IsFirstHasOffset
		{
			get => _isFistHasOffset;
			set => _isFistHasOffset = value;
		}

		public bool IsFirstImageRowHasOffset
		{
			get => _isFistImagRowHasOffset;
			set => _isFistImagRowHasOffset = value;
		}

		public int MaxStepsAmount
		{
			get => _maxStepsAmount;
			set => _maxStepsAmount = value;
		}

		public int TimeLimit
		{
			get => _timeLimit;
			set => _timeLimit = value;
		}

		public List<CellRow> InitialGrid
		{
			get => _initialGrid;
			set => _initialGrid = value;
		}

		public List<CellRow> ExpectedGrid
		{
			get => _expectedGrid;
			set => _expectedGrid = value;
		}
	}
}
