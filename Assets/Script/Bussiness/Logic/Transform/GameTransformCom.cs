
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameTransformCom
    {
        public int id { get; private set; }

        public GameVec2 position
        {
            get { return _position; }
            set
            {
                if (value.x == 0 && value.y == 0) return;
                _position = value;
                _posDirty = true;
            }
        }
        private GameVec2 _position;
        private bool _posDirty;

        public float scale
        {
            get { return _scale; }
            set
            {
                if (value == _scale) return;
                _scale = value;
                _scaleDirty = true;
            }
        }
        private float _scale;
        private bool _scaleDirty;

        public float angle
        {
            get { return _angle; }
            set
            {
                if (_angle != value) _angleDirty = true;
                _angle = value;
            }
        }
        private float _angle;
        private bool _angleDirty;

        public GameVec2 forward
        {
            get { return _forward; }
            set
            {
                if (value.x == 0 && value.y == 0) return;
                _forward = value;
                _forwardDirty = true;
            }
        }
        private GameVec2 _forward;
        private bool _forwardDirty;

        public GameTransformCom()
        {
            Reset();
        }

        public void Reset()
        {
            _position = GameVec2.zero;
            _scale = 1;
            _angle = 0;
            _forward = GameVec2.up;
            ClearDirty();
        }

        private void ClearDirty()
        {
            _posDirty = false;
            _scaleDirty = false;
            _angleDirty = false;
            _forwardDirty = false;
        }

        public bool CheckDirty()
        {
            bool dirty = _posDirty || _scaleDirty || _angleDirty || _forwardDirty;
            ClearDirty();
            return dirty;
        }

        public GameTransformArgs ToArgs()
        {
            return new GameTransformArgs
            {
                position = position,
                scale = scale,
                angle = angle,
                forward = forward
            };
        }

        public void SetByArgs(in GameTransformArgs trs)
        {
            SetPosition(trs.position);
            SetScale(trs.scale);
            SetAngle(trs.angle);
            SetForward(trs.forward);
        }

        private void SetPosition(GameVec2 v)
        {
            _position = v;
        }

        // 设置缩放
        private void SetScale(float v)
        {
            _scale = v;
        }

        // 设置角度
        private void SetAngle(float v)
        {
            _angle = v;
        }

        // 设置前方
        private void SetForward(GameVec2 v)
        {
            _forward = v;
        }
    }
}
