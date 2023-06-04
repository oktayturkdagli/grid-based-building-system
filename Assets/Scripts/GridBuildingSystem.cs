using System;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : Singleton<GridBuildingSystem>
{
    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;
    
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private Grid<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;
    
    private void Awake()
    {
        int gridWidth = 10;
        int gridHeight = 10;
        float cellSize = 10f;
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (Grid<GridObject> g, int x, int z) => new GridObject(g, x, z));
        
        placedObjectTypeSO = placedObjectTypeSOList[0];
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && placedObjectTypeSO)
        {
            grid.GetXZ(Utils.GetMousePositionIn3D(), out int x, out int z);
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(new Vector2Int(x, z), dir);
            
            // Check can build
            bool canBuild = true;
            foreach (var gridPosition in gridPositionList)
            {
                if (grid.GetValue(gridPosition.x, gridPosition.y).CanBuild())
                {
                    // Cannot build here
                    canBuild = false;
                    break;
                }
            }

            var gridObject = grid.GetValue(x, z);
            if (canBuild)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, 0, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedObjectTypeSO);
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetValue(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.Log("Cannot build here!");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            GridObject gridObject = grid.GetValue(Utils.GetMousePositionIn3D());
            PlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject)
            {
                placedObject.DestroySelf();
                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetValue(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
            Debug.Log(dir);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }
    }
    
    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Utils.GetMousePositionIn3D();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO)
        {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir); 
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, 0, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize(); 
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }
    
    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObjectTypeSO)
        {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }
    
    private void DeselectObjectType() 
    {
        placedObjectTypeSO = null;
        RefreshSelectedObjectType();
    }
    
    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public PlacedObjectTypeSO GetPlacedObjectTypeSo()
    {
        return placedObjectTypeSO;
    }
}

public class GridObject
{
    private Grid<GridObject> grid;
    private int x;
    private int z;
    private PlacedObject placedObject;
    
    public GridObject(Grid<GridObject> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }
    
    public override string ToString()
    {
        return x + ", " + z + "\n" + placedObject;
    }
    
    public PlacedObject GetPlacedObject()
    {
        return placedObject;
    }
    
    public void SetPlacedObject(PlacedObject placedObject)
    {
        this.placedObject = placedObject;
    }
    
    public void ClearPlacedObject()
    {
        placedObject = null;
    }
    
    public bool CanBuild()
    {
        return placedObject;
    }
}

