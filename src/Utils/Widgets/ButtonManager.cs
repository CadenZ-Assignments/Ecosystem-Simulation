using System.Numerics;
using Raylib_cs;

namespace Simulation_CSharp.Utils.Widgets;

public class ButtonManager
{
    private List<ButtonRenderer> _buttons;
    private int _selected;

    public int Selected
    {
        get => _selected;
        private set
        {
            _selected = value;
            SelectedChanged();
        }
    }

    public ButtonManager(params string[] buttons)
    {
        _buttons = new List<ButtonRenderer>();

        var x = 20 + 100;
        for (var i = 0; i < buttons.Length; i++)
        {
            var i1 = i;
            var btn = new ButtonRenderer(x, 10, buttons[i], renderer =>
            {
                renderer.IsSelected = !renderer.IsSelected;
                if (renderer.IsSelected)
                {
                    Selected = i1;
                }
                else
                {
                    Selected = -1;
                }
            });
            x += (int) btn.RectWidth + 10;
            _buttons.Add(btn);
        }
        
        Selected = -1;
    }

    public void Render()
    {
        foreach (var btn in _buttons)
        {
            btn.Render();
        }
    }

    public bool IsMouseOver(ref Camera2D camera)
    {
        var res = _buttons.Select(btn => Helper.IsMousePosOverAreaUI(new Vector2(btn.RectX, btn.RectY), btn.RectWidth, 40)).ToList();
        return res.Contains(true);
    }

    private void SelectedChanged()
    {
        for (var i = 0; i < _buttons.Count; i++)
        {
            if (i == Selected) continue;
            _buttons[i].IsSelected = false;
        }
    }
}