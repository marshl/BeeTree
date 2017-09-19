using UnityEngine;
using System.Collections;

/// <summary>
///  Loads the menu when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Load Menu")]
public class btLoadMenuButton : MonoBehaviour {

	private void OnClick()
	{
		btGameManager.instance.LoadMenu();		
	}
}
