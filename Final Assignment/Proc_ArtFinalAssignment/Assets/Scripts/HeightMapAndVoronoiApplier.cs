using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapAndVoronoiApplier : MonoBehaviour
{

    [SerializeField] private Texture2D heightMap;
    [SerializeField] private float minimumScale = 3;
    [SerializeField] private float scaleMultiplier = 10;
    [SerializeField] private float minimumThreshold = .1f;
    [SerializeField] private GameObject debugGameObject;

    [SerializeField] private VoronoiNoiseGenerator gen;
    public Material mapMaterial;
    public NeighborhoodBlock neighborhoodBlockPrefab;
    
    Vector2Int[] GetShapePoints(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;

        Vector2Int[] shapePoints = new Vector2Int[width * height];
        int numShapePoints = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                if (pixels[index].grayscale > minimumThreshold)
                {
                    shapePoints[numShapePoints] = new Vector2Int(x, y);
                    numShapePoints++;
                }
            }
        }

        // Trim the array to only contain the shape points
        System.Array.Resize(ref shapePoints, numShapePoints);

        return shapePoints;
    }
    
    private void Start()
    {
        
        // Extract the shape from the texture
        Vector2Int[] shapePoints = GetShapePoints(gen.GenerateVoronoiNoiseTexture());

        // Instantiate the neighborhood block and pass the shape to it
        NeighborhoodBlock neighborhoodBlock = Instantiate(neighborhoodBlockPrefab);
        neighborhoodBlock.SetShape(shapePoints);
        
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
               
                float x = transform.position.x - (transform.localScale.x/2) + i * minimumScale/2;
                float y = transform.position.y + scale/2;
                float z = transform.position.z - (transform.localScale.x/2) + j * minimumScale/2;;
                GameObject tower = Instantiate(debugGameObject, new Vector3(x,y,z), Quaternion.identity);
                tower.transform.localScale = new Vector3(minimumScale, scale, minimumScale);
               
            }
        }

    }
    
}
