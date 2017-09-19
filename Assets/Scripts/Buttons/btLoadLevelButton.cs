using UnityEngine;
using System.Collections;

/// <summary>
/// Loads the selected level when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Load Level")]
public class btLoadLevelButton : MonoBehaviour
{
	public int levelNumber;
	
	private void Start()
	{
		UILabel label = this.GetComponentInChildren<UILabel>();
		
		label.text = "Level " + this.levelNumber.ToString();
	}
	
	private void OnClick()
	{
		btMenuManager.instance.BeginLevelLoad( this.levelNumber );
	}
}
