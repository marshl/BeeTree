using UnityEngine;
using System.Collections;

/// <summary>
/// Hides the node details bar when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Hide Node Details")]
public class btHideNodeDetailButton : MonoBehaviour {

	private void OnClick()
	{
		btUIManager.instance.HideNodeDetailPanel();	
	}
}
