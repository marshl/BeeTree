using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Interface/Node Variable Slider")]
public class btNodeVariableSlider : MonoBehaviour 
{
	public UISlider slider;
	public UILabel nodeVariableDescription;
	public UILabel nodeVariableCurrent;
	public UILabel nodeVariableMin;
	public UILabel nodeVariableMax;
	
	public void ShowVariableDetails( btInterfaceNode _node )
	{
		int minValue = _node.GetMinVariable();
		int maxValue = _node.GetMaxVariable();
		
		this.nodeVariableMin.text = minValue.ToString();
		this.nodeVariableMax.text = maxValue.ToString();
		
		this.slider.numberOfSteps = maxValue - minValue;
		this.slider.sliderValue = _node.variable;
		
		this.nodeVariableDescription.text = _node.GetVariableLabel();
	}
	
	public void OnSliderChange()
	{
		btUIManager.instance.currentNode.variable = (int)this.slider.sliderValue;
	}
	
	/*[TestAttribute]
	public bool TestMeBro()
	{
		//Assert.IsTrue(false);
		//throw new System.Exception("Shit");
		Assert.IsTrue(false);
		return true;
	}*/
}
