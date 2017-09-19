using UnityEngine;
using System.Collections;

/// <summary>
/// Runs all children until one fails
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Inner/Sequence")]
public class btFunctionSequenceNode : btFunctionInnerNode
{
	public override NODE_STATE TreeExecute()
	{
		if ( this.children.Count == 0
			|| this.updateIndex < 0
			|| this.updateIndex >= this.children.Count )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
		
		for ( ; this.updateIndex < this.children.Count; ++this.updateIndex )
		{
			NODE_STATE childState = this.children[ this.updateIndex ].TreeExecute();
			
			switch( childState )
			{
				case NODE_STATE.SUCCEEDED:
				{
					continue;
				}
				case NODE_STATE.FAILED:
				{
					this.currentState = btFunctionNode.NODE_STATE.SUCCEEDED;
					return this.currentState;
				}
				case NODE_STATE.RUNNING:
				{
					this.currentState = btFunctionNode.NODE_STATE.FAILED;
					return this.currentState;
				}
				default: 
				{
					Debug.LogError("Uncaught BTSTATE: " + childState);
					break;
				}
			}
		}
		
		this.currentState = btFunctionNode.NODE_STATE.SUCCEEDED;
		return this.currentState;
	}
}