using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapRenderer : MonoBehaviour
{
    [SerializeField] private Tilemap _floorMap;
    [SerializeField] private Tilemap _wallMap;
    [SerializeField] private TileBase _floorTile;
    [SerializeField] private TileBase _wallTile;
    [SerializeField] private TileBase _pathTile;
    [SerializeField] private TileBase _targetTile;
    public void Clear()
    {
        _floorMap.ClearAllTiles();
        _wallMap.ClearAllTiles();
    }
    public void Render(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> wallsPositions)
    {
        foreach(var pos in floorPositions)
        {
            _floorMap.SetTile((Vector3Int)pos, _floorTile);
        }
        foreach(var pos in wallsPositions)
        {
            _wallMap.SetTile((Vector3Int)pos, _wallTile);
        }
    }
    public void PaintSinglePathTile(Vector2Int pos, Color? color)
    {
        Vector3Int tilePos = (Vector3Int)pos;
        _floorMap.SetTile((Vector3Int)pos, _pathTile);
        if (color.HasValue)
        {
            _floorMap.SetTileFlags(tilePos, TileFlags.None);

            _floorMap.SetColor(tilePos, color.Value);
        }
    }
    public void PaintTargetTile(Vector2Int pos)
    {
        _floorMap.SetTile((Vector3Int)pos, _targetTile);
    }
    public void ClearPath(HashSet<Vector2Int> floorPositions, Vector2Int start, Vector2Int target)
    {
        foreach (var pos in floorPositions)
        {

            Vector3Int tilePos = (Vector3Int)pos;
            _floorMap.SetTileFlags(tilePos, TileFlags.None);
            _floorMap.SetColor(tilePos, Color.white);
            _floorMap.SetTile(tilePos, _floorTile);
            if (pos != start && pos != target)
            {
                _floorMap.SetTile(tilePos, _floorTile);
            }
            else if (pos == start)
            {
                _floorMap.SetTile(tilePos, _pathTile);
            }
            else if (pos == target)
            {
                _floorMap.SetTile(tilePos, _targetTile);
            }
        }
    }
}
