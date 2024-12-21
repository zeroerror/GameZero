
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameTransformCom
    {
        public bool isEnable = true;
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

        public GameVec2 scale
        {
            get { return _scale; }
            set
            {
                if (value == _scale) return;
                _scale = value;
                _scaleDirty = true;
            }
        }
        private GameVec2 _scale;
        private bool _scaleDirty;

        public float angle
        {
            get { return _angle; }
            set
            {
                if (isLockRotation) return;
                if (_angle != value) _angleDirty = true;
                _angle = value;
            }
        }
        private float _angle;
        private bool _angleDirty;

        public bool isLockRotation { get; set; }

        public GameVec2 forward
        {
            get { return _forward; }
            set
            {
                if (isLockRotation) return;
                if (value.x == 0 && value.y == 0) return;
                _forward = value;
                _forwardDirty = true;
                this._angle = GameVec2.SignedAngle(GameVec2.right, value);
            }
        }
        private GameVec2 _forward;
        private bool _forwardDirty;

        public GameTransformCom()
        {
            Clear();
        }

        public void Clear()
        {
            this.position = GameVec2.zero;
            this.scale = GameVec2.one;
            this.angle = 0;
            this.forward = GameVec2.up;
            ClearDirty();
        }

        private void ClearDirty()
        {
            this._posDirty = false;
            this._scaleDirty = false;
            this._angleDirty = false;
            this._forwardDirty = false;
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
            this.forward = trs.forward;
            this.position = trs.position;
            this.scale = trs.scale;
            this.angle = trs.angle;
        }
    }
}
