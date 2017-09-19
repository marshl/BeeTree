using UnityEngine;
using System.Collections;

/// <summary>
/// Quits the game when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Exit Game")]
public class btExitGameButton : MonoBehaviour
{
#if UNITY_WEBPLAYER
	private void Awake()
	{
		this.gameObject.SetActive( false );	
	}
#endif
	
	private void OnClick()
	{
#if UNITY_EDITOR
		Debug.Log( "Attempting application exit" );
		Debug.Break();
#endif
		Application.Quit();
	}

}
