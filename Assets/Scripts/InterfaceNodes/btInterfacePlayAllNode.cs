using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Inner/Play All")]
public class btInterfacePlayAllNode : btInterfaceInnerNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionPlayAllNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Play All";
	}
	
	public override string GetDescription()
	{
		return "Play All";
	}
	
	public override string GetIconMaterialName()
	{
		return "PlayAll";	
	}
}
