using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Action/Move Forward")]
public class btInterfaceJumpNode : btInterfaceActionNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionJumpNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Jump";
	}
	
	public override string GetDescription()
	{
		return "The jump node will jump over a single obstalce in front of the player. If a jump is performed, then this node succeeds, otherwise it fails.";
	}
	
	public override string GetIconMaterialName()
	{
		return "Jump";	
	}
}
