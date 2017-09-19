using UnityEngine;
using System.Collections;

public class btMenuCompleteButton : MonoBehaviour {

	private void OnClick()
	{
		btMenuManager.instance.ToggleGameCompletePanel( false );	
	}
}
