using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Test/Boulder Test")]
public class btInterfaceBoulderTestNode : btInterfaceTestNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionBoulderTestNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Boulder Test";
	}
	
	public override string GetDescription()
	{
		return "The boulder test node looks to see if there is a boulder in front of the player. If there is, this node succeeds, otherwise it fails.";
	}
	
	public override string GetIconMaterialName()
	{
		return "BoulderTest";	
	}
	
	public override bool UsesVariable()
	{
		return true;
	} 
}
