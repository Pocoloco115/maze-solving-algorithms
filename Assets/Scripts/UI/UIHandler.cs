using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _explorationStepsText;
    [SerializeField] private TextMeshProUGUI _finalStepsText;

    public void UpdateTimer(float time)
    {
        _timerText.text = $"Time: {time:F2}s";
    }
    public void UpdateSteps(int explorationSteps, int? finalSteps)
    {
        _explorationStepsText.text = $"Steps: {explorationSteps}";
        _finalStepsText.text = finalSteps.HasValue ? $"Final Steps: {finalSteps.Value}" : $"Final Steps: {explorationSteps}";
    }
    public void UpdateExplorationStepsTxt(int explorationSteps)
    {
        _explorationStepsText.text = $"Steps: {explorationSteps}";
    }
    public void UpdateFinalStepsTxt(int finalSteps)
    {
        _finalStepsText.text = $"Final Steps: {finalSteps}";
    }
    public void ResetUI()
    {
        UpdateTimer(0);
        UpdateSteps(0, 0);
    }
    public void CloseApp()
    {
        Application.Quit();
    }
}
