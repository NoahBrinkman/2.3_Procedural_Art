using UnityEngine;
public class NeighborhoodBlock : MonoBehaviour
{
    public GameObject cubePrefab;
    public float cubeSize = 1f;

    Vector2Int[] shapePoints;

    public void SetShape(Vector2Int[] points)
    {
        shapePoints = points;

        // Instantiate cubes at the shape points
        foreach (Vector2Int point in shapePoints)
        {
            Vector3 position = new Vector3(point.x * cubeSize, 0f, point.y * cubeSize);
            Instantiate(cubePrefab, position, Quaternion.identity, transform);
        }
    }
}