using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Action/Move Forward")]
public class btInterfaceMoveForwardNode : btInterfaceActionNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionMoveForwardNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Move Forward";
	}
	
	public override string GetDescription()
	{
		return "The move forward node will move the player forward by one square when executed." +
				"But if the player cannot move forward, the node fails.";
	}
	
	public override string GetIconMaterialName()
	{
		return "MoveForward";	
	}
}
