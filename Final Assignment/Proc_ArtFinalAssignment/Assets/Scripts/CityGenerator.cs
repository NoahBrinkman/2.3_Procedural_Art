using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CityGenerator : AGeneratable
{
    [Header("Height Generation")] [SerializeField]
    private bool generateOnPlay = true;
    [SerializeField] private Texture2D heightMap;
    [SerializeField] private int minimumHeight = 3;
    [SerializeField] private float scaleMultiplier = 10;
    [SerializeField] private float minimumThreshold = .1f;
    
    [Header("Neighbourhood Generation")]
    [SerializeField] private Neighbourhood neighbourhood;
    [SerializeField] private Vector3 minimumNeighbourhoodSize;
     [SerializeField] private int densityMultiplier = 10;
     public List<Neighbourhood> hoods;
    
    [Header("Road Generation")]
    [SerializeField] private List<Vector3> cornerPoints;

     [Header("Debug")]
     [SerializeField] private GameObject debugGameObject;

     [SerializeField] private Curve roadGenerator;
    private void Start()
    {
        //SpawnHeightCubes();
        if (generateOnPlay)
        {
            hoods = new List<Neighbourhood>();
            Generate();
        }
    }

    public override void Generate()
    {
        
        /*bool verticalSplit = true;
        //bool verticalSplit = false;
        List<Neighbourhood> uncutHoods = new List<Neighbourhood>();
        Neighbourhood hood = Instantiate(neighbourhood, transform.position, Quaternion.identity);
        hood.size = new Vector3(transform.localScale.x, 1, transform.localScale.y);
        uncutHoods.Add(hood);
        int unCutCount = 0;
        while(uncutHoods.Count > 0)
        {
            Neighbourhood h = uncutHoods[uncutHoods.Count-1];
            verticalSplit = h.size.z > h.size.x;
            uncutHoods.Remove(h);
            float widthOffset = h.size.x / (2 * minimumNeighbourhoodSize.x);
            float heightOffset = h.size.z / (2 * minimumNeighbourhoodSize.z);;
            
            float widthCut = minimumNeighbourhoodSize.x + Random.Range(0, minimumNeighbourhoodSize.x + widthOffset);
            float heightCut = minimumNeighbourhoodSize.z + Random.Range(0, minimumNeighbourhoodSize.z + heightOffset);
            
            if ((h.size.x - widthCut < minimumNeighbourhoodSize.x && verticalSplit)
                || (h.size.z - heightCut < minimumNeighbourhoodSize.z && !verticalSplit))
            {
                hoods.Add(h);
                continue;
            }

            unCutCount++;
            Neighbourhood clone1;
            Neighbourhood clone2;
            if (verticalSplit)
            {
                clone1 = Instantiate(h, transform.position, Quaternion.identity);
                clone1.size.z = heightCut;
                Vector3 clone1Pos = new Vector3(clone1.transform.position.x, clone1.transform.position.y, clone1.transform.position.z +heightCut/2- transform.localScale.y/2);
                clone1.transform.position = clone1Pos;
                clone1.transform.name = "Clone1 " + unCutCount;
                clone2 = Instantiate(h, transform.position, Quaternion.identity);
                Vector3 clone2Pos = new Vector3(clone2.transform.position.x, clone2.transform.position.y, clone2.transform.position.z - heightCut - transform.localScale.y/2);
                clone2.transform.position = clone2Pos;
                clone2.size.z -= heightCut;
                clone2.transform.name = "Clone2 " + unCutCount;
                clone1.color = Random.ColorHSV();
                clone2.color = Random.ColorHSV();
            }
            else
            {
                clone1 = Instantiate(h, h.transform.position, Quaternion.identity);
                clone1.size.x = widthCut;
                Vector3 clone1Pos = 
                    new Vector3(h.transform.position.x, h.transform.position.y, h.transform.position.z);
                clone1.transform.position = clone1Pos;
                
                clone2 = Instantiate(h, h.transform.position, Quaternion.identity);
                Vector3 clone2Pos = 
                    new Vector3(h.transform.position.x, h.transform.position.y, h.transform.position.z);
                clone2.transform.position = clone2Pos;
                clone2.size.x -= widthCut;
             
                clone1.color = Random.ColorHSV();
                clone2.color = Random.ColorHSV();
            }
            uncutHoods.Add(clone1);
            uncutHoods.Add(clone2);
            DestroyImmediate(h.gameObject);

        }*/

       /* ###Working Grid based Generation###*/
        int rows = Mathf.RoundToInt(transform.localScale.y / minimumNeighbourhoodSize.z);
       int cols = Mathf.RoundToInt(transform.localScale.x / minimumNeighbourhoodSize.x);
       Vector3 startPosition = transform.position;
       startPosition.x -= transform.localScale.x / 2 - minimumNeighbourhoodSize.x/2;
       startPosition.z -= transform.localScale.y / 2- minimumNeighbourhoodSize.z/2;
      

       for (int i = 0; i < cols; i++)
       {
           Vector3 spawnPosition = startPosition;
           spawnPosition.x = startPosition.x + minimumNeighbourhoodSize.x * i;
           for (int j = 0; j < rows; j++)
           {
               spawnPosition.z = startPosition.z + minimumNeighbourhoodSize.z * j;
               Neighbourhood h = Instantiate(neighbourhood,spawnPosition,quaternion.identity);
               h.size = minimumNeighbourhoodSize;

               int x = Mathf.RoundToInt( h.transform.localPosition.x + transform.localScale.x / 2);
               
               int y = Mathf.RoundToInt(h.transform.localPosition.z - transform.localScale.y / 2);
               h.height = GetHeight(x, y);
               h.density = GetDensity(x, y);
               h.color = Random.ColorHSV();
               hoods.Add(h);

           }
       }
       foreach (Neighbourhood hood in hoods)
       {
           hood.Generate();
           hood.transform.SetParent(transform);
          
       }
    }
    
    public override void DeGenerate()
    {
        List<Neighbourhood> deletableHoods = new List<Neighbourhood>(hoods);
        foreach (var hood in deletableHoods)
        {
            hoods.Remove(hood);
            DestroyImmediate(hood.gameObject);
        }
        deletableHoods.Clear();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Transform g in transform)
            {
                Destroy(g.gameObject);
            }
            Generate();
        }
    }

    private int GetHeight(int x, int y)
    {
        Color c = heightMap.GetPixel(Mathf.RoundToInt(x * transform.localScale.x/4), Mathf.RoundToInt(y * transform.localScale.y/4));
        

            int scale;
        if (c.grayscale > minimumThreshold)
        {
            scale = Mathf.RoundToInt(minimumHeight + (c.grayscale * scaleMultiplier));
        }
        else
        { 
            scale = minimumHeight;
        }
        
        
        return scale;
    }
    private int GetDensity(int x, int y)
    {

        Color c = heightMap.GetPixel(x, y);
        

        int scale;
        scale = Mathf.RoundToInt(minimumHeight + (c.grayscale * densityMultiplier));

        return scale;
    }
    private void SpawnHeightCubes()
    {
        int width = heightMap.width;
        int height = heightMap.height;
        for (int i = 0; i < width; i+= 4)
        {
            for (int j = 0; j < height; j+=4)
            {
                Color c = heightMap.GetPixel(i, j);
                Debug.Log(c.grayscale);
                float scale;
                if (c.grayscale > minimumThreshold)
                {
                    scale = minimumHeight + (c.grayscale * scaleMultiplier);
                }
                else
                { 
                    scale = minimumHeight;
                }
               
                float x = transform.position.x - (transform.localScale.x/2)*10+minimumHeight/2 + i * minimumHeight/2;
                float y = transform.position.y + scale/2;
                float z = transform.position.z - (transform.localScale.x/2)*10+minimumHeight/2 + j * minimumHeight/2;;
                GameObject tower = Instantiate(debugGameObject, new Vector3(x,y,z), Quaternion.identity);
                tower.transform.localScale = new Vector3(minimumHeight, scale, minimumHeight);
               
            }
        }

    }
}
