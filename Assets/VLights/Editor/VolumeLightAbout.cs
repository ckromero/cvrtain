using UnityEngine;
using UnityEditor;

/*
 * VLight
 * Copyright Brian Su 2011-2016
*/
public class VolumeLightAbout : EditorWindow
{
    private Texture _logoImage;

    [MenuItem("Help/About V-Lights...", false, 1010)]
    private static void Init()
    {
        VolumeLightAbout window = EditorWindow.CreateInstance<VolumeLightAbout>();
        window.ShowUtility();

#if UNITY_5
        window.titleContent = new GUIContent("About V-Lights");
#else
        window.title = "About V-Lights";
#endif
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        GUILayout.BeginVertical();
        if (_logoImage == null)
        {
            _logoImage = Resources.Load("VLights/Logo") as Texture;
        }
        GUI.DrawTexture(new Rect(-60, -64, 256, 256), _logoImage);
        GUILayout.Space(64);
        GUILayout.FlexibleSpace();
        GUILayout.Label("Version 1.2.8");
        GUILayout.FlexibleSpace();
        GUILayout.Label("For help and more information.");
        if (GUILayout.Button("http://vlights-system.blogspot.com/"))
        {
            Application.OpenURL("http://vlights-system.blogspot.com/");
        }

        GUILayout.FlexibleSpace();
        GUILayout.Label("(c) 2011-2016 Brian Su");

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}