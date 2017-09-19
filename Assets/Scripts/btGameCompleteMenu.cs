using UnityEngine;
using System.Collections;

/// <summary>
/// used to show the message when the player complete the game for the first time
/// </summary>
public class btGameCompleteMenu : MonoBehaviour 
{
	private void Awake()
	{
		DontDestroyOnLoad( this );	
	}
	
	private void OnLevelWasLoaded()
	{
		btMenuManager menuManager = GameObject.FindObjectOfType( typeof(btMenuManager) ) as btMenuManager;
		menuManager.ToggleGameCompletePanel( true );
	}
}
