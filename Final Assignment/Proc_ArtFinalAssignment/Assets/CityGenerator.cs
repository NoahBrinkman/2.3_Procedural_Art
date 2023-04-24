using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CityGenerator : MonoBehaviour
{
    
    [SerializeField] private Texture2D heightMap;
    [SerializeField] private float minimumScale = 3;
    [SerializeField] private float scaleMultiplier = 10;
    [SerializeField] private float minimumThreshold = .1f;
    [SerializeField] private GameObject debugGameObject;
    [SerializeField] private Neighbourhood neighbourhood;
    [SerializeField] private Vector3 minimumNeighbourhoodSize;
    [SerializeField] private List<Neighbourhood> hoods;
    private void Start()
    {
        hoods = new List<Neighbourhood>();
        //SpawnHeightCubes();
        Generate();
    }

    private void Generate()
    {
        bool splitting = true;

        bool verticalSplit = true;
        //bool verticalSplit = false;
        List<Neighbourhood> uncutHoods = new List<Neighbourhood>()
            ;
        Neighbourhood hood = Instantiate(neighbourhood, transform.position, Quaternion.identity);
        hood.size = new Vector3(transform.localScale.x, 1, transform.localScale.y);
        uncutHoods.Add(hood);
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

            Neighbourhood clone1;
            Neighbourhood clone2;
            if (verticalSplit)
            {
                clone1 = Instantiate(h, transform.position, Quaternion.identity);
                clone1.size.z = heightCut;
                Vector3 clone1Pos = new Vector3(h.transform.position.x, h.transform.position.y, h.transform.position.z +heightCut-heightOffset);
                clone1.transform.position = clone1Pos;
                
                clone2 = Instantiate(h, transform.position, Quaternion.identity);
                Vector3 clone2Pos = new Vector3(h.transform.position.x, h.transform.position.y, h.transform.position.z-heightOffset);
                clone2.transform.position = clone2Pos;
                clone2.size.z -= heightCut;
            }
            else
            {
                clone1 = Instantiate(h, h.transform.position, Quaternion.identity);
                clone1.size.x = widthCut;
                Vector3 clone1Pos = new Vector3(h.transform.position.x +widthCut , h.transform.position.y, h.transform.position.z);
                clone1.transform.position = clone1Pos;
                
                clone2 = Instantiate(h, h.transform.position, Quaternion.identity);
                Vector3 clone2Pos = new Vector3(h.transform.position.x-widthOffset/2, h.transform.position.y, h.transform.position.z);
                clone2.transform.position = clone2Pos;
                clone2.size.x -= widthCut;
            }
            //uncutHoods.Add(clone1);
           // uncutHoods.Add(clone2);
            Destroy(h.gameObject);

        }
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
                    scale = minimumScale + (c.grayscale * scaleMultiplier);
                }
                else
                { 
                    scale = minimumScale;
                }
               
                float x = transform.position.x - (transform.localScale.x/2)*10+minimumScale/2 + i * minimumScale/2;
                float y = transform.position.y + scale/2;
                float z = transform.position.z - (transform.localScale.x/2)*10+minimumScale/2 + j * minimumScale/2;;
                GameObject tower = Instantiate(debugGameObject, new Vector3(x,y,z), Quaternion.identity);
                tower.transform.localScale = new Vector3(minimumScale, scale, minimumScale);
               
            }
        }

    }
}
