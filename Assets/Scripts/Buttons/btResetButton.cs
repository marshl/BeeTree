using UnityEngine;
using System.Collections;

/// <summary>
/// Resets the game when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Reset Game")]
public class btResetButton : MonoBehaviour
{
	private void OnClick()
	{
		btGameManager.instance.StopAndReset();
	}
}
