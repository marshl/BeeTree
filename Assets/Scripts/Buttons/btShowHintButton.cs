using UnityEngine;
using System.Collections;

public class btShowHintButton : MonoBehaviour
{
	private void OnClick()
	{
		if ( btGameManager.instance.IsPlaying() == false )
		{
			btUIManager.instance.DisplayHintDialog();
		}
	}
}
