using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : AGeneratable
{
    [SerializeField] private int minimumHeight;
    
    [SerializeField] private int maximumHeight;

    [SerializeField] private GameObject debugCube;

    public BuildingHelper helper;

    [SerializeField] private float sizeOfBlock = 3;

    public float SizeOfBlock => sizeOfBlock;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHeight(int pHeight)
    {
        if (pHeight > minimumHeight)
        {
            maximumHeight = pHeight;
        }
        else
        {
            Debug.LogWarning("givenHeight is smaller than tge minimum height!");
        }
    }
    public override void Generate()
    {
        int height = Random.Range(minimumHeight,maximumHeight);
        if (helper == null)
        {
            for (int i = 0; i < height; i++)
            {
                Vector3 position = new Vector3(transform.position.x,
                    transform.position.y +.5f+ + (debugCube.transform.localScale.y * i), transform.position.z);
                Instantiate(debugCube, position, Quaternion.identity,transform);
            }
        }
        else
        {
            height -= 2;
            Vector3 position = transform.position;
            Instantiate(helper.groundFloors[Random.Range(0, helper.groundFloors.Count)]
                , position, Quaternion.identity, transform);
            for (int i = 0; i < height; i++)
            {
                GameObject floor = helper.floors[Random.Range(0, helper.floors.Count)];
                 position = new Vector3(transform.position.x,
                    transform.position.y + sizeOfBlock + (sizeOfBlock  * i), transform.position.z);
                 Instantiate(floor, position, Quaternion.identity,transform);
            }

            GameObject roof = helper.roofs[Random.Range(0, helper.roofs.Count)];
            position = new Vector3(transform.position.x,
                transform.position.y + (sizeOfBlock * (height+1)), transform.position.z);
            Instantiate(roof, position, Quaternion.identity,transform);
        }
    }

    public override void DeGenerate()
    {
        foreach (Transform t in transform)
        {
            DestroyImmediate(t.gameObject);
        }
    }
}
