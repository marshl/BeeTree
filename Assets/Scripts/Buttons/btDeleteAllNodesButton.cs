using UnityEngine;
using System.Collections;

/// <summary>
/// The trash button, deleting all nodes
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Delete All Nodes")]
public class btDeleteAllNodesButton : MonoBehaviour
{
	private void OnClick()
	{
		btInterfaceTreeManager.instance.DestroyAllNodesExceptRoot();	
	}
}
