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
        foreach(var pos in exploration.ExplorationPath)
        {
            _mapRenderer.PaintSinglePathTile(pos, null);
            Debug.Log($"Path tile colocated in {pos.x}, {pos.y}");
            yield return new WaitForSeconds(_delay);
        }
        if(exploration.FinalPath.Count > 0)
        {
            foreach(var pos in exploration.FinalPath)
            {
                _mapRenderer.PaintSinglePathTile(pos, Color.darkOliveGreen);
                Debug.Log($"Final path tile colocated in {pos.x}, {pos.y}");
                yield return new WaitForSeconds(_delay);
            }
        }
        OnResolutionCompleted(exploration.ExplorationPath.Count, exploration.FinalPath.Count);
    }

}
