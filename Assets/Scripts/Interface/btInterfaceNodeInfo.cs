using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Interface/Delete Node Button")]
public class btInterfaceNodeInfo : MonoBehaviour 
{
	public btInterfaceNode attachedNode;
	
	void OnClick()
	{
		btUIManager.instance.ShowNodeDetailPanel( this.attachedNode );
	}	
}
