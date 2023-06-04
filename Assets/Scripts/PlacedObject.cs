using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;
    
    public static PlacedObject Create(Vector3 position, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, position, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));
        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }
    
    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
