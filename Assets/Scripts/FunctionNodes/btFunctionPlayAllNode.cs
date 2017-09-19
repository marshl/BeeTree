using UnityEngine;
using System.Collections;

/// <summary>
/// AN inner node that plays all children nodes without fault
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Inner/Play All")]
public class btFunctionPlayAllNode : btFunctionInnerNode
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
					continue;
				}
				case NODE_STATE.RUNNING:
				{
					this.currentState = btFunctionNode.NODE_STATE.RUNNING;
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
