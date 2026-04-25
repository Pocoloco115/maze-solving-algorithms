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
    private float _simulatedTime = 0;
    public void StartSolving()
    {
        if (isSolving || AnySolverFinished()) return;
        isSolving = true;
        _startTime = Time.time;
        _timerActive = true;
        Solve();
    }
    public abstract void Solve();
    protected abstract void Skip();
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
    public void SkipSolution()
    {
        StopTimer();
        OnResolutionCompleted();
    }
    protected void AproximateTime()
    {
        UIHandler.UpdateTimer(null);
    }
    protected void OnResolutionCompleted()
    {
        isSolving = false;
        hasFinished = true;
    }
    protected void StopTimer()
    {
        _timerActive = false;
    }
    protected void UpdateExplorationSteps(int steps)
    {
        UIHandler.UpdateExplorationStepsTxt(steps);
    }
    protected void UpdateFinalSteps(int steps)
    {
        UIHandler.UpdateFinalStepsTxt(steps);
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
