using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    private void Awake()
    {
        GraphicsConfigManager.ApplyCurrentSettings();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pausePanel.activeSelf)
            {
                Time.timeScale = 1f;
                pausePanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                pausePanel.SetActive(true);
            }
        }
    }
}

