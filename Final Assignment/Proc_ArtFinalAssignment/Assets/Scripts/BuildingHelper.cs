using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NeighbourhoodHelper", order = 1)]
public class BuildingHelper : ScriptableObject
{
    public List<GameObject> groundFloors;
    public List<GameObject> floors;
    public List<GameObject> roofs;
}
