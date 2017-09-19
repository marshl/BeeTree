using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Inner/Play One")]
public class btInterfacePlayOneNode : btInterfaceInnerNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionPlayOneNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Play One";
	}
	
	public override string GetDescription()
	{
		return "Play One";
	}
	
	public override string GetIconMaterialName()
	{
		return "PlayOne";	
	}
}
