using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private GameObject _pausePanel;

	#endregion

	#region Private Fields
	#endregion

	#region Public Fields
	#endregion

	#region Unity Methods
	#endregion

	#region Public Methods
	public void PressPauseButton()
	{
		_pausePanel.SetActive(true);
	}
	#endregion

	#region Private Methods
	#endregion
}
