using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Test/Boulder Test")]
public class btInterfaceLeftTestNode : btInterfaceTestNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionLeftTestNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Left Test";
	}
	
	public override string GetDescription()
	{
		return "The left test node looks to see if the bee can turn left. If it can this node suceeds, otherwise it fails.";
	}
	
	public override string GetIconMaterialName()
	{
		return "LeftTest";	
	}
}
