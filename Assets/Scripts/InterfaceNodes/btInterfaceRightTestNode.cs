using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Test/Boulder Test")]
public class btInterfaceRightTestNode : btInterfaceTestNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionRightTestNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Right Test";
	}
	
	public override string GetDescription()
	{
		return "The right test node looks to see if the bee can turn right. If it can this node suceeds, otherwise it fails.";
	}
	
	public override string GetIconMaterialName()
	{
		return "RightTest";	
	}
}
