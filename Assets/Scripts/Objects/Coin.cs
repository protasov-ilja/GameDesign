using Assets.Scripts.Objects.Enums;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Objects
{
	public sealed class Coin : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/
	{
		[SerializeField] SpriteRenderer _render;

		public Color Color { get; set; }
		public CoinState State { get; set; }
		public Cell ParentCell { get; set; }

		//public event Action<Vector3> DrugBegan;
		//public event Action<Vector3> DrugEnd;

		//private bool isDragging = false;

		public void Initilize()
		{
			_render.color = Color;
		}

		//void OnMouseUp()
		//{
		//	isDragging = false;
		//}

		//public void OnBeginDrag(PointerEventData eventData)
		//{
		//	isDragging = true;
		//	var newPosition = eventData.pointerCurrentRaycast.worldPosition;
		//	transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
		//}

		//public void OnDrag(PointerEventData eventData)
		//{
		//	if (isDragging)
		//	{
		//		var newPosition = eventData.pointerCurrentRaycast.worldPosition;
		//		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
		//	}
		//}

		//public void OnEndDrag(PointerEventData eventData)
		//{
		//	if (isDragging)
		//	{
		//		var newPosition = eventData.pointerCurrentRaycast.worldPosition;
		//		transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
		//		isDragging = false;
		//	}
		//}
	}
}
