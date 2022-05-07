using Raylib_cs;

namespace Simulation_CSharp.Utils;

public class TooltipRenderer
{
    private float _rectX;
    private float _rectY;
    private float _rectWidth;
    private float _rectHeight;
    private float _contentYModifier;

    public TooltipRenderer(float rectX, float rectY, float rectWidth, float rectHeight)
    {
        _rectX = rectX;
        _rectY = rectY;
        _rectWidth = rectWidth;
        _rectHeight = rectHeight;
        _contentYModifier = 5;
    }

    public void DrawBackground()
    {
        Raylib.DrawRectangleRounded(
            new Rectangle(
                _rectX,
                _rectY,
                _rectWidth,
                _rectHeight
            ),
            0.05f,
            20,
            new Color(20, 20, 20, 20)
        );
    }

    public void DrawText(string text)
    {
        Raylib.DrawText(text, (int) _rectX + 5, (int) _rectY + (int) _contentYModifier, 5, Color.WHITE);
        _rectHeight += 10;
        _contentYModifier += 10;
    }

    public void DrawProgressBar(string label, float max, float value)
    {
        DrawText(label);
            
        var barWidth = _rectWidth - 10;
        const int barHeight = 20;
        var barX = _rectX + 5;
        var barY = _rectY + _contentYModifier;

        Raylib.DrawRectangleRounded(
            new Rectangle(
                barX,
                barY,
                barWidth,
                barHeight
            ),
            0.5f,
            4,
            new Color(200, 200, 200, 200)
        );
            
        Raylib.DrawRectangleRounded(
            new Rectangle(
                barX + 2,
                barY + 2,
                (barWidth - 4) * (value / max),
                barHeight - 4
            ),
            0.5f,
            4,
            new Color(100, 100, 100, 250)
        );
            
        Raylib.DrawText(value.ToString(), (int) barX + 8, (int) barY + 5, 3, Color.BLACK);

        _rectHeight += barHeight + 5;
        _contentYModifier += barHeight + 2;
    }
}