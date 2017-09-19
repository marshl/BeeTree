using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract inner node base
/// </summary>
[AddComponentMenu("")]
public abstract class btFunctionInnerNode : btFunctionNode 
{
	public List<btFunctionNode> children;
   
    /// The child index that is next to update
	public int updateIndex;
	
    /// <summary>
    /// Default constructor
    /// </summary>
	public btFunctionInnerNode()
	{
		children = new List<btFunctionNode>();	
	}
	
    /// <summary>
    /// Recursively builds the tree using the input interface node
    /// Children nodes are created and populated using the children of the interface node, then they recurse this function if able
    /// </summary>
    /// <param name="_interfaceNode"> The input interface node</param>
	public void AddChildNodes_r( btInterfaceNode _interfaceNode )
	{
		btInterfaceInnerNode innerInterfaceNode = _interfaceNode as btInterfaceInnerNode;
		
		if( innerInterfaceNode == null )
			return;	
		
		foreach ( btInterfaceConnection connection in innerInterfaceNode.childConnectors )
		{
			btInterfaceNode interfaceChild = connection.child;
			btFunctionNode functionChild = interfaceChild.CreateFunctionNode();
			functionChild.variable = interfaceChild.variable;
			functionChild.interfaceNode = interfaceChild;
			interfaceChild.functionNode = functionChild;
			
			this.children.Add( functionChild );
			
			btFunctionInnerNode innerFunctionNode = functionChild as btFunctionInnerNode;
			if ( innerFunctionNode != null )
			{
				innerFunctionNode.AddChildNodes_r( interfaceChild );
			}
		}
	}
	
    /// <summary>
    /// Returns a text version of this node and all child nodes
    /// </summary>
    /// <param name="depth">The tab depth which to prepend to each line (incremented on each depth)</param>
    /// <returns>The string representation of this and its children</returns>
	public override string DebugPrintState( int depth )
	{
		string str = "";
		string tabs = Tabs( depth );
		str += tabs + this.ToString();
		if ( this.children.Count > 0 )
		{
			str += tabs + "{\n";
			foreach ( btFunctionNode child in children )
			{
				str += child.DebugPrintState(depth + 1);	
			}
			str += tabs + "}\n";
		}
		return str;
	}
	
    /// <summary>
    /// Resets this node and all children
    /// </summary>
	public override void Reset()
	{
        base.Reset();
		this.updateIndex = 0;
		foreach ( btFunctionNode child in this.children )
		{
			child.Reset();
		}
	}
}
