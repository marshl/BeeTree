using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Inner/Root")]
public class btInterfaceRootNode : btInterfaceInnerNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionRootNode();	
	}
	
	public override bool CanHaveParent()
	{
		return false;	
	}
	
	public override string GetDisplayLabel()
	{
		return "Root";
	}
	
	public override string GetDescription()
	{
		return "The root node is the root of teh behaviour tree, where all children are run from left to right.";
	}
	
	public override string GetIconMaterialName()
	{
		return "Root";	
	}
}

