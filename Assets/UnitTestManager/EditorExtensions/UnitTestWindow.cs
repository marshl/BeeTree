#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;

public class UnitTestWindow : EditorWindow 
{ 
    private UnitTestManager unitTestManager;
    
	private Vector2 currentScroll = new Vector2(0,0);
	
    [MenuItem ("Window/Unit Tests")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(UnitTestWindow));
    }
    
    private void OnEnable()
    {
        unitTestManager = new UnitTestManager();
    }

    private void OnGUI()
    {
       // GUI.skin = Resources.Load("GUISkins/Tests") as GUISkin;
        
        if (GUILayout.Button("Run Tests", GUILayout.Width(150), GUILayout.Height(40)))
        {
            unitTestManager.RunTests();
        }
		
		if (GUILayout.Button("Clear", GUILayout.Width(150), GUILayout.Height(40)))
        {
            unitTestManager.DestroyTests();
			currentScroll = new Vector2(0,0);
        }
        currentScroll = GUILayout.BeginScrollView(currentScroll);
        	//GUILayout.BeginHorizontal();
            foreach (TestItem testItem in unitTestManager.testItems)
            {
               // string style = testItem.success == null ? "Label" : (testItem.success.Value ? "Success" : "Failure");
                
                string text = testItem.name + "".PadLeft(70 - Math.Min(testItem.name.Length, 68), '.');
                if (testItem.success != null)
                {
                    text += testItem.success.Value ? "Passed!" : "Failed!";
                }
                
               // GUILayout.Label(text, style);
				GUI.contentColor = testItem.success == true ? Color.green : Color.red;
            	
				GUILayout.Label(text);
			
                if (!string.IsNullOrEmpty(testItem.message))
                {
                    GUILayout.Label(testItem.message);//, style);
                }
			
            }
        	//GUILayout.EndHorizontal();
        //GUILayout.EndVertical();
		 GUILayout.EndScrollView();
    }
}

#endif