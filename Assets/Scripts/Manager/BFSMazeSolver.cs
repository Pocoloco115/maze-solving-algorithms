using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal.Internal;

public class BFSMazeSolver : MazeSolver
{
    
    public override void Solve()
    {
        StopAllCoroutines();
        StartCoroutine(BFSSolveRoutine());
    }
    IEnumerator BFSSolveRoutine()
    {
        Vector2Int start = _mazeGenerator._start, end = _mazeGenerator._end;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(_mazeGenerator._floorPositions);
        Debug.Log($"Starting BFS from {start.x}, {start.y} to {end.x}, {end.y}");
        Debug.Log($"Total floor positions: {floorPositions.Count}");
        PathFindingResult exploration = AlgorithmsManager.GetBFSExploration(start, end, floorPositions);
        int stepCount = 0;
        foreach (var pos in exploration.ExplorationPath)
        {
            stepCount++;
            _mapRenderer.PaintSinglePathTile(pos, null);
            Debug.Log($"Path tile colocated in {pos.x}, {pos.y}");
            UpdateExplorationSteps(stepCount);
            yield return new WaitForSeconds(_delay);
        }

        StopTimer();
        stepCount = 0;

        foreach (var pos in exploration.FinalPath)
        {
            stepCount++;
            _mapRenderer.PaintSinglePathTile(pos, Color.darkOliveGreen);
            Debug.Log($"Final path tile colocated in {pos.x}, {pos.y}");
            UpdateFinalSteps(stepCount);
            yield return new WaitForSeconds(_delay);
        }
        OnResolutionCompleted();
    }
}
