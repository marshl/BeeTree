using UnityEngine;
using System.Collections;

/// <summary>
/// Actuvates the in-game menu
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Game Menu Activate")]
public class btInGameMenuActivateButton : MonoBehaviour
{
	private void OnClick()
	{
		if ( btGameManager.instance.IsPlaying() == false )
		{
			btUIManager.instance.ToggleInGameMenuDialog( true );
		}
	}
}
