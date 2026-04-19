using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class DFSMazeSolver : MazeSolver
{
    public override void Solve()
    {
        StopAllCoroutines();
        StartCoroutine(DFSSolveRoutine());
    }
    private IEnumerator DFSSolveRoutine()
    {
        Vector2Int start = _mazeGenerator._start, end = _mazeGenerator._end;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(_mazeGenerator._floorPositions);
        Debug.Log($"Starting DFS from {start.x}, {start.y} to {end.x}, {end.y}");
        Debug.Log($"Total floor positions: {floorPositions.Count}");
        PathFindingResult exploration = AlgorithmsManager.GetDFSExploration(start, end, floorPositions);
        foreach(var pos in exploration.ExplorationPath)
        {
            _mapRenderer.PaintSinglePathTile(pos, null);
            Debug.Log($"Path tile colocated in {pos.x}, {pos.y}");
            yield return new WaitForSeconds(_delay);
        }
        foreach(var pos in exploration.FinalPath)
        {
            _mapRenderer.PaintSinglePathTile(pos, Color.darkOliveGreen);
            Debug.Log($"Final path tile colocated in {pos.x}, {pos.y}");
            yield return new WaitForSeconds(_delay);
        }
        OnResolutionCompleted(exploration.ExplorationPath.Count, exploration.FinalPath.Count);
    }
}
