using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Neighbourhood : AGeneratable
{
    public Vector3 size = Vector3.one;
    public Color color;
    [SerializeField] private float padding;
    [SerializeField] private Building buildingPrefab;
    [SerializeField] private List<Building> buildings;
    public int height;
    [SerializeField] private int attemptsAllowedToTryAndSpawn = 50;
    
    
    [Header("Debug")] [SerializeField] 
    private bool showPositionCube = true;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if(showPositionCube) Gizmos.DrawCube(transform.position, size);
    }

    public override void Generate()
    {
      
        Bounds bounds = new Bounds(transform.position, size);
        List<Vector3> positions = new List<Vector3>();
        int buildings = Random.Range(1, height);
        for (int i = 0; i < buildings; i++)
        {
            Vector3 spawnPos;
            int attemptsDone = 0;
            do
            {
                attemptsDone++;
      
                float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
                float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);
                float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);
                spawnPos = bounds.center + new Vector3(offsetX, offsetY, offsetZ);

            } while (positions.Any(p => Vector3.Distance(p, spawnPos) < padding) || attemptsDone <= attemptsAllowedToTryAndSpawn);
            
            positions.Add(spawnPos);
            Building b = Instantiate(buildingPrefab, spawnPos, Quaternion.identity,transform);
            b.SetHeight(height);
            b.Generate();
        }
    }

    public override void DeGenerate()
    {
        List<Building> deletablebuildings = new List<Building>(buildings);
        foreach (var building in deletablebuildings)
        {
            buildings.Remove(building);
            DestroyImmediate(building.gameObject);
        }
        deletablebuildings.Clear();
    }
}
