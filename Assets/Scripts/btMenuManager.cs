using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the buttons and other menu paraphenalia
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Managers/Menu Manager")]
public class btMenuManager : MonoBehaviour 
{
	// Instance
	public static btMenuManager instance;
	
	public GameObject gameCompletePanel;
	
	// Private
	
	private bool loadingLevel = false;
	private btCameraFade cameraFade;
	private int levelToLoad = -1;
	
	
	private void Awake()
	{
		btMenuManager.instance = this;	
		
		Screen.SetResolution( 800, 450, false );
	}
	
	private void Start()
	{
		btSaveManager newSave = new btSaveManager();
		newSave.LoadXML();
		
		/// Find and disable all buttons that the player has not yet reached
		btLoadLevelButton[] levelButtons = Object.FindObjectsOfType( typeof(btLoadLevelButton) ) as btLoadLevelButton[];
		foreach ( btLoadLevelButton button in levelButtons )
		{
			if ( button.levelNumber > newSave.levelProgress + 1)
			{
				button.GetComponentInChildren<UISlicedSprite>().color = Color.grey;
				button.GetComponent<Collider>().enabled = false;
				button.enabled = false;
			}
		}
	}
	
	private void Update()
	{
		// Once the fade out is complete, load the level
		if ( this.loadingLevel == true )
		{
			if ( this.cameraFade.state == btCameraFade.STATE.DONE )
			{
				btLevelPopulator.LoadLevel( this.levelToLoad );	
			}
		}
	}
	
	public void BeginLevelLoad( int _levelToLoad )
	{
		if ( this.loadingLevel == true )
			return;
		
		this.loadingLevel = true;
		this.levelToLoad = _levelToLoad;
		
		// Create a new camera fade and wait for it to complete
		GameObject newObj = new GameObject();
		this.cameraFade = newObj.AddComponent<btCameraFade>();
		this.cameraFade.FadeOut( 1.0f );
		
		// Disable all of the buttons
		this.ToggleMenuButtons( false );
	}
	
	public void ToggleMenuButtons( bool _active )
	{
		List<MonoBehaviour> buttonList = new List<MonoBehaviour>();
		buttonList.AddRange( Object.FindObjectsOfType( typeof( btLoadLevelButton ) ) as MonoBehaviour[] );
		buttonList.AddRange( Object.FindObjectsOfType( typeof( btExitGameButton ) ) as MonoBehaviour[] );
		foreach ( MonoBehaviour button in buttonList )
		{
			button.GetComponent<Collider>().enabled = _active;
			button.enabled = _active;
		}	
	}
	
	public void ToggleGameCompletePanel( bool _active )
	{
		this.gameCompletePanel.SetActive( _active );	
	}
}
