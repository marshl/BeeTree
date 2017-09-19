using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/BeeTree/UI Nodes/Base Node")]
public abstract class btInterfaceNode : MonoBehaviour
{
	public int variable;
	
	// Editor Variables
	public btInterfaceConnection parentConnector;
	public btInterfaceReceiver receiver;
	public btInterfaceTreeManager.INODE_TYPE nodeType;
	public btInterfaceIndexDisplay indexDisplay;
	public UIDragObject dragScript;
	public btInterfaceDeleteNode deleteButton;
	public UITexture glowTexture;
	public UITexture dotTexture;
	
	// Public Variables
	public bool isBeingDragged;
	
	public btFunctionNode functionNode;
	
	public void OnPress( bool _isPressed )
	{
		this.isBeingDragged = _isPressed;
	}
	
	public virtual void Update()
	{
		//this.glowTexture.enabled = this.isBeingDragged;
		
		if ( this.isBeingDragged == true )
		{
			if ( this.parentConnector != null )
			{
				this.parentConnector.parent.invalidChildIndices = true;	
			}
		}	
		
		btInterfaceInnerNode innerNode = this as btInterfaceInnerNode;
		if ( innerNode != null
		&& innerNode.invalidChildIndices == true )
		{
			innerNode.ResetChildIndices();
		}
		
		this.dotTexture.enabled = (this.parentConnector != null);

	}
	
	public void UnlockEditting()
	{
		SetLock( false );
	}
	
	public void LockEditting()
	{
		SetLock( false );
	}
	
	public void SetLock( bool _locked )
	{
		this.dragScript.enabled = !_locked;
		if ( this.deleteButton != null )
		{
			this.deleteButton.GetComponent<Collider>().enabled = !_locked;
		}
	}
	
	public void RemoveParent()
	{
		if(this.parentConnector == null)
		{
			return;
		}
		
		this.parentConnector.parent.invalidChildIndices = true;
		this.parentConnector.parent.childConnectors.Remove( this.parentConnector );
		GameObject.Destroy( this.parentConnector.gameObject );
		this.parentConnector = null;
		
		this.indexDisplay.SetIndex(-1);
	}
	
	public virtual bool CanHaveParent()
	{
		return true;	
	}
	
	public void SetIndex( int _index)
	{
		this.indexDisplay.SetIndex( _index );
	}
	
	
	// Background Glow

	public void SetGlowColour( Color _colour )
	{
		this.ShowGlow();
		this.glowTexture.color = _colour;
	}
	
	public void SetFailureGlow()
	{
		this.SetGlowColour( Color.red );
	}
	
	public void SetSuccessGlow()
	{
		this.SetGlowColour( Color.green );	
	}
	
	public void SetWorkingGlow()
	{
		this.SetGlowColour( Color.yellow );	
	}
	
	public void ResetGlow()
	{
		this.SetGlowColour( Color.white );	
	}
	
	public void HideGlow()
	{
		this.glowTexture.enabled = false;
	}
	
	public void ShowGlow()
	{
		this.glowTexture.enabled = true;	
	}
	
	public void UpdateGlowColour()
	{
		if ( this.functionNode == null )
		{
			this.ResetGlow();
			return;
		}
		
		switch ( this.functionNode.currentState )
		{
		case btFunctionNode.NODE_STATE.FAILED:
			this.SetFailureGlow();
			break;
		case btFunctionNode.NODE_STATE.NONE:
			this.ResetGlow();
			break;
		case btFunctionNode.NODE_STATE.RUNNING:
			this.SetWorkingGlow();
			break;
		case btFunctionNode.NODE_STATE.SUCCEEDED:
			this.SetSuccessGlow();
			break;
		default:
			Debug.LogError( "Uncaught function node state \"" + this.functionNode.currentState + "\"" );
			break;
		}
	}
	
	
	public virtual bool UsesVariable()
	{
		return false;	
	}
	
	public virtual int GetMinVariable()
	{
		Debug.LogError("GetMinVariable() should not be called from the base class");
		return 0;	
	}
	
	public virtual int GetMaxVariable()
	{
		Debug.LogError("GetMaxVariable() should not be called from the base class");
		return 0;	
	}
	
	public virtual string GetVariableLabel()
	{
		Debug.LogError("GetVariableDescription() should not be called from the base class");
		return "ERROR";	
	}
	
	public abstract btFunctionNode CreateFunctionNode();
	
	public abstract string GetDisplayLabel();
	
	public abstract string GetDescription();
	
	public abstract Color GetTintColour();
	
	public string GetIconMaterialPath()
	{
		return "UINodes/" + this.GetIconMaterialName();	
	}
	
	public abstract string GetIconMaterialName();
	
	
}
