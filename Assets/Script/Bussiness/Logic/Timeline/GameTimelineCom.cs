using System;
using System.Collections.Generic;
using GamePlay.Core;

namespace GamePlay.Bussiness.Logic
{
    public class GameTimelineCom
    {
        // 时长(秒)
        public float length { get; private set; }
        public void SetLength(float length)
        {
            this.length = length;
        }
        // 时长(帧)
        public int frameLength => GameMathF.FloorToInt(length * GameTimeCollection.frameRate);
        // 是否正在播放
        public bool isPlaying { get; private set; }
        // 0非循环，-1无限循环，>0循环时间
        public float loopDuration { get; private set; }
        /// <summary> 是否为无限循环 </summary>
        public bool isEndlessLoop => this.loopDuration < 0;
        /// <summary> 是否为循环 </summary>
        public bool IsLoop => this.loopDuration != 0;

        // 当前时间
        public float time { get; private set; }
        // 当前帧
        public int frame => this._ConvertToFrame(this.time);

        // 完成回调
        private Action _complete;

        private float _cacheDt;

        // 事件列表
        private Dictionary<int, List<Action>> _events;

        public GameTimelineCom(float length = 0)
        {
            this.length = length;
            this._events = new Dictionary<int, List<Action>>();
        }

        public void Clear()
        {
            this.isPlaying = false;
            this.loopDuration = 0;
            this.time = 0;
            this._cacheDt = 0;
            this._complete = null;
            this._events.Clear();
        }

        private int _ConvertToFrame(float time)
        {
            var frame = GameMathF.FloorToInt(time * GameTimeCollection.frameRate) - 1;
            frame = frame < 0 ? 0 : frame;
            return frame;
        }

        public void AddEventByFrame(int frame, Action action)
        {
            if (frame < 0 || frame >= this.frameLength)
            {
                GameLogger.LogError($"时间轴添加事件帧超出范围: {frame} / {this.frameLength}");
                return;
            }
            if (!this._events.ContainsKey(frame))
            {
                this._events.Add(frame, new List<Action>());
            }
            this._events[frame].Add(action);
            GameLogger.Log($"时间轴添加事件帧: {frame}");
        }

        public void AddEventByTime(float time, Action action)
        {
            this.AddEventByFrame(this._ConvertToFrame(time), action);
        }

        /// <summary> 播放, 后续逻辑的迭代需使用Tick </summary>
        public void Play(float loopDuration = 0, float startTime = 0, Action complete = null)
        {
            if (this.length == 0) return;
            this.loopDuration = loopDuration;
            this.time = startTime;
            this._complete = complete;
            this._loopTime = 0;
            this.isPlaying = true;
        }

        public void Stop()
        {
            this.isPlaying = false;
            this.loopDuration = 0;
            this.time = 0;
            this._cacheDt = 0;
            this._complete = null;
        }

        public bool Tick(float dt)
        {
            if (!this.isPlaying) return false;
            if (this.IsLoop) this._TickLoop(dt);
            else this._TickNormal(dt);
            if (!this.isPlaying)
            {
                this._complete?.Invoke();
                this.Stop();
            }
            return this.isPlaying;
        }

        private void _TickFrame(int frame)
        {
            if (this._events.ContainsKey(frame))
            {
                var list = this._events[frame];
                for (int i = 0; i < list.Count; i++)
                {
                    list[i]();
                }
            }
        }

        private void _TickNormal(float dt)
        {
            var frameTime = GameTimeCollection.frameTime;
            dt += this._cacheDt;
            while (dt >= frameTime)
            {
                dt -= frameTime;
                this.time += frameTime;
                this._TickFrame(this.frame);
            }
            this._cacheDt = dt;
            this.isPlaying = this.time < this.length;
        }

        private void _TickLoop(float dt)
        {
            var frameTime = GameTimeCollection.frameTime;
            dt += this._cacheDt;
            while (dt > frameTime)
            {
                dt -= frameTime;
                this.time += frameTime;
                this._loopTime += frameTime;
                if (this._loopTime >= this.length) this._loopTime -= this.length;
                this._TickFrame(this._ConvertToFrame(this._loopTime));
            }
            this._cacheDt = dt;
            if (!this.isEndlessLoop) this.isPlaying = this.time < this.loopDuration;
        }
        private float _loopTime;

    }
}