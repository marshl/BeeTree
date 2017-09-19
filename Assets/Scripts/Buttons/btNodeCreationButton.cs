using UnityEngine;
using System.Collections;

/// <summary>
/// Creates a node of the given type when pressed
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Create Node")]
public class btNodeCreationButton : MonoBehaviour
{
	public btInterfaceTreeManager.INODE_TYPE nodeType;
	
	private void OnClick()
	{
		btInterfaceTreeManager.instance.CreateInterfaceNode( this.nodeType );
	}
}
