using Assets.Scripts.Objects.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects
{
	[Serializable]
	public sealed class Cell : MonoBehaviour
	{
		[SerializeField] SpriteRenderer _render;

		public Color Color { get; set; }
		public Coin Coin { get; set; }
		public CellState State { get; set; }

		public void Initilize()
		{
			_render.color = Color;
		}
	}
}
