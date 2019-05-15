using Assets.Scripts.Utils.LoadedObjects;

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
	public sealed class LevelsManager : MonoBehaviour
	{
		[SerializeField] private int _levelsAmount = 1;

		private List<LevelData> _levels = new List<LevelData>();

		public List<LevelData> Levels => _levels;

		public void LoadLevels()
		{
			for (var i = 1; i <= _levelsAmount; ++i)
			{
				_levels.Add(GetLevelDataFormJson(i));
			}
		}

		public LevelData GetLevelDataFormJson(int level)
		{
			var data = DataSaver.LoadData<LevelData>($"level{level}");
			return data;
		}
	}
}
