using UnityEngine;
using System;
using System.Collections.Generic;


namespace GamePlay.Core
{

    public class GameCameraZoomData
    {
        public float from;
        public float to;
        public float duration;
        public EasingType easingType;
        public bool needReset;
        public Action onComplete;

        public float time;
        public float value;


        public GameCameraZoomData(float from, float to, float duration, EasingType easingType, bool needReset, Action onComplete = null)
        {
            this.from = from;
            this.to = to;
            this.duration = duration;
            this.easingType = easingType;
            this.needReset = needReset;
            this.onComplete = onComplete;

            this.time = 0;
            this.value = 0;
        }

        public void Clear()
        {
            this.value = 0;
            this.time = 0;
        }
    }


    public class GameCameraZoomCom
    {
        public Camera camera { get; private set; }

        Queue<GameCameraZoomData> _shakeDataQueue = new Queue<GameCameraZoomData>();
        Queue<GameCameraZoomData> _shakeDataPool = new Queue<GameCameraZoomData>();
        float _zoomValue = 0;
        GameCameraZoomData _CreateData(float from, float to, float duration, EasingType easingType, bool needReset, Action onComplete)
        {
            if (!this._shakeDataPool.TryDequeue(out var data))
            {
                data = new GameCameraZoomData(from, to, duration, easingType, needReset, onComplete);
            }
            else
            {
                data.Clear();
            }
            this._shakeDataQueue.Enqueue(data);
            return data;
        }

        public GameCameraZoomCom(Camera camera)
        {
            this.camera = camera;
        }

        public void Tick(float dt)
        {
            if (!this._shakeDataQueue.TryPeek(out var top)) return;
            var needRemove = this._Tick(top, dt);
            if (needRemove)
            {
                this._shakeDataPool.Enqueue(this._shakeDataQueue.Dequeue());
            }
        }

        bool _Tick(GameCameraZoomData data, float dt)
        {
            data.time += dt;
            var t = data.time / data.duration;
            t = EasingFunctionUtil.EaseByType(data.easingType, t);
            data.value = data.from + (data.to - data.from) * t;
            this._zoomValue = data.value;
            var needRemove = t >= 1;
            if (needRemove)
            {
                data.onComplete?.Invoke();
                if (data.needReset) this._zoomValue = data.from;
            }
            return needRemove;
        }


        public void Apply()
        {
            if (this._zoomValue == 0) return;
            this.camera.orthographicSize = this._zoomValue;
        }

        public void Zoom(float from, float to, float duration, EasingType easingType, bool needReset = false, Action onComplete = null)
        {
            this._CreateData(from, to, duration, easingType, needReset, onComplete);
        }
    }
}