#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace _Scripts.EditorUtils
{
    [InitializeOnLoad]
    public class SceneInfoDisplay
    {
        private static bool showTrisCount = false;
        private static TrisAndPolygonsData trisAndPolygonsData;
        
        static SceneInfoDisplay()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            Selection.selectionChanged += SelectionChanged;
        }

        private static void SelectionChanged()
        {
            trisAndPolygonsData = ObjectTrianglePolygonCounter.CountTrisAndPolygonsForAllSelected();
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();
            int width = CalculateMaxRequiredWidth();
            Rect canvasRect = new Rect(50, 10, width, 100);

            GUILayout.BeginArea(canvasRect);
            GUILayout.BeginVertical("Box");
            
            showTrisCount = GUILayout.Toggle(showTrisCount, "Realtime Tris and Polygons");
            
            if (showTrisCount)
            {
                ShowTrisAndPologons();
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();

            Handles.EndGUI();
        }

        private static int CalculateMaxRequiredWidth()
        {
            int requiredWidth = 180;
            if (trisAndPolygonsData == null)
            {
                return requiredWidth;
            }

            if (trisAndPolygonsData.selectedObject != String.Empty)
            {
                Vector2 size = GUI.skin.label.CalcSize(new GUIContent($"Selected Object Name: {trisAndPolygonsData.selectedObject}"));
                if (size.x > requiredWidth)
                {
                    requiredWidth = (int)size.x + 10;
                }
            }
            
            if (trisAndPolygonsData.errorText != String.Empty)
            {
                Vector2 size = GUI.skin.label.CalcSize(new GUIContent($"Error: {trisAndPolygonsData.errorText}"));
                if (size.x > requiredWidth)
                {
                    requiredWidth = (int)size.x + 10;
                }
            }
            
            return requiredWidth;
        }

        private static void ShowTrisAndPologons()
        {
            if (trisAndPolygonsData == null)
            {
                return;
            }
            
            if (trisAndPolygonsData.selectedObject != String.Empty)
                GUILayout.Label($"Selected Object Name: {trisAndPolygonsData.selectedObject}");

            GUILayout.Label($"Tris: {trisAndPolygonsData.formatedTrisCount}");
            GUILayout.Label($"Poly: {trisAndPolygonsData.formatedPolyCount}");

            if (trisAndPolygonsData.errorText != String.Empty)
                GUILayout.Label($"Error: {trisAndPolygonsData.errorText}");
        }

        
    }
}
#endif