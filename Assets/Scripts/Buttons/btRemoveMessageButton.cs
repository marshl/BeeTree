using UnityEngine;
using System.Collections;

/// <summary>
/// Hides the message box when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Remove Message")]
public class btRemoveMessageButton : MonoBehaviour
{
	private void OnClick()
	{
		btUIManager.instance.HideMessageDialog();
	}
}
