using Assets.Scripts;
using Assets.Scripts.Objects;
using UnityEngine;

namespace Assers.Scripts
{
	public class GameController : MonoBehaviour
	{
		#region Editor Fields
		[SerializeField] private InterfaceController _interfaceController;
		[SerializeField] private LevelManager _levelManager;


		#endregion

		#region Private Fields
		#endregion

		#region Public Fields
		#endregion

		#region Unity Methods
		private void Awake()
		{
			_levelManager.LinkToUI(_interfaceController);
			//DataSaver.DataSave(1);
		}
		#endregion

		#region Public Methods
		#endregion

		#region Private Methods
		#endregion
	}
}

