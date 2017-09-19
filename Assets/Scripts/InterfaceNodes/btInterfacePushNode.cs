using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Action/Push")]
public class btInterfacePushNode : btInterfaceActionNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionPushNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Push";	
	}
	
	public override string GetDescription()
	{
		return "The push forward node pushes any boulder in front of the player forward by one square." +
				"If there isn't a boulder in front of the player, or the boulder cannot be pushed, then the push node fails.";	
	}
	
	public override string GetIconMaterialName()
	{
		return "Push";	
	}
}
