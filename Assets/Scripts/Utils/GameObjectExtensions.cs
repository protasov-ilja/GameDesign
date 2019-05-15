using UnityEngine;

public static class GameObjectExtensions
{
	public static bool TryToGetComponent<T>(this GameObject obj, out T component)
	{
		component = obj.GetComponent<T>();
		return component != null;
	}
}
