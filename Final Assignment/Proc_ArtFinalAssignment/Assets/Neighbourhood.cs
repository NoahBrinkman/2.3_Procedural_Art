using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Neighbourhood : MonoBehaviour
{
    public Vector3 size = Vector3.one;
    

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
        Gizmos.DrawCube(transform.position, size);
    }
}
