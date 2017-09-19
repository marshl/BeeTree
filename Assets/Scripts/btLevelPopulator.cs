using UnityEngine;
using System.Collections;

/// <summary>
/// Used to load and populate a level
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Level Populator")]
public class btLevelPopulator : MonoBehaviour
{
	// Editor variables
	public GameObject gameInterfacePrefab;
	
	public static void LoadLevel( int _levelNumber)
	{
		string levelName = "Level-" + _levelNumber;
		
		if ( _levelNumber < 0 || _levelNumber >= Application.levelCount )
		{
			Debug.LogError( "Could not find level \"" + levelName + "\"" );
			Application.LoadLevel( 0 );
			return;
		}
		
		if ( _levelNumber == 0 )
		{
			Debug.LogError( "Should not load menu using Level Populator" );
			Application.LoadLevel( 0 );
			return;
		}
		
		GameObject levelPopulator = Resources.Load( "LevelPopulator" ) as GameObject;
		if ( levelPopulator == null )
		{
			Debug.LogError( "Cannot find level populator prefab in resources folder." );
		}	
		GameObject.Instantiate( levelPopulator );

		Application.LoadLevel( _levelNumber );
		if ( Application.isLoadingLevel == false )
		{
			Debug.LogError( "Error loading level \"" + _levelNumber + "\"" );
		}
	}
	
	private void Awake()
	{
		DontDestroyOnLoad( this.gameObject );	
	}
	
	private void OnLevelWasLoaded ()
	{
		GameObject obj = new GameObject();
		btCameraFade fadeScript = obj.AddComponent<btCameraFade>();
		fadeScript.FadeIn( 1.0f );
		GameObject.Destroy( obj, 1.0f );
		
		this.CreateUserInterface();
		
		// Destroy the editor helper grid
		GameObject gridObj = GameObject.Find( "HelperGrid10x10" );
		GameObject.Destroy( gridObj );
		
		GameObject.Destroy( this.gameObject );
	}

	private void CreateUserInterface()
	{
		GameObject.Instantiate( this.gameInterfacePrefab );	
	}
}
