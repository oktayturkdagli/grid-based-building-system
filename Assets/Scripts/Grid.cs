using System;
using UnityEngine;

public class Grid<T>
{
    private int width;
    private int height;
    private float cellSize;
    Vector3 originPosition;
    private T[,] gridArray;
    private TextMesh[,] gridTextArray;
    
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<T>, int, int, T> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        
        gridArray = new T[width, height];
        for (var x = 0; x < gridArray.GetLength(0); x++)
        {
            for (var z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }
        
        bool showDebug = true;
        if (showDebug)
        {
            gridTextArray = new TextMesh[width, height];
            for (var x = 0; x < gridArray.GetLength(0); x++)
            {
                for (var z = 0; z < gridArray.GetLength(1); z++)
                {
                    gridTextArray[x,z] = Utils.CreateWorldText(gridArray[x, z].ToString(), null, GetWorldPosition(x, 0, z) + new Vector3(cellSize, 0 , cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, 0, z), GetWorldPosition(x, 0, z + 1), Color.white, 1000f);
                    Debug.DrawLine(GetWorldPosition(x, 0, z), GetWorldPosition(x + 1, 0, z), Color.white, 1000f);
                }
            }
        
            Debug.DrawLine(GetWorldPosition(0, 0, height), GetWorldPosition(width, 0, height), Color.white, 1000f);
            Debug.DrawLine(GetWorldPosition(width, 0, 0), GetWorldPosition(width, 0, height), Color.white, 1000f);
        }
    }
    
    public int GetWidth()
    {
        return width;
    }
    
    public int GetHeight()
    {
        return height;
    }
    
    public float GetCellSize()
    {
        return cellSize;
    }
    
    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize + originPosition;
    }
    
    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void SetValue(int x, int z, T value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
            gridTextArray[x, z].text = gridArray[x, z].ToString();
        }
    }
    
    public void SetValue(Vector3 worldPosition, T value)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetValue(x, z, value);
    }
    
    public T GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        
        return default(T);
    }
    
    public T GetValue(Vector3 worldPosition)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetValue(x, z);
    }
}