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
        public GameObject body { get; private set; }

        public GameVFXPlayArgs playArgs { get; private set; }

        public GameParticlePlayCom particleCom { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }

        public bool isPlaying => this.timelineCom.isPlaying;
        public float length => this.particleCom?.length ?? -1;

        private bool _stopDirty = false;

        public GameVFXEntityR(GameObject body, string prefabUrl)
        {
            this.root = new GameObject();
            body.transform.SetParent(this.root.transform);
            body.name = "Body";
            this.body = body;
            this.prefabUrl = prefabUrl;

            var ps = body.GetComponent<ParticleSystem>();
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
            this.root.transform.position = args.position;
            this.root.transform.eulerAngles = new Vector3(0, 0, args.angle);
            this.root.transform.localScale = new Vector3(args.scale.x, args.scale.y, 1);

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
            this.root.transform.position = this.playArgs.attachNode.transform.position;
            this.body.transform.position = this.playArgs.attachOffset;
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
            this.body.SetActive(false);
        }
    }
}