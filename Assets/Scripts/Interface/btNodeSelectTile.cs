using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Interface/Node Select Tile")]
public class btNodeSelectTile : MonoBehaviour
{
	public UILabel label;
	public UITexture texture;
	public btInterfaceTreeManager.INODE_TYPE nodeType; 
	public UIButton button;
	public btNodeCreationButton buttonScript;
	
	public void SetLock( bool _isLocked )
	{
		this.GetComponent<Collider>().enabled = !_isLocked;
		this.button.GetComponent<Collider>().enabled = !_isLocked;
	}
}
