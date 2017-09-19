using UnityEngine;
using System.Collections;

/// <summary>
/// Used to control the connection widget that is dragged from inner nodes
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Interface/Node Drag Widget")]
public class btInterfaceDragWidget : MonoBehaviour
{
	public btInterfaceConnector attachedConnector;
    public UIDragObject dragScript;

	public btInterfaceNode currentChild;
	public bool isBeingDragged;
	
	private void OnPress( bool isPressed )
	{
		this.isBeingDragged = isPressed;	
	}
	
	private void Update()
	{
		this.GetComponent<btAreaLimiter>().enabled = this.isBeingDragged;
	}
	
	public void OnTriggerEnter( Collider collider )
	{
		if ( this.isBeingDragged == false )
			return;
		
		btInterfaceNode receiver = collider.GetComponent<btInterfaceNode>();
		
		if ( receiver == null)
		{
			return;
		}
		
		if ( receiver != this.attachedConnector.attachedNode )
		{
			this.currentChild = receiver;
		}
	}
	
	void OnTriggerExit( Collider collider )
	{
		if ( this.isBeingDragged == false )
			return;
		
		btInterfaceNode receiver = collider.GetComponent<btInterfaceNode>();
		
		if( receiver == null )
			return;
		
		this.currentChild = null;
	}
}
