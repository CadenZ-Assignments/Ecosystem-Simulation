using System.Timers;
using Timer = System.Timers.Timer;

namespace Simulation_CSharp.Utils;

public class Oscillator
{
    private readonly float _max;
    private readonly float _delta;
    private readonly Timer _timer;
    private float _value;
    private OscillationState _state;
    
    public Oscillator(float max, float startingValue = 0, float interval = 0.01f, float delta = 0.1f)
    {
        _max = max;
        _delta = delta;
        _timer = new Timer(interval);
        _timer.Start();
        _timer.Elapsed += Oscillate;
        _value = startingValue;
        _state = OscillationState.Up;
    }

    ~Oscillator()
    {
        _timer.Elapsed -= Oscillate;
    }

    public float GetValue()
    {
        return _value;
    }

    private void Oscillate(object? sender, ElapsedEventArgs e)
    {
        Console.WriteLine(_value);
        if (_state == OscillationState.Up)
        {
            _value+=_delta;
            if (_value >= _max)
            {
                _state = OscillationState.Down;
            }
        }

        if (_state == OscillationState.Down)
        {
            _value-=_delta;
            if (_value <= -_max)
            {
                _state = OscillationState.Up;
            }
        }
    }

    enum OscillationState
    {
        Up,
        Down
    }
}