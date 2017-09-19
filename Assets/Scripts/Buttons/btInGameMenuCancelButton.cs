using UnityEngine;
using System.Collections;

/// <summary>
/// Disables the game menu when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Game Menu Cancel")]
public class btInGameMenuCancelButton : MonoBehaviour
{
	private void OnClick()
	{
		btUIManager.instance.ToggleInGameMenuDialog( false );
	}
}
