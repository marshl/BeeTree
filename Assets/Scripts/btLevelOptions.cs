using UnityEngine;
using System.Collections;

/// <summary>
/// Used in every level to store the options
/// </summary>
[AddComponentMenu("Scripts/BeeTree/Level Options")]
public class btLevelOptions : MonoBehaviour
{
	public int tilesHorizontal;
	public int tilesVertical;
	
	public string levelName = "Level";
	public int levelNumber;
	public string levelDescription = "This isn't a very descriptive description.";
	public string levelHint = "Whoops, looks like your own your own in this level.";
	
	public btInterfaceTreeManager.INODE_TYPE[] allowedNodes;
}
