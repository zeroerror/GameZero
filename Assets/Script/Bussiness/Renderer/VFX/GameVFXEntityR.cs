using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Renderer
{
    public class GameVFXEntityR
    {
        public int entityId;

        public GameObject go { get; private set; }

        public GameObject attachNode { get; private set; }
        public Vector2 attachOffset { get; private set; }
        public bool isAttachParent { get; private set; }

        public GamePlayableCom animCom { get; private set; }
        public GameTimelineCom timelineCom { get; private set; }

        public bool isPlaying => this.timelineCom.isPlaying;

        public GameVFXEntityR(GameObject go)
        {
            this.go = go;
            var animator = go.GetComponent<Animator>();
            this.animCom = new GamePlayableCom(animator);
            this.timelineCom = new GameTimelineCom();
        }

        public void Play(in GameVFXPlayArgs args)
        {
            var clip = args.clip;
            this.animCom.Play(clip);
            this.go.transform.position = args.position;
            this.go.transform.eulerAngles = new Vector3(0, 0, args.angle);
            var localScale = this.go.transform.localScale;
            this.go.transform.localScale = new Vector3(localScale.x * args.scale, localScale.y * args.scale, localScale.z);
            this.timelineCom.SetLength(clip.length);
            this.timelineCom.Play(args.loopDuration);
            this.go.SetActive(true);
            this.attachNode = args.attachNode;
            this.attachOffset = args.attachOffset;
            this.isAttachParent = args.isAttachParent;
            if (args.isAttachParent) this.go.transform.SetParent(args.attachNode.transform);

            this.go.name = $"VFX_{clip.name}_{this.entityId}";
        }

        public void Tick(float dt)
        {
            this.animCom.Tick(dt);
            this.timelineCom.Tick(dt);
            this._TickAttach();
            if (!isPlaying)
            {
                this.Stop();
            }
        }

        private void _TickAttach()
        {
            if (this.isAttachParent) return;
            if (this.attachNode == null) return;
            var pos = this.attachNode.transform.position.Add(this.attachOffset);
            this.go.transform.position = pos;
        }

        public void Stop()
        {
            this.go.SetActive(false);
        }
    }
}