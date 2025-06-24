#if UNITY_EDITOR
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

namespace _Scripts.EditorUtils
{
    public class ObjectTrianglePolygonCounter
    {
        public static TrisAndPolygonsData CountTrisAndPolygonsForFirstSelected()
        {
            TrisAndPolygonsData lastTrisAndPolygonsData = new TrisAndPolygonsData();
            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject == null)
            {
                lastTrisAndPolygonsData.errorText = "No object selected.";
                return lastTrisAndPolygonsData;
            }


            MeshFilter[] meshFilters = new MeshFilter[1];
            meshFilters[0] = selectedObject.GetComponent<MeshFilter>();

            lastTrisAndPolygonsData = CalculateTrisForMeshes(meshFilters);
            lastTrisAndPolygonsData.selectedObject = selectedObject.name;

            return lastTrisAndPolygonsData;
        }

        public static TrisAndPolygonsData CountTrisAndPolygonsInChilds()
        {
            TrisAndPolygonsData lastTrisAndPolygonsData = new TrisAndPolygonsData();
            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject == null)
            {
                lastTrisAndPolygonsData.errorText = "No object selected.";
                return lastTrisAndPolygonsData;
            }

            MeshFilter[] meshFilters = selectedObject.GetComponentsInChildren<MeshFilter>();

            lastTrisAndPolygonsData = CalculateTrisForMeshes(meshFilters);
            lastTrisAndPolygonsData.selectedObject = selectedObject.name;
            return lastTrisAndPolygonsData;
        }

        public static TrisAndPolygonsData CountTrisAndPolygonsForAllSelected()
        {
            if (Selection.count <= 1)
            {
                return CountTrisAndPolygonsInChilds();
            }

            TrisAndPolygonsData lastTrisAndPolygonsData = new TrisAndPolygonsData();
            Object[] selectedObjects = Selection.objects;

            if (selectedObjects == null || selectedObjects.Length == 0)
            {
                lastTrisAndPolygonsData.errorText = "No objects selected.";
                return lastTrisAndPolygonsData;
            }

            List<MeshFilter> allMeshFilters = new List<MeshFilter>();

            foreach (Object selectedObject in selectedObjects)
            {
                MeshFilter[] meshFilters = selectedObject.GetComponentsInChildren<MeshFilter>();
                allMeshFilters.AddRange(meshFilters);
            }

            lastTrisAndPolygonsData = CalculateTrisForMeshes(allMeshFilters.ToArray());
            lastTrisAndPolygonsData.selectedObject = $"Parents ({Selection.count}) - Meshes ({allMeshFilters.Count})";
            return lastTrisAndPolygonsData;
        }

        private static TrisAndPolygonsData CalculateTrisForMeshes(MeshFilter[] meshFilters)
        {
            TrisAndPolygonsData lastTrisAndPolygonsData = new TrisAndPolygonsData();

            if (meshFilters.Length == 0)
            {
                lastTrisAndPolygonsData.errorText = "Selected object does not have a valid MeshFilter children";
                return lastTrisAndPolygonsData;
            }

            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    int tris = meshFilter.sharedMesh.triangles.Length / 3;
                    int polygons = meshFilter.sharedMesh.vertices.Length / 3;

                    lastTrisAndPolygonsData.trisCount += tris;
                    lastTrisAndPolygonsData.polyCount += polygons;
                }
            }

            return lastTrisAndPolygonsData;
        }
    }
}
#endif