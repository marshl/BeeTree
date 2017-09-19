using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/BeeTree/Interface/Node Template")]
public class btInterfaceNodeTemplate : MonoBehaviour
{
	public Transform receiverPlaceholder;
	public Transform connectorPlaceholder;
	public Transform deleteButtonPlaceholder;
	public Transform indexDisplayPlaceholder;
	public Transform infoButtonPlaceholder;
	
	public UILabel label;
	public UITexture backgroundTexture;
	public UITexture iconTexture;
	public UITexture glowTexture;
	public UITexture dotTexture;
}
