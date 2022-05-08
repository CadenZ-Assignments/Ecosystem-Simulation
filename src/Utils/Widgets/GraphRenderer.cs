using System.Numerics;
using Raylib_cs;

namespace Simulation_CSharp.Utils.Widgets;

public class GraphRenderer
{
    private const int Gap = 100;
    private const int Width = 2000;
    private const int Height = 1000;
    
    private readonly int _xPos;
    private readonly int _yPos;
    private int _yStamp;

    public GraphRenderer(int xPos, int yPos)
    {
        _xPos = xPos;
        _yPos = yPos;
        _yStamp = 0;
    }

    public void Render()
    {
        _yStamp++;
        
        // background
        Raylib.DrawRectangle(_xPos, _yPos, Width, Height, Color.GRAY);
        
        // x axis
        Raylib.DrawLineEx(new Vector2(_xPos + Gap, _yPos + Height - Gap), new Vector2(_xPos + Width - Gap, _yPos + Height - Gap), 5, Color.BLACK);
        // y axis
        Raylib.DrawLineEx(new Vector2(_xPos + Gap, _yPos + Gap), new Vector2(_xPos + Gap, _yPos + Height - Gap), 5, Color.BLACK);
        
        // labels
        Raylib.DrawText("Time", _xPos + Width/2 - Raylib.MeasureText("Time", 40), _yPos + Height - Gap + 50, 40, Color.BLACK);
        Raylib.DrawTextPro(Raylib.GetFontDefault(), "Data", new Vector2(_xPos + Gap - 80, _yPos + Height / 2 - 10), Vector2.Zero, 270, 40, 1, Color.BLACK);
    }
}