using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Inner/Inner Base")]
public abstract class btInterfaceInnerNode : btInterfaceNode
{
	public List<btInterfaceConnection> childConnectors;
	public btInterfaceConnector Connector;
	
	public bool invalidChildIndices = false;
	
	public void Awake()
	{
		this.childConnectors = new List<btInterfaceConnection>();	
	}
	
	public void ResetChildIndices()
	{
		while ( this.invalidChildIndices == true )
		{
			this.invalidChildIndices = false;
			for ( int i = 0; i < this.childConnectors.Count - 1; ++i )
			{
				btInterfaceConnection connection1 = this.childConnectors[i];
				btInterfaceConnection connection2 = this.childConnectors[i + 1];

				float angle1 = connection1.GetAngleFromParentToChild();
				float angle2 = connection2.GetAngleFromParentToChild();
				
				if ( angle1 > angle2)
				{
					SwapChildrenIndices( i, i + 1);
					this.invalidChildIndices = true;
				}
			}
		}
		
		for ( int i = 0; i < this.childConnectors.Count; ++i )
		{
			this.childConnectors[i].child.indexDisplay.SetIndex( i );
		}
	}
	
	public void SwapChildrenIndices( int _index1, int _index2 )
	{
		btInterfaceConnection connection1 = this.childConnectors[_index1];
		btInterfaceConnection connection2 = this.childConnectors[_index2];
		
		this.childConnectors[_index1] = connection2;
		this.childConnectors[_index2] = connection1;
	}
	
	public void RemoveChildrenConnections()
	{
		if ( this.childConnectors.Count == 0 )
		{
			return;
		}
		
		foreach ( btInterfaceConnection connection in this.childConnectors )
		{
			connection.child.SetIndex(-1);
			connection.child.parentConnector = null;
			GameObject.Destroy( connection.gameObject );
		}
		this.childConnectors.Clear();
	}
	
	public bool GetHasThisChild( btInterfaceNode _node )
	{
		if ( this.childConnectors.Count == 0 )
			return false;
		
		foreach ( btInterfaceConnection connection in this.childConnectors )
		{
			if ( connection.child == _node )
			{
				return true;	
			}
		}
		return false;
	}
	
	public bool HasNodeInChildTree( btInterfaceNode _node )
	{
		if ( this.GetHasThisChild( _node ) == true )
		{
			return true;
		}
		
		foreach ( btInterfaceConnection connection in this.childConnectors )
		{
			btInterfaceInnerNode childInnerNode = connection.child as btInterfaceInnerNode;
			if ( childInnerNode != null && childInnerNode.HasNodeInChildTree( _node ) == true )
			{
				return true;
			}
		}
		return false;
	}
	
	public int GetChildrenCount()
	{
		return this.childConnectors.Count;
	}
	
	public virtual bool CanHaveChildren()
	{
		return true;	
	}
	
	public override Color GetTintColour()
	{
		return Color.blue;
	}
}

