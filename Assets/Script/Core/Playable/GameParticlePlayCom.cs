using UnityEngine;

namespace GamePlay.Core
{
    public class GameParticlePlayCom
    {
        public ParticleSystem root { get; private set; }

        public bool isPlaying { get; private set; }

        public float length => this.root.main.duration;

        public float time { get; private set; }
        public float timeScale { get; set; } = 1.0f;

        public GameParticlePlayCom(ParticleSystem main)
        {
            this.root = main;
        }

        public void Tick(float dt)
        {
            if (!this.isPlaying) return;
            dt *= this.timeScale;
            this.root.Simulate(dt, true, false);
            if (this.time >= this.length)
            {
                this.Stop();
            }
        }

        public void Play()
        {
            this.isPlaying = true;
        }

        public void Stop()
        {
            this.root.Stop();
            this.isPlaying = false;
        }
    }
}