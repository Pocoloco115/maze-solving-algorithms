using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MapRenderer _mapRenderer;
    [SerializeField] private Vector2Int _mazeSize = new Vector2Int(20, 20);
    [SerializeField] private MazeSolver[] mazeSolvers;
    public Vector2Int _start { get; private set; }
    public Vector2Int _end { get; private set; }
    public HashSet<Vector2Int> _floorPositions { get; private set; }

    public void StartGeneration()
    {
        if(MazeSolver.isSolving)
        {
            return;
        }
        _mapRenderer.Clear();
        if(mazeSolvers != null && mazeSolvers.Length > 0)
        {
            foreach(var solver in mazeSolvers)
            {
                solver.hasFinished = false;
                solver.UIHandler.ResetUI();
            }
        }
        BoundsInt mazeBounds = new BoundsInt(
            new Vector3Int(-_mazeSize.x / 2, -_mazeSize.y / 2, 0),
            new Vector3Int(_mazeSize.x, _mazeSize.y, 1));

        AlgorithmsManager.GenerateAldousBroder(mazeBounds, out var floors, out var walls);

        Debug.Log($"Generated maze with {floors.Count} floor tiles and {walls.Count} wall tiles.");
        _start = new Vector2Int(mazeBounds.xMin + 1, mazeBounds.yMin + 1);
        _floorPositions = new HashSet<Vector2Int>(floors);
        List<Vector2Int> list = floors.ToList();

        _end = list[Random.Range(0, list.Count)];

        while (_start == _end)
        {
            _end = list[Random.Range(0, list.Count)];
        }

        _mapRenderer.Render(floors, walls);
        _mapRenderer.PaintSinglePathTile(_start, null);
        _mapRenderer.PaintTargetTile(_end);
    }
    public void ClearCurrentSolution()
    {
        if (MazeSolver.isSolving) return;
        _mapRenderer.ClearPath(_floorPositions, _start, _end);
        foreach(var solver in mazeSolvers)
        {
            solver.hasFinished = false;
            solver.UIHandler.ResetUI();
        }
    }
    void Start()
    {
        StartGeneration();
    }
}
