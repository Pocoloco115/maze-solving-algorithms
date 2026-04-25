using UnityEngine;

[System.Serializable]
public class GraphicsConfig
{
    public int resolutionWidth;
    public int resolutionHeight;
    public bool fullscreen;

    public GraphicsConfig Clone()
    {
        return new GraphicsConfig
        {
            resolutionWidth = this.resolutionWidth,
            resolutionHeight = this.resolutionHeight,
            fullscreen = this.fullscreen
        };
    }
}
