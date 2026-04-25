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
        List<Vector2Int> exploration = AlgorithmsManager.GetDFSExploration(start, end, floorPositions);
        int stepCount = 0;
        foreach (var pos in exploration)
        {
            if(!isSolving)
            {
                Skip();
                yield break;
            }
            stepCount++;
            _mapRenderer.PaintSinglePathTile(pos, null);
            Debug.Log($"Path tile colocated in {pos.x}, {pos.y}");
            UpdateExplorationSteps(stepCount);
            yield return new WaitForSeconds(_delay);
        }

        StopTimer();
        
        stepCount = 0;
        foreach (var pos in exploration)
        {
            if(!isSolving)
            {
                Skip();
                yield break;
            }
            stepCount++;
            _mapRenderer.PaintSinglePathTile(pos, Color.darkOliveGreen);
            Debug.Log($"Final path tile colocated in {pos.x}, {pos.y}");
            UpdateFinalSteps(stepCount);
            yield return new WaitForSeconds(_delay);
        }
        OnResolutionCompleted();
    }
    protected override void Skip()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>(_mazeGenerator._floorPositions);
        List<Vector2Int> exploration = AlgorithmsManager.GetDFSExploration(_mazeGenerator._start, _mazeGenerator._end, floorPositions);
        foreach(var pos in exploration)
        {
            _mapRenderer.PaintSinglePathTile(pos, null);
        }
        UpdateExplorationSteps(exploration.Count);
        foreach (var pos in exploration)
        {
            _mapRenderer.PaintSinglePathTile(pos, Color.darkOliveGreen);
        }
        UpdateFinalSteps(exploration.Count);
        AproximateTime();
    }
}
