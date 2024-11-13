public class Easing1DCom
{
    public float time { get; private set; }
    public float duration { get; private set; }
    public float to { get; private set; }
    public bool isEnd { get; private set; }

    private float _from;
    private EasingType _easingType;
    private bool _isResetTime = true;
    private float _curValue;

    public Easing1DCom()
    {
        Clear();
    }

    public void Clear()
    {
        this._from = 0f;
        this.to = 0f;
        this.time = 0f;
        this.isEnd = false;
        this._curValue = 0f;
    }

    public void SetEase(float duration, EasingType easingType)
    {
        this.duration = duration;
        this._easingType = easingType;
    }

    public void SetIsResetTime(bool isResetTime)
    {
        this._isResetTime = isResetTime;
    }

    public float TickEase(float from, float to, float dt)
    {
        this.time += dt;
        if (this.to != to)
        {
            this._from = from;
            this.to = to;
            this.isEnd = false;
            if (this._isResetTime) this.time = dt;
        }

        if (this.isEnd) return this.to;

        float t = this.duration > 0 ? this.time / this.duration : 1f;
        if (t >= 1f)
        {
            t = 1f;
            this.time -= this.duration;
            this.isEnd = true;
        }

        float easedValue = EasingFunctionUtil.EaseByType(this._easingType, t);
        float offset = this.to - this._from;
        this._curValue = this._from + offset * easedValue;
        return this._curValue;
    }
}
