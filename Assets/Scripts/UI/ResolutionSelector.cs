using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ResolutionSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;

    private static readonly Vector2Int[] commonResolutions = new Vector2Int[]
    {
        new Vector2Int(400, 400),
        new Vector2Int(600, 600),
        new Vector2Int(800, 800)
    };

    private List<Vector2Int> _validResolutions = new List<Vector2Int>();
    private int _currentIndex;
    void Start()
    {
        _validResolutions = new List<Vector2Int>(commonResolutions);
        var config = GraphicsConfigManager.GetWorkingCopy();
        _currentIndex = _validResolutions.FindIndex(r => r.x == config.resolutionWidth && r.y == config.resolutionHeight);

        if (_currentIndex < 0)
        {
            _currentIndex = 2; 
            config.resolutionWidth = _validResolutions[_currentIndex].x;
            config.resolutionHeight = _validResolutions[_currentIndex].y;
        }

        UpdateUI();
    }
    public void ChangeValue(int direction)
    {
        Debug.Log($"Actual resolution: {Screen.width} x {Screen.height} and index {_currentIndex}");
        Debug.Log($"Changing resolution by {direction}");
        _currentIndex += direction;

        if (_currentIndex < 0)
        {
            _currentIndex = _validResolutions.Count - 1;
        }
        if (_currentIndex >= _validResolutions.Count)
        {
            _currentIndex = 0;
        }

        var config = GraphicsConfigManager.GetWorkingCopy();
        config.resolutionWidth = _validResolutions[_currentIndex].x;
        config.resolutionHeight = _validResolutions[_currentIndex].y;

        UpdateUI();
    }
    private void UpdateUI()
    {
        Vector2Int res = _validResolutions[_currentIndex];
        valueText.text = $"{res.x} x {res.y}";
    }
    public void SaveSettings()
    {
        GraphicsConfigManager.SaveAndApply();
    }
}
