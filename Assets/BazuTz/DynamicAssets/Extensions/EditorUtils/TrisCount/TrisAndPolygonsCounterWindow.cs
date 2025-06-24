#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace _Scripts.EditorUtils
{
    public class TrisAndPolygonsCounterWindow : EditorWindow
    {
        private TrisAndPolygonsData lastTrisAndPolygonsData;

        [MenuItem("Tools/Count Tris and Polygons")]
        public static void ShowWindow()
        {
            GetWindow<TrisAndPolygonsCounterWindow>("Tris and Polygons");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Count Tris and Polygons"))
            {
                lastTrisAndPolygonsData = ObjectTrianglePolygonCounter.CountTrisAndPolygonsForFirstSelected();
            }
            else if (GUILayout.Button("Count Tris and Polygons in childs"))
            {
                lastTrisAndPolygonsData = ObjectTrianglePolygonCounter.CountTrisAndPolygonsInChilds();
            }
            else if (GUILayout.Button("Count Tris and Polygons for all selected"))
            {
                lastTrisAndPolygonsData = ObjectTrianglePolygonCounter.CountTrisAndPolygonsForAllSelected();
            }

            if (lastTrisAndPolygonsData == null)
            {
                return;
            }

            if (lastTrisAndPolygonsData.selectedObject != String.Empty)
            {
                GUILayout.Label($"Selected Object Name: {lastTrisAndPolygonsData.selectedObject}");
            }

            GUILayout.Label($"Tris: {lastTrisAndPolygonsData.formatedTrisCount}");
            GUILayout.Label($"Poly: {lastTrisAndPolygonsData.formatedPolyCount}");

            if (lastTrisAndPolygonsData.errorText != String.Empty)
            {
                GUILayout.Label($"Error: {lastTrisAndPolygonsData.errorText}");
            }
        }
    }
}
#endif