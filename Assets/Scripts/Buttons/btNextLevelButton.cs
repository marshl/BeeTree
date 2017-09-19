using UnityEngine;
using System.Collections;

/// <summary>
/// Loads the level after the current one when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Next Level")]
public class btNextLevelButton : MonoBehaviour {

	private void OnClick()
	{
		btGameManager.instance.LoadNextLevel();	
	}
}
