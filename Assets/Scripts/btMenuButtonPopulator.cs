using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Creates buttons to access each of the levels in the game for the menu
/// </summary>
public class btMenuButtonPopulator : MonoBehaviour
{
	// Editor variables
	public GameObject levelButtonPrefab;
	
	public Vector3 startPosition;
	public float xGap;
	public float yGap;
	public int buttonsPerColumn;
	
	// Private variables
	private List<GameObject> levelButtonList;
	
	
	// Methods
	
	private void Awake()
	{
		// Make a button for each level (other than the menu)
		this.levelButtonList = new List<GameObject>();
		
		int levelCount = Application.levelCount;
		
		for ( int i = 1; i < levelCount; ++i )
		{
			GameObject button = GameObject.Instantiate( this.levelButtonPrefab ) as GameObject;
			button.transform.parent = this.transform;
			button.transform.localScale = Vector3.one;
			
			btLoadLevelButton loadScript = button.GetComponent<btLoadLevelButton>();
			loadScript.levelNumber = i;
			this.levelButtonList.Add( button );
		}
		
		this.SetButtonPositions();
	}
	
	/// <summary>
	/// Set the button positions while in the editor to make adjustments easier
	/// </summary>
#if UNITY_EDITOR
	private void Update()
	{
		this.SetButtonPositions();
	}
#endif
	
	/// <summary>
	/// Moves the buttons into their positions using the spacer variables
	/// </summary>
	private void SetButtonPositions()
	{
		int columnCount = 0;
		int buttonCount = 0;
		for ( int i = 0; i < this.levelButtonList.Count; ++i )
		{
			GameObject button = this.levelButtonList[i];
			
			button.transform.localPosition = this.startPosition 
				+ new Vector3( this.xGap * columnCount, -this.yGap * buttonCount, 0.0f );
				
			buttonCount++;
			if ( buttonCount >= this.buttonsPerColumn )
			{
				columnCount++;
				buttonCount = 0;
			}
			
		}	
	}
}
