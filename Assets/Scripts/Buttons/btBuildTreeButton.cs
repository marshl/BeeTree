using UnityEngine;
using System.Collections;

/// <summary>
/// The play button
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Buttons/Build Tree")]
public class btBuildTreeButton : MonoBehaviour
{
	private void OnClick()
	{
		btGameManager.instance.BuildAndPlay();
	}
}
