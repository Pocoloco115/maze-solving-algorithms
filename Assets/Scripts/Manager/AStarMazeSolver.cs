using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class AStarMazeSolver : MazeSolver
{
    public override void Solve()
    {
        StopAllCoroutines();
        StartCoroutine(AStarSolverRoutine());
    }
    private IEnumerator AStarSolverRoutine()
    {
        PathFindingResult exploration = AlgorithmsManager.GetAStarExploration(_mazeGenerator._start, _mazeGenerator._end, new HashSet<Vector2Int>(_mazeGenerator._floorPositions));
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

        if (exploration.FinalPath.Count > 0)
        {
            foreach(var pos in exploration.FinalPath)
            {
                stepCount++;
                _mapRenderer.PaintSinglePathTile(pos, Color.darkOliveGreen);
                Debug.Log($"Final path tile colocated in {pos.x}, {pos.y}");
                UpdateFinalSteps(stepCount);
                yield return new WaitForSeconds(_delay);
            }
        }
        OnResolutionCompleted();
    }

}
