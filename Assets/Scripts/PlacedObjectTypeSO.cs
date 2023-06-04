using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Buildings/Standard Building")]
public class PlacedObjectTypeSO : ScriptableObject
{
    public enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }
    
    public string title;
    public Sprite sprite;
    public Transform prefab;
    public int width;
    public int height;

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }
    
    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0,0);
            case Dir.Left: return new Vector2Int(0,width);
            case Dir.Up: return new Vector2Int(width,height);
            case Dir.Right: return new Vector2Int(height,0);
        }
    }
    
    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        var gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            case Dir.Down:
            case Dir.Up:
                for (var x = 0; x < width; x++)
                {
                    for (var z = 0; z < height; z++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, z));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (var x = 0; x < height; x++)
                {
                    for (var z = 0; z < width; z++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, z));
                    }
                }
                break;
        }
        return gridPositionList;
    }
}

