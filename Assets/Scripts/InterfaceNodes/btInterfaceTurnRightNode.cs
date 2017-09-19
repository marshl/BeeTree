using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Bt interface turn right node.
/// </summary>
[AddComponentMenu("Scripts/BeeTree/UI Nodes/Action/Turn Right")]
public class btInterfaceTurnRightNode : btInterfaceActionNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionTurnRightNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Turn Right";
	}
	
	public override string GetDescription()
	{
		return "The turn right node will turn the player right and move it one square in that directionwhen executed." +
				"But if the tile to the right of the player is impassable, the node fails.";
	}
	
	public override string GetIconMaterialName()
	{
		return "TurnRight";	
	}
}
