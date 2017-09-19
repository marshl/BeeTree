using UnityEngine;
using System.Collections;

/// <summary>
/// Loops all child nodes until all fail
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Inner/Loop")]
public class btFunctionLoopNode : btFunctionInnerNode
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
		
		bool completeCycle = false;
		
		while ( true )
		{
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
						if ( this as btFunctionRootNode != null )
						{
							continue;	
						}
						this.currentState = btFunctionNode.NODE_STATE.FAILED;
						return this.currentState;
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
			
			if ( completeCycle == true )
			{
				this.currentState = btFunctionNode.NODE_STATE.FAILED;
				return this.currentState;	
			}
			
			this.updateIndex = 0;
			completeCycle = true;
		}
	}
}
