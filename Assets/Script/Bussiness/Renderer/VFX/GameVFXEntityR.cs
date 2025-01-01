using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXEntityR
    {
        public readonly string prefabUrl;

        public int entityId;

        public GameObject root { get; private set; }
        private GameObject _body;

        public GameVFXPlayArgs playArgs { get; private set; }

        public GameParticlePlayCom particleCom { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }

        public bool isPlaying => this.timelineCom.isPlaying;
        public float length => this.particleCom.length;

        private bool _stopDirty = false;

        public GameVFXEntityR(GameObject root, GameObject body, string prefabUrl)
        {
            this.root = root;
            this._body = body;
            this.prefabUrl = prefabUrl;

            var ps = body.GetComponent<ParticleSystem>();
            if (ps)
            {
                this.particleCom = new GameParticlePlayCom(ps);
            }
            GameLogger.Assert(ps, $"特效实体: {prefabUrl} 未找到粒子系统");

            this.timelineCom = new GameTimelineCom();
            this.timelineCom.SetLength(this.length);
        }

        public void Destroy()
        {
        }

        public void Tick(float dt)
        {
            this.timelineCom.Tick(dt);
            this.particleCom.Tick(dt);

            this._TickAttach();

            if (!isPlaying && this._stopDirty)
            {
                this.Stop();
            }
        }

        public void Play(in GameVFXPlayArgs args)
        {
            this.particleCom.Play(true);
            this.root.transform.position = args.position;
            this.root.transform.eulerAngles = new Vector3(0, 0, args.angle);

            var scale = args.scale;
            if (scale.x <= 0 || scale.y <= 0)
            {
                scale = Vector2.one;
                GameLogger.LogWarning($"特效实体: {this.prefabUrl} 缩放参数错误 {args.scale}, 使用默认值(1,1)");
            }
            this.root.transform.localScale = new Vector3(scale.x, scale.y, 1);

            this.root.SetActive(true);
            this.timelineCom.Play(args.loopDuration);
            this.playArgs = args;
            if (args.isAttachParent) this.root.transform.SetParent(args.attachNode.transform);
            this.root.name = $"VFX_{this.entityId}";
            this._stopDirty = true;
        }

        private void _TickAttach()
        {
            if (this.playArgs.isAttachParent) return;
            if (this.playArgs.attachNode == null) return;
            this.root.transform.position = this.playArgs.attachNode.transform.position.Add(this.playArgs.attachOffset);
            if (this.playArgs.attachNode.TryGetSortingLayer(out var order, out var layerName))
            {
                order += 1;
                order += this.playArgs.orderOffset;
                this.root.SetSortingLayer(order, layerName);
            }
        }

        public void Stop()
        {
            this._stopDirty = false;
            this.root.SetActive(false);
        }
    }
}