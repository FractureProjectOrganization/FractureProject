using UnityEngine;
using System.Collections.Generic;

public class IndependentCrowdManager : MonoBehaviour
{
    private CrowdDisplayer.CharacterData[] cpuData;
    private CrowdNode referenceNode;
    private CrowdNode[] currentPathNodes; 
    private Crowd targetCrowd; 
    
    private ComputeBuffer crowdBuffer;
    private ComputeBuffer argsBuffer;
    private ComputeBuffer waypointBuffer;
    private MaterialPropertyBlock propertyBlock;
    private Vector4[] waypointPositions; 
    
    private Mesh characterMesh;
    private Material materialTemplate;
    private Material materialInstance;
    
    private int currentWaypointCount; 
    private int characterCount;
    private float moveSpeed;
    private float totalPathLength;
    private float localOffset = 0f;

    private float dispersionDelay;
    private float dispersionDuration;
    private float dispersionDistance;
    private float blockedTimer = 0f;
    private float dispersionTimer = 0f;
    private bool isDispersing = false;

    public void Initialize(
        CrowdDisplayer.CharacterData[] characters, 
        Vector4[] pathWaypoints, 
        CrowdNode[] pathNodes, 
        int waypointCount, 
        float pathLength, 
        CrowdNode refNode, 
        Mesh mesh, 
        Material matTemplate, 
        Texture mainTex, 
        float speed,
        Crowd parentCrowd,
        float dispDelay,
        float dispDuration,
        float dispDist,
        float characterRotationY) 
    {
        cpuData = characters;
        characterCount = characters.Length;
        referenceNode = refNode;
        targetCrowd = parentCrowd;
        
        currentPathNodes = new CrowdNode[pathNodes.Length];
        System.Array.Copy(pathNodes, currentPathNodes, pathNodes.Length);
        
        waypointPositions = new Vector4[pathWaypoints.Length];
        System.Array.Copy(pathWaypoints, waypointPositions, pathWaypoints.Length);
        
        currentWaypointCount = waypointCount;
        totalPathLength = pathLength;
        
        characterMesh = mesh;
        materialTemplate = matTemplate;
        materialInstance = new Material(matTemplate);
        moveSpeed = speed;

        dispersionDelay = dispDelay;
        dispersionDuration = dispDuration;
        dispersionDistance = dispDist;

        propertyBlock = new MaterialPropertyBlock();
        if (mainTex != null) propertyBlock.SetTexture("_MainTex", mainTex);

        crowdBuffer = new ComputeBuffer(characterCount, 32);
        crowdBuffer.SetData(cpuData);
        propertyBlock.SetBuffer("_CrowdBuffer", crowdBuffer);

        waypointBuffer = new ComputeBuffer(waypointPositions.Length, 16);
        waypointBuffer.SetData(waypointPositions);
        propertyBlock.SetBuffer("_WaypointBuffer", waypointBuffer);

        propertyBlock.SetInt("_WaypointCount", currentWaypointCount);
        propertyBlock.SetFloat("_TotalPathLength", totalPathLength);
        
        propertyBlock.SetFloat("_DispersionProgress", 0f);
        propertyBlock.SetFloat("_DispersionDistance", dispersionDistance);

        argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(new uint[5] { characterMesh.GetIndexCount(0), (uint)characterCount, 0, 0, 0 });
        
        propertyBlock.SetFloat("_RotationY", characterRotationY);
    }

    void Update()
    {
        if (characterCount == 0) return;

        if (isDispersing)
        {
            dispersionTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(dispersionTimer / dispersionDuration);
            propertyBlock.SetFloat("_DispersionProgress", progress);

            Graphics.DrawMeshInstancedIndirect(characterMesh, 0, materialInstance, new Bounds(Vector3.zero, Vector3.one * 1000), argsBuffer, 0, propertyBlock);

            if (progress >= 1f)
            {
                Destroy(gameObject);
            }
            return;
        }

        float maxCurrentPos = -float.MaxValue;
        int activeCount = 0;
        
        for (int i = 0; i < characterCount; i++)
        {
            if (cpuData[i].uvRect.z == 0f) continue;
            
            activeCount++;
            float currentPos = cpuData[i].absoluteDistance + localOffset;
            if (currentPos > maxCurrentPos) maxCurrentPos = currentPos;
        }

        if (activeCount == 0)
        {
            Destroy(gameObject);
            return;
        }

        bool isBlocked = false;
        for (int i = 0; i < currentWaypointCount; i++)
        {
            if (waypointPositions[i].w >= maxCurrentPos - 0.01f)
            {
                if (currentPathNodes[i] != null && currentPathNodes[i] is StopCrowdNode stopNode && stopNode.isStopped)
                {
                    isBlocked = true;
                }
                break; 
            }
        }

        bool canMove = true;
        bool bufferDirty = false;

        if (isBlocked)
        {
            float distanceToEnd = totalPathLength - maxCurrentPos;
            float step = Time.deltaTime * moveSpeed;

            canMove = !(step >= distanceToEnd);
        }

        if (canMove)
        {
            localOffset += Time.deltaTime * moveSpeed;
            
            blockedTimer = 0f;
        }
        else
        {
            blockedTimer += Time.deltaTime;
            if (blockedTimer >= dispersionDelay)
            {
                isDispersing = true;
                return; 
            }
        }

        for (int i = 0; i < characterCount; i++)
        {
            if (cpuData[i].uvRect.z != 0f) 
            {
                if (cpuData[i].absoluteDistance + localOffset > totalPathLength)
                {
                    cpuData[i].uvRect.z = 0f; 
                    bufferDirty = true;
                }
            }
        }

        if (bufferDirty) crowdBuffer.SetData(cpuData);

        propertyBlock.SetFloat("_GlobalOffset", localOffset);

        Graphics.DrawMeshInstancedIndirect(characterMesh, 0, materialInstance, new Bounds(Vector3.zero, Vector3.one * 1000), argsBuffer, 0, propertyBlock);
    }

    void OnDisable()
    {
        if (crowdBuffer != null) crowdBuffer.Release();
        if (argsBuffer != null) argsBuffer.Release();
        if (waypointBuffer != null) waypointBuffer.Release();
        if (materialInstance != null) Destroy(materialInstance);
    }
}