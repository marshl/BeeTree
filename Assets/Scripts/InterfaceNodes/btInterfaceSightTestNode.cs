using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Test/Sight Test")]
public class btInterfaceSightTestNode : btInterfaceTestNode
{
	public override btFunctionNode CreateFunctionNode()
	{
		return new btFunctionSightTestNode();	
	}
	
	public override string GetDisplayLabel()
	{
		return "Sight Test";
	}
	
	public override string GetDescription()
	{
		return "The sight test node looks ahead of the player by a certain number of tiles to see if those tiles are passable." +
				"If those tiles are, then a sight test node succeeds, otherwise it fails." +
				"The range of a sight test node can be modified below.";
	}
	
	public override string GetIconMaterialName()
	{
		return "SightTest";	
	}
	
	public override bool UsesVariable()
	{
		return true;
	} 
	
	public override int GetMinVariable()
	{
		return 1;
	}
	
	public override int GetMaxVariable()
	{
		return 5;
	}
	
	public override string GetVariableLabel()
	{
		return "Range";
	}
	
	
}
