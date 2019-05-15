using System;
using UnityEngine;

namespace Assets.Scripts.Utils.SwipeSystem
{
	public interface IInputController
	{
		void DetectInput();
		float GetAxis(string axisType);

		event Action<Vector2> TouchBegan;
		event Action<Vector2> PointerDown;
		event Action<Vector2> TouchEnd;
	}
}