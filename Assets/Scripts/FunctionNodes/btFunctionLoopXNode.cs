using UnityEngine;
using System.Collections;

/// <summary>
/// Loops all child nodes until all fail
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Function Nodes/Inner/Loop")]
public class btFunctionLoopXNode : btFunctionInnerNode
{
	private int loopCount = 0;
	
	public override NODE_STATE TreeExecute()
	{
		if ( this.children.Count == 0
		  || this.updateIndex < 0
		  || this.updateIndex >= this.children.Count )
		{
			this.currentState = btFunctionNode.NODE_STATE.FAILED;
			return this.currentState;
		}
	
		for ( ; loopCount < this.variable; ++loopCount )
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
		}
		
		this.currentState = btFunctionNode.NODE_STATE.FAILED;
		return this.currentState;
	}
	
	public override void Reset()
	{
		base.Reset ();
		this.loopCount = 0;
	}
}
