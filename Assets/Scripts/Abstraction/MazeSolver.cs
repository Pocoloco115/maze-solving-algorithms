using System.Collections.Generic;
using UnityEngine;

public abstract class MazeSolver : MonoBehaviour
{
    [SerializeField] protected MazeGenerator _mazeGenerator;
    [SerializeField] protected float _delay = 0.5f;
    [SerializeField] protected MapRenderer _mapRenderer = null;
    [SerializeField] public UIHandler UIHandler;
    private float _startTime;
    private bool _timerActive = false;
    public static bool isSolving = false;
    public bool hasFinished = false;
    public void StartSolving()
    {
        if (isSolving || AnySolverFinished()) return;
        isSolving = true;
        _startTime = Time.time;
        _timerActive = true;
        Solve();
    }
    public abstract void Solve();
    private bool AnySolverFinished()
    {
        foreach(var solver in Object.FindObjectsByType<MazeSolver>(FindObjectsSortMode.None))
        {
            if (solver.hasFinished)
            {
                return true;
            }
        }
        return false;
    }
    protected void OnResolutionCompleted(int explorationLenght, int? pathLenght)
    {
        isSolving = false;
        hasFinished = true;
        _timerActive = false;
        UIHandler.UpdateSteps(explorationLenght, pathLenght);
    }
    private void Update()
    {
        if(_timerActive)
        {
            float elapsedTime = Time.time - _startTime;
            UIHandler.UpdateTimer(elapsedTime);
        }
    }
}
