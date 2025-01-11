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
        public void SetPlayArgs(in GameVFXPlayArgs args) => this.playArgs = args;

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
            GameObject.Destroy(this.root);
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
            this._UpdateLayerOrder();
        }

        private void _UpdateLayerOrder()
        {
            var vfxTrans = this.root.transform;
            vfxTrans.TryGetSortingLayer(out var order, out var layerName);

            var layerType = this.playArgs.layerType;
            var newOrder = 0;
            var attachNode = this.playArgs.attachNode;
            if (layerType == GameFieldLayerType.Entity && attachNode)
            {
                // 实体层特效，层级相对于挂载节点
                attachNode.TryGetSortingOrder(out newOrder, out var attachLayer);
            }
            else
            {
                // 其余层级，层级相对于特效位置
                newOrder = GameFieldLayerCollection.GetLayerOrder(layerType, vfxTrans.position);
            }

            if (order == newOrder) return;
            vfxTrans.SetSortingOrder(newOrder, layerName);
        }

        public void Play(GameVFXPlayArgs args)
        {
            if (args.scale.x <= 0 || args.scale.y <= 0)
            {
                GameLogger.LogWarning($"特效实体: {this.prefabUrl} 缩放参数错误 {args.scale}, 使用默认缩放(1,1)");
                args.scale = Vector2.one;
            }
            if (args.layerType == GameFieldLayerType.None)
            {
                GameLogger.LogWarning($"特效实体: {this.prefabUrl} 层级类型未设置, 使用默认层级VFX");
                args.layerType = GameFieldLayerType.VFX;
            }

            this.playArgs = args;
            this.root.transform.position = args.position;
            this.root.transform.eulerAngles = new Vector3(0, 0, args.angle);
            this.root.transform.localScale = args.scale;
            this.particleCom.Play(true);
            this.root.SetActive(true);
            this.timelineCom.Play(args.loopDuration);
            if (args.isAttachParent) this.SetParent(args.attachNode.transform);
            this.root.name = $"VFX_{this.entityId}_{this.prefabUrl}";
            this._stopDirty = true;
        }

        private void _TickAttach()
        {
            if (this.playArgs.isAttachParent) return;
            if (this.playArgs.attachNode == null) return;
            this.root.transform.position = this.playArgs.attachNode.transform.position.Add(this.playArgs.attachOffset);
        }

        public void Stop()
        {
            this._stopDirty = false;
            this.root.SetActive(false);
        }

        public void SetParent(Transform parent)
        {
            this.root.transform.SetParent(parent);
        }
    }
}