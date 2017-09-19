using UnityEngine;
using System.Collections;

/// <summary>
/// Deletes the node attached to this button
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Interface/Delete Node Button")]
public class btInterfaceDeleteNode : MonoBehaviour 
{
	public btInterfaceNode attachedNode;
	
	void OnClick()
	{
		btInterfaceTreeManager.instance.DeleteInterfaceNode( this.attachedNode );
	}	
}
