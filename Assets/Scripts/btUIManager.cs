using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the interface for the levels
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Managers/UI Manager")]
public class btUIManager : MonoBehaviour
{
	// Instance
	public static btUIManager instance;
	
	// Editor
	public GameObject playButton;
	public GameObject stopButton;
	public btMessagePanel messagePanel;
	public GameObject levelCompleteBox;
	public GameObject nodeDetailPanel;
	public UILabel nodeDetailNameLabel;
	public UILabel nodeDetailDescriptionLabel;
	public btNodeVariableSlider nodeVariableSlider;
	public btLevelStartPanel levelStartPanel;
	public btNodeSelectPanel nodeSelectPanel;
	public GameObject inGameMenuPanel;
	
	public UIButton hintButton;
	public UIButton menuButton;
	// Public
	public btInterfaceNode currentNode;
	
	///////////
	// Unity Methods
	
	private void Awake()
	{
#if UNITY_EDITOR
		if ( btUIManager.instance != null )
		{
			Debug.LogError( "Multiple instances of btUIManager found" );	
		}
#endif
		btUIManager.instance = this;
		
#if UNITY_EDITOR
		if ( this.playButton == null ) Debug.LogError( "Play Button is null" );
		if ( this.stopButton == null ) Debug.LogError( "Stop Button is null" );
		if ( this.levelCompleteBox == null ) Debug.LogError( "Level Complete Box is null" );
		if ( this.nodeDetailPanel == null ) Debug.LogError( "Node Detail Panel is null" );
		if ( this.nodeDetailNameLabel == null ) Debug.LogError( "Node Detail Name Label is null" );
		if ( this.nodeDetailDescriptionLabel == null ) Debug.LogError( "Node Detail Description Label is null" );
		if ( this.nodeVariableSlider == null ) Debug.LogError( "Node Variable Slider is null" );
		if ( this.levelStartPanel == null ) Debug.LogError( "Level Start Panel is null" );
#endif
		
		UIButtonMessage[] objs = GameObject.FindObjectsOfType( typeof( UIButtonMessage ) ) as UIButtonMessage[];
		foreach ( UIButtonMessage obj in objs )
		{
			Debug.Log( obj, obj );	
		}
	}
	
	private void Start()
	{
		this.ShowPlayButton();
		this.levelCompleteBox.SetActive( false );
	}
	
	////////////////
	// Other Methods
	
	public void DisplayMessageDialog( string _heading, string _description )
	{
		this.messagePanel.DisplayMessage( _heading, _description );	
		btNodeSelectPanel.instance.SetEditLock( true );
		this.TogglePlayPauseButtons( false );
	}
	
	public void HideMessageDialog()
	{
		this.messagePanel.HideMessageBox();	
		btNodeSelectPanel.instance.SetEditLock( false );
		this.TogglePlayPauseButtons( true );
	}
	
	public void TogglePlayPauseButtons( bool _active )
	{
		this.playButton.GetComponent<UIButton>().enabled = _active;
		this.stopButton.GetComponent<UIButton>().enabled = _active;
	}
	
	public void ShowPauseButton()
	{
		this.playButton.SetActive( false );	
		this.stopButton.SetActive( true );
	}
	
	public void ShowPlayButton()
	{
		this.stopButton.SetActive( false );
		this.playButton.SetActive( true );	
	}
	
	public void ShowLevelCompleteDialog()
	{
		this.levelCompleteBox.SetActive( true );
		btNodeSelectPanel.instance.SetEditLock( true );
		this.TogglePlayPauseButtons( false );
	}
	
	public void ShowNodeDetailPanel( btInterfaceNode _node )
	{
		if ( _node == null )
			return;
		
		this.nodeDetailPanel.SetActive( true );
		this.currentNode = _node;
		
		string nodeName = _node.GetDisplayLabel();
		this.nodeDetailNameLabel.text = nodeName + " Node";
		
		string nodeDescription = _node.GetDescription();
		this.nodeDetailDescriptionLabel.text = nodeDescription;
		
		bool hasVariable = _node.UsesVariable();
		this.nodeVariableSlider.gameObject.SetActive( hasVariable );
		
		if ( hasVariable == true )
		{
			this.nodeVariableSlider.ShowVariableDetails( _node );
		}
		
		this.TogglePlayPauseButtons( false );
	
	}
	
	public void HideNodeDetailPanel()
	{
		this.currentNode = null;
		this.nodeDetailPanel.SetActive( false );
		this.TogglePlayPauseButtons( true );
	}
	
	public void DisplayLevelStartDialog()
	{
		this.levelStartPanel.ShowPanel();
		btNodeSelectPanel.instance.SetEditLock( true );
		this.TogglePlayPauseButtons( false );
	}
	
	public void DisplayHintDialog()
	{
		int levelNumber = btGameManager.instance.GetLevelNumber();
		string levelHint = btGameManager.instance.GetLevelHint();
		this.DisplayMessageDialog( "Hint for level " + levelNumber, levelHint );
		this.TogglePlayPauseButtons( false );
	}
	
	public void ToggleInGameMenuDialog( bool _display )
	{
		this.inGameMenuPanel.SetActive( _display );
		btNodeSelectPanel.instance.SetEditLock( _display );	
		this.TogglePlayPauseButtons( _display );
	}
	
	public void ToggleInterfaceButtons( bool _active )
	{
		this.hintButton.GetComponent<Collider>().enabled = _active;
		this.menuButton.GetComponent<Collider>().enabled = _active;
		this.hintButton.defaultColor = Color.grey;
	}
}
