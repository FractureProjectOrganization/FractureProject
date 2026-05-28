using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class OutlineGeneratorWindow : EditorWindow
{
    private float angleThreshold = 30f;
    private float thickness = 0.02f;
    private float zOffset = 0.005f;
    
    private string materialPath = "Assets/GeneratedOutlines/Material/OutlineUnlitBlack.mat";
    
    [MenuItem("Tools/Generer Outline")]
    public static void ShowWindow()
    {
        GetWindow<OutlineGeneratorWindow>("Paramètres Outline");
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Configuration de l'Outline", EditorStyles.boldLabel);
        GUILayout.Space(10);

        angleThreshold = EditorGUILayout.Slider("Angle Minimal (°)", angleThreshold, 0f, 180f);
        
        thickness = EditorGUILayout.FloatField("Épaisseur du trait", thickness);
        zOffset = EditorGUILayout.FloatField("Décalage (Z-Offset)", zOffset);

        GUILayout.Space(20);

        if (GUILayout.Button("Générer le Mesh Outline", GUILayout.Height(40)))
        {
            GenerateAllOutlines();
        }
    }
    
    private void GenerateAllOutlines()
    {
        GameObject rootObject = Selection.activeGameObject;
        if (rootObject == null) 
        { 
            Debug.LogWarning("Aucun objet selectionné"); 
            return;
        }

        MeshFilter[] allMeshFilters = rootObject.GetComponentsInChildren<MeshFilter>();
        int objectsProcessed = 0;
        
        string saveFolder = "Assets/GeneratedOutlines/" + rootObject;
        
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        else
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(saveFolder);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }
        AssetDatabase.Refresh();
        
        string safeFolderPath = saveFolder;
        if (!safeFolderPath.EndsWith("/"))
        {
            safeFolderPath += "/";
        }

        foreach (MeshFilter filter in allMeshFilters)
        {
            Debug.Log("Prochain objet...");
            
            if (filter == null) 
            {
                continue;
            }
            if (filter.gameObject.name.EndsWith("_Outline"))
            {
                continue;
            }
            
            GameObject targetObject = filter.gameObject;
            
            string assetName = rootObject + "_" + targetObject.name + "_Outline.asset";
            string cleanAssetName = Regex.Replace(assetName, @"\s+|\([^)]*\)", "");

            string finalPath = safeFolderPath + cleanAssetName;
            
            Mesh outlineMesh = AssetDatabase.LoadAssetAtPath<Mesh>(finalPath);
        
            if (outlineMesh == null)
            {
                outlineMesh = ExtractAndGenerate(targetObject, rootObject);
                SaveOutlineMesh(targetObject, outlineMesh, finalPath);
            }
            
            if (outlineMesh != null)
                PlaceOutlineAuto(targetObject, outlineMesh);
            
            objectsProcessed++;
        }
        
        Debug.Log("Execution du tool terminée");
    }

    private Mesh ExtractAndGenerate(GameObject targetObject, GameObject rootObject)
    {
        MeshFilter meshFilter = targetObject.GetComponent<MeshFilter>();
        
        if (meshFilter == null || meshFilter.sharedMesh == null) return null;

        Mesh sourceMesh = meshFilter.sharedMesh;

        Vector3[] vertices = sourceMesh.vertices;
        int[] triangles = sourceMesh.triangles;

        Debug.Log(targetObject.name + ": Extraction réussie. Analyse en cours...");

        #region Anti Hard-Edges

        int[] vertexToPosId = new int[vertices.Length];
        Dictionary<Vector3, int> posToIdMap = new Dictionary<Vector3, int>();
        
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 p = vertices[i];
            if (!posToIdMap.ContainsKey(p))
            {
                posToIdMap[p] = i;
            }
            vertexToPosId[i] = posToIdMap[p];
        }

        #endregion
        
        Dictionary<Edge, Vector3> edgeToNormalMap = new Dictionary<Edge, Vector3>();
        
        List<Edge> finalOutlineEdges = new List<Edge>();
        
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int indexA = triangles[i];
            int indexB = triangles[i + 1];
            int indexC = triangles[i + 2];
            
            Vector3 posA = vertices[indexA];
            Vector3 posB = vertices[indexB];
            Vector3 posC = vertices[indexC];
            
            Vector3 currentFaceNormal = GetFaceNormal(posA, posB, posC);
            
            Edge[] currentTriangleEdges = new Edge[3] {
                new Edge(vertexToPosId[indexA], vertexToPosId[indexB]),
                new Edge(vertexToPosId[indexB], vertexToPosId[indexC]),
                new Edge(vertexToPosId[indexC], vertexToPosId[indexA])
            };

            foreach (Edge edge in currentTriangleEdges)
            {
                if (!edgeToNormalMap.ContainsKey(edge))
                {
                    edgeToNormalMap.Add(edge, currentFaceNormal);
                }
                else
                {
                    Vector3 neighborNormal = edgeToNormalMap[edge];
                    
                    float angleEntreFaces = Vector3.Angle(currentFaceNormal, neighborNormal);
                    
                    if (angleEntreFaces > angleThreshold)
                    {
                        finalOutlineEdges.Add(edge);
                    }
                    
                    edgeToNormalMap.Remove(edge);
                }
            }
        }
        foreach (KeyValuePair<Edge, Vector3> remainingEdge in edgeToNormalMap)
        {
            finalOutlineEdges.Add(remainingEdge.Key);
        }
        
        Debug.Log(targetObject.name + ": Analyse terminée. Calcul du mesh...");
        
        Vector3[] normals = sourceMesh.normals;
        
        List<Vector3> newVertices = new List<Vector3>();
        List<int> newTriangles = new List<int>();
        
        int vertexIndex = 0;
        
        foreach (Edge edge in finalOutlineEdges)
        {
            Vector3 posA = vertices[edge.v1];
            Vector3 posB = vertices[edge.v2];
            
            Vector3 normA = normals[edge.v1];
            Vector3 normB = normals[edge.v2];
            
            Vector3 lineDirection = (posB - posA).normalized;
            
            Vector3 rightA = Vector3.Cross(lineDirection, normA).normalized;
            Vector3 rightB = Vector3.Cross(lineDirection, normB).normalized;
            
            Vector3 offsetA = rightA * (thickness * 0.5f);
            Vector3 offsetB = rightB * (thickness * 0.5f);
            
            Vector3 p0 = posA - offsetA + (normA * zOffset);
            Vector3 p1 = posA + offsetA + (normA * zOffset);
            Vector3 p2 = posB - offsetB + (normB * zOffset);
            Vector3 p3 = posB + offsetB + (normB * zOffset);
            
            newVertices.Add(p0);
            newVertices.Add(p1);
            newVertices.Add(p2);
            newVertices.Add(p3);

            newTriangles.Add(vertexIndex + 0);
            newTriangles.Add(vertexIndex + 2);
            newTriangles.Add(vertexIndex + 1);

            newTriangles.Add(vertexIndex + 1);
            newTriangles.Add(vertexIndex + 2);
            newTriangles.Add(vertexIndex + 3);
            
            vertexIndex += 4;
        }
        
        Debug.Log(targetObject.name + ": Mesh calculé. Generation du mesh...");
        
        Mesh outlineMesh = new Mesh();
        outlineMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        outlineMesh.SetVertices(newVertices);
        outlineMesh.SetTriangles(newTriangles, 0);

        outlineMesh.RecalculateNormals();
        outlineMesh.RecalculateBounds();

        Debug.Log(targetObject.name + ": Génération terminée. Sauvegarde en cours...");

        return outlineMesh;
    }

    private static Vector3 GetFaceNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;
        
        return Vector3.Cross(side1, side2).normalized;
    }

    private void SaveOutlineMesh(GameObject targetObject, Mesh outlineMesh, string savePath)
    {
        if (outlineMesh == null) return;
        
        outlineMesh.name = Path.GetFileNameWithoutExtension(savePath);
        
        if (AssetDatabase.LoadAssetAtPath<Mesh>(savePath) != null)
        {
            AssetDatabase.DeleteAsset(savePath);
            Debug.Log(targetObject.name + ": Ancien fichier Asset supprimé");
        }

        AssetDatabase.CreateAsset(outlineMesh, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(targetObject.name + ": Fichier sauvegardé !");
    }

    private void PlaceOutlineAuto(GameObject targetObject, Mesh outlineMesh)
    {
        for (int i = targetObject.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = targetObject.transform.GetChild(i);
        
            if (child.name.Contains("Outline"))
            {
                DestroyImmediate(child.gameObject);
            
                Debug.Log(targetObject.name + ": Ancien outline GameObject supprimé de la scène.");
            }
        }
        
        GameObject outlineObject = new GameObject(outlineMesh.name);

        outlineObject.transform.SetParent(targetObject.transform, false);

        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = Vector3.one;

        MeshFilter mf = outlineObject.AddComponent<MeshFilter>();
        mf.sharedMesh = outlineMesh;

        MeshRenderer mr = outlineObject.AddComponent<MeshRenderer>();
        
        Material outlineMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        
        if (outlineMaterial != null)
        {
            mr.sharedMaterial = outlineMaterial;
        }
        else
        {
            Debug.LogWarning("Material introuvable au chemin indiqué.");
        }
        
        Undo.RegisterCreatedObjectUndo(outlineObject, "Création Outline");
        EditorSceneManager.MarkSceneDirty(targetObject.scene);
        EditorUtility.SetDirty(targetObject);
    }
    
}

public struct Edge
{
    public int v1;
    public int v2;

    public Edge(int a, int b)
    {
        v1 = Mathf.Min(a, b);
        v2 = Mathf.Max(a, b);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Edge)) return false;
        Edge other = (Edge)obj;
        return v1 == other.v1 && v2 == other.v2;
    }

    public override int GetHashCode()
    {
        return v1.GetHashCode() ^ v2.GetHashCode();
    }
}