using Assets.Scripts.Objects.Enums;

using UnityEngine;

namespace Assets.Scripts.Objects
{
	public sealed class Coin : MonoBehaviour
	{
		[SerializeField] SpriteRenderer _render;

		public Color Color { get; set; }
		public CoinState State { get; set; }
		public Cell ParentCell { get; set; }

		public void Initilize()
		{
			_render.color = Color;
		}
	}
}
