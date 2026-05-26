using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

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

        foreach (MeshFilter filter in allMeshFilters)
        {
            if (filter == null) 
            {
                continue;
            }
            if (filter.gameObject.name.EndsWith("_Outline"))
            {
                continue;
            }
            Debug.Log("Prochain objet...");

            ExtractAndGenerate(filter.gameObject);
            objectsProcessed++;
        }
        
        Debug.Log("Execution du tool terminée");
    }

    private void ExtractAndGenerate(GameObject targetObject)
    {
        MeshFilter meshFilter = targetObject.GetComponent<MeshFilter>();
        
        if (meshFilter == null || meshFilter.sharedMesh == null) return;

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
        outlineMesh.name = targetObject.name + "_Outline";

        outlineMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        outlineMesh.SetVertices(newVertices);
        outlineMesh.SetTriangles(newTriangles, 0);

        outlineMesh.RecalculateNormals();
        outlineMesh.RecalculateBounds();

        Debug.Log(targetObject.name + ": Génération terminée. Sauvegarde en cours...");
        
        string saveFolder = "Assets/GeneratedOutlines/" + sourceMesh.name;
        
        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
            AssetDatabase.Refresh(); 
        }
        
        string safeFolderPath = saveFolder;
        if (!safeFolderPath.EndsWith("/"))
        {
            safeFolderPath += "/";
        }
        
        string fileAssetPath = safeFolderPath + outlineMesh.name + ".asset";
        
        if (AssetDatabase.LoadAssetAtPath<Mesh>(fileAssetPath) != null)
        {
            AssetDatabase.DeleteAsset(fileAssetPath);
            Debug.Log(targetObject.name + ": Ancien fichier Asset supprimé");
        }

        AssetDatabase.CreateAsset(outlineMesh, fileAssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(targetObject.name + ": Fichier sauvegardé !");
        
        Transform existingChild = targetObject.transform.Find(outlineMesh.name);
        
        if (existingChild != null)
        {
            DestroyImmediate(existingChild.gameObject);
            Debug.Log(targetObject.name + ": Ancien outline GameObject supprimé de la scène.");
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
    }

    private static Vector3 GetFaceNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;
        
        return Vector3.Cross(side1, side2).normalized;
    }
    
}