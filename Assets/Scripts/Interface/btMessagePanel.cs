using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Interface/Message Panel")]
public class btMessagePanel : MonoBehaviour
{
	public UILabel headingLabel;
	public UILabel messageLabel;
	private bool showingMessage = false;
	
	public void Awake()
	{
#if UNITY_EDITOR
		if ( this.headingLabel == null ) Debug.LogError( "Heading Label is null" );
		if ( this.messageLabel == null ) Debug.LogError( "Message Label is null" );
#endif
	}
	
	public void DisplayMessage( string _heading, string _message )
	{
		if ( this.showingMessage == true )
		{
#if UNITY_EDITOR
			Debug.LogError( "Attempting to display message while message is still displayed" );		
#endif
			return;
		}
		
		this.headingLabel.text = _heading;
		this.messageLabel.text = _message;
		this.gameObject.SetActive( true );
		this.showingMessage = true;
	}
	
	public void HideMessageBox()
	{
		this.showingMessage = false;
		this.gameObject.SetActive( false );	
		
		
	}
}
