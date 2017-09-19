using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Handles progress storing and loading
/// </summary>
public class btSaveManager
{
#if UNITY_EDITOR
	private static string xmlPath = "assets/xml/save.xml";
#else
	private static string xmlPath = "BeeTree_data/save.xml";
#endif

	public int levelProgress = 0;
	
	public void LoadXML()
	{
		if ( File.Exists( xmlPath ) == false )
		{
			this.SaveXML();
		}
		
		XmlDocument document = new XmlDocument();
		document.Load( xmlPath );
		XmlNode rootNode = document.FirstChild;
		XmlNode levelNode = rootNode.FirstChild;
		
		/// Level Progress
		string progressString = levelNode.Attributes["progress"].Value;
		this.levelProgress = Mathf.Max(0, int.Parse( progressString ) );
	}
	
	/// <summary>
	/// Creates a blank save file overriding the old save file if it exists
	/// </summary>
	public void SaveXML()
	{
		XmlDocument xmlDoc = new XmlDocument();
		XmlElement root = (XmlElement)xmlDoc.AppendChild( xmlDoc.CreateElement("root") );
		
		XmlElement save = (XmlElement)root.AppendChild( xmlDoc.CreateElement("level") );
		save.SetAttribute("progress", this.levelProgress.ToString() );
		xmlDoc.Save( xmlPath );
		
		FileStream fs = new FileStream( xmlPath, FileMode.Open ); 
		xmlDoc.Save( fs );
		fs.Close();
	}
}
