using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Interface/Level Start Panelr")]
public class btLevelStartPanel : MonoBehaviour
{
	public UILabel headingLabel;
	public UILabel descriptionLabel;
	
	public void Awake()
	{
#if UNITY_EDITOR
		if ( this.headingLabel == null ) Debug.LogError( "Heading Label is null" );	
		if ( this.descriptionLabel == null ) Debug.LogError( "Description Label is null" );
#endif
	}
	
	public void ShowPanel()
	{
		int number = btGameManager.instance.GetLevelNumber();
		string name = btGameManager.instance.GetLevelName();
		string description = btGameManager.instance.GetLevelDescription();
		
		this.gameObject.SetActive( true );
		this.headingLabel.text = number + ": " + name;
		this.descriptionLabel.text = description;
		
		btNodeSelectPanel.instance.SetEditLock( true );
	}
	
	public void ClosePanel()
	{
		this.gameObject.SetActive( false );	
		btNodeSelectPanel.instance.SetEditLock( false );
	}
}

