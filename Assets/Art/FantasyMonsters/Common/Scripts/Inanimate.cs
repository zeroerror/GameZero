using Assets.FantasyMonsters.Common.Scripts.Tweens;
using UnityEngine;

namespace Assets.FantasyMonsters.Common.Scripts
{
    public class Inanimate : Monster
    {
        public ParticleSystem HitParticles;

        /// <summary>
        /// Play scale spring animation.
        /// </summary>
        public override void Spring()
        {
            ScaleSpring.Begin(this, 1f, 1.05f, 40, 2);
            HitParticles.Play();
        }
    }
}