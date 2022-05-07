using System.Numerics;
using Raylib_cs;

namespace Simulation_CSharp.Utils.Widgets;

public class ButtonRenderer
{
    public readonly int RectX;
    public readonly int RectY;
    public readonly int RectWidth;
    private readonly int _rectHeight;
    private readonly string _text;
    private readonly Action<ButtonRenderer> _onPress;
    public bool IsSelected;
    
    public ButtonRenderer(int rectX, int rectY, string text, Action<ButtonRenderer> onPress)
    {
        _text = text;
        RectWidth = text.Length * 15 + 10;
        _rectHeight = 40;
        _onPress = onPress;
        RectX = rectX;
        RectY = rectY;
    }

    public void Render()
    {
        Raylib.DrawRectangleRounded(
            new Rectangle(
                RectX,
                RectY,
                RectWidth,
                _rectHeight
            ),
            0.05f,
            20,
            GetColor()
        );
        
        Raylib.DrawText(_text, (RectX + RectWidth / 8), (RectY + _rectHeight / 4), 20, Color.WHITE);

        if (!Helper.IsMousePosOverAreaUI(new Vector2(RectX, RectY), RectWidth, _rectHeight)) return;
        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            _onPress(this);
        }
    }

    private Color GetColor()
    {
        return Helper.IsMousePosOverAreaUI(new Vector2(RectX, RectY), RectWidth, _rectHeight) || IsSelected ? SimulationColors.WidgetHoverColor : SimulationColors.WidgetBackgroundColor;
    }
}