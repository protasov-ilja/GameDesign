
using Assets.Scripts.UI.MainMenuUI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField]
	private MainMenu _mainMenu;

	[SerializeField]
	private InterfaceController _interfaceController;

	[SerializeField]
	private LevelsMenu _levelsMenu;

	#endregion

	#region Private Fields
	#endregion

	#region Public Fields
	#endregion

	#region Unity Methods

	public LevelsMenu LevelsMenu => _levelsMenu;

	public MainMenu MainMenu => _mainMenu;

	public InterfaceController Interface => _interfaceController;

	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}
