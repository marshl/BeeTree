using UnityEngine;
using System.Collections;

/// <summary>
/// The base function node used for tree execution
/// </summary>
[AddComponentMenu("")]
public abstract class btFunctionNode
{
    // The state of this node
	public enum NODE_STATE : short
	{
		SUCCEEDED,
		FAILED,
		RUNNING,
		NONE,
	}
	
	public NODE_STATE currentState = NODE_STATE.NONE;
	public btInterfaceNode interfaceNode;
	
    /// Used to store data for various node types
	public int variable;
	
	///////////////////////////
	/// METHODS
	
	public abstract NODE_STATE TreeExecute();
	
	public virtual void Reset()
	{
		this.currentState = NODE_STATE.NONE;
	}

    /// <summary>
    /// Returns a text version of this node and all child nodes
    /// </summary>
    /// <param name="depth">The tab depth which to prepend to each line (incremented on each depth)</param>
    /// <returns>The string representation of this and its children</returns>
	public virtual string DebugPrintState(int depth)
	{
		string str = "";
		str += Tabs(depth) + this.ToString() + " state: " + this.currentState + " var: " + this.variable;
		return str;
	}
	
    /// <summary>
    /// Returns a string of the input number of tabs
    /// </summary>
    /// <param name="depth"></param>
    /// <returns></returns>
	public string Tabs(int depth)
	{
		string str = "";
		for(int i = 0; i < depth; ++i)
			str += "\t";
		return str;
	}
}
