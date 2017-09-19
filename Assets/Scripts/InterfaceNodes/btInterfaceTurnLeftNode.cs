using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Action/Turn Left")]
public class btInterfaceTurnLeftNode : btInterfaceActionNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionTurnLeftNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Turn Left";	
	}
	
	public override string GetDescription()
	{
		return "The turn left node will turn the player left and move it one square in that directionwhen executed." +
				"But if the tile to the left of the player is impassable, the node fails.";
	}
	
	public override string GetIconMaterialName()
	{
		return "TurnLeft";	
	}
}
