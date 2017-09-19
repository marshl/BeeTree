using UnityEngine;
using System.Collections;

/// <summary>
/// The overarching manager for the game while in a level
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Managers/Game Manager")]
public class btGameManager : MonoBehaviour
{
	public enum GAMESTATE
	{
		IDLE,
		PLAYING,
		TILE_UPDATE,
		DEAD_PLAYER,
		LEVEL_COMPLETE,
		PAUSED,
	};
	
	// Instance
	public static btGameManager instance;
	
	// Private variables
	
	// Level options stashed for quick access
	private int levelNumber;
	private string levelName;
	private string levelDescription;
	private string levelHint;
	
	private GAMESTATE currentState = GAMESTATE.IDLE;
	private bool gameFinale = false;
	
	
	public int GetLevelNumber() { return this.levelNumber; }
	public string GetLevelName() { return this.levelName; }
	public string GetLevelDescription() { return this.levelDescription; }
	public string GetLevelHint() { return this.levelHint; }
	
	
	
	private void Awake()
	{
		if ( btGameManager.instance != null )
		{
			Debug.LogError("Multiple instances of btGameManager found");
		}	
		btGameManager.instance = this;
		
		// Find the options and stash them
		btLevelOptions levelOptions = GameObject.FindObjectOfType( typeof(btLevelOptions) ) as btLevelOptions;
		Assert.IsNotNull( levelOptions, "Level Options not found" );
		this.levelNumber = levelOptions.levelNumber;
		this.levelName = levelOptions.levelName;
		this.levelDescription = levelOptions.levelDescription;
		this.levelHint = levelOptions.levelHint;
	}
	
	private void Start()
	{
		btUIManager.instance.DisplayLevelStartDialog();	
	}
	
	private void Update()
	{
		// Main state machine
		switch ( this.currentState )
		{
			case GAMESTATE.IDLE:
			{
				break;	
			}
			case GAMESTATE.PLAYING:
			{
				btTileManager.instance.playerTile.ResetUpdateValues();
				btFunctionNode.NODE_STATE executeState = btFunctionTreeManager.instance.ExecuteTree();
				
				this.currentState = GAMESTATE.TILE_UPDATE;	
				btInterfaceTreeManager.instance.UpdateNodeGlowColour();
			
				if ( executeState == btFunctionNode.NODE_STATE.FAILED )
				{
					this.StopAndReset();
					btUIManager.instance.DisplayMessageDialog( "Tree Failure", "The tree stopped, meaning that there were no more actions that could run." );
					break;
				}
				break;
			}
			case GAMESTATE.TILE_UPDATE: // During the update, the game manager waits for the tile manager to finish
			{
				bool doneUpdating = btTileManager.instance.UpdateTiles();
				bool doneMoving = btTileManager.instance.ExecuteTileMovements();
			
				if ( doneUpdating == true && doneMoving == true )
				{
					this.currentState = GAMESTATE.PLAYING;
				}
				break;
			}
			case GAMESTATE.DEAD_PLAYER:
			{
				this.StopAndReset();
				break;
			}
			case GAMESTATE.LEVEL_COMPLETE:
			{
				this.currentState = GAMESTATE.IDLE;
				break;
			}
			default:
			{
				Debug.LogError( "Uncaught GAMESTATE: " + this.currentState );
				break;
			}
		}
	}
	
	public bool IsPlaying()
	{
		return this.currentState != GAMESTATE.IDLE;	
	}
	
	/// <summary>
	/// Builds the tree and starts movement, while locking out the interface
	/// </summary>
	public void BuildAndPlay()
	{
		if ( this.currentState != GAMESTATE.IDLE )
			return;
		
		this.currentState = GAMESTATE.PLAYING;
		
		btUIManager.instance.ShowPauseButton();
		
		btInterfaceTreeManager.instance.SetEditLock( true );
		btNodeSelectPanel.instance.SetEditLock( true );
		btUIManager.instance.ToggleInterfaceButtons( false );
		
		bool treeBuilt = btFunctionTreeManager.instance.BuildFunctionTree();
		
		if ( treeBuilt == false )
		{
			this.StopAndReset();
			btUIManager.instance.DisplayMessageDialog
			(
				"Attach Nodes to the Root",
				"You need to attach nodes to the root node."
			);
		}
	}
	
	public void StopAndReset()
	{
		if ( this.currentState == GAMESTATE.IDLE )
			return;
		
		this.currentState = GAMESTATE.IDLE;
	
		btUIManager.instance.ShowPlayButton();
		
		btInterfaceTreeManager.instance.ResetNodeGlowColour();
		btInterfaceTreeManager.instance.RemoveFunctionNodeConnections();
		btInterfaceTreeManager.instance.SetEditLock( false );
		btNodeSelectPanel.instance.SetEditLock( false );
		btUIManager.instance.ToggleInterfaceButtons( true );
		
		btTileManager.instance.ResetTiles();
		btFunctionTreeManager.instance.DestroyTree();
	}
	
	public void KillPlayer()
	{
		this.currentState = GAMESTATE.DEAD_PLAYER;
	}
	
	public void CompleteLevel()
	{
		this.currentState = GAMESTATE.IDLE;
		
		btSaveManager saveFile = new btSaveManager();
		saveFile.LoadXML();
		
		// If the player is firther than they have been before, 
		// then a special message will be played when they get back to the menu 
		// ( if it is the last level )
		int oldProgress = saveFile.levelProgress;
		if ( oldProgress > this.levelNumber )
		{
			this.gameFinale = true;
		}
		saveFile.levelProgress = Mathf.Max( oldProgress, this.levelNumber );
		saveFile.SaveXML(); // Update the save file
		
		btUIManager.instance.ShowLevelCompleteDialog();	
	}
	
	public void LoadNextLevel()
	{
		// Last level is complete, go back to menu
		if ( Application.loadedLevel >= Application.levelCount - 1)
		{
			if ( this.gameFinale == true )
			{
				GameObject tempObj = new GameObject();
				tempObj.AddComponent<btGameCompleteMenu>();
			}
			Application.LoadLevel( 0 );
		}
		else // Load next level
		{
			btLevelPopulator.LoadLevel( this.levelNumber + 1 );
		}
	}
	
	public void LoadMenu()
	{
		Application.LoadLevel( "Menu" );
	}
}
