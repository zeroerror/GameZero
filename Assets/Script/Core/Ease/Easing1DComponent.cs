public class Easing1DComponent
{
    public float Duration => _duration;
    public float Time => _time;
    public float To => _to;
    public bool IsEnd => _isEnd;

    private float _from;
    private float _to;
    private float _duration;
    private float _time;
    private EasingType _easingType;
    private bool _isResetTime = true;
    private float _curValue;
    private bool _isEnd;

    public Easing1DComponent()
    {
        Clear();
    }

    public void Clear()
    {
        _from = 0f;
        _to = 0f;
        _time = 0f;
        _isEnd = false;
        _curValue = 0f;
    }

    public void SetEase(float duration, EasingType easingType)
    {
        _duration = duration;
        _easingType = easingType;
    }

    public void SetIsResetTime(bool isResetTime)
    {
        _isResetTime = isResetTime;
    }

    public float TickEase(float from, float to, float dt)
    {
        _time += dt;
        if (_to != to)
        {
            _from = from;
            _to = to;
            _isEnd = false;
            if (_isResetTime) _time = dt;
        }

        if (_isEnd) return _to;

        float t = _duration > 0 ? _time / _duration : 1f;
        if (t >= 1f)
        {
            t = 1f;
            _time -= _duration;
            _isEnd = true;
        }

        float easedValue = EasingFunctionUtil.EaseByType(_easingType, t);
        float offset = _to - _from;
        _curValue = _from + offset * easedValue;
        return _curValue;
    }
}
