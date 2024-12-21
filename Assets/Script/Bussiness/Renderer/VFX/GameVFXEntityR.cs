using GamePlay.Bussiness.Logic;
using GamePlay.Core;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXEntityR
    {
        public readonly string prefabUrl;

        public int entityId;

        public GameObject go { get; private set; }

        public GameObject attachNode { get; private set; }
        public Vector2 attachOffset { get; private set; }
        public bool isAttachParent { get; private set; }

        public GameParticlePlayCom particleCom { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }

        public bool isPlaying => this.timelineCom.isPlaying;
        public float length => this.particleCom.length;

        private bool _stopDirty = false;

        public GameVFXEntityR(GameObject go, string prefabUrl)
        {
            this.go = go;
            this.prefabUrl = prefabUrl;

            var ps = go.GetComponent<ParticleSystem>();
            if (ps) this.particleCom = new GameParticlePlayCom(ps);
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
            this.go.transform.position = args.position;
            this.go.transform.eulerAngles = new Vector3(0, 0, args.angle);
            var scale = args.scale;
            this.go.transform.localScale = new Vector3(scale.x, scale.y, 1);
            this.timelineCom.Play(args.loopDuration);
            this.go.SetActive(true);
            this.attachNode = args.attachNode;
            this.attachOffset = args.attachOffset;
            this.isAttachParent = args.isAttachParent;
            if (args.isAttachParent) this.go.transform.SetParent(args.attachNode.transform);
            this.go.name = $"VFX_{this.entityId}";
            this._stopDirty = true;
        }

        private void _TickAttach()
        {
            if (this.isAttachParent) return;
            if (this.attachNode == null) return;
            var pos = this.attachNode.transform.position.Add(this.attachOffset);
            this.go.transform.position = pos;
            if (this.attachNode.TryGetSortingLayer(out var order, out var layerName))
            {
                this.go.SetSortingLayer(order + 1, layerName);
            }
        }

        public void Stop()
        {
            this._stopDirty = false;
            this.go.SetActive(false);
        }
    }
}