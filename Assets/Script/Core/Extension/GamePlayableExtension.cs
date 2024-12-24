using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GamePlay.Core
{
    public static class GamePlayableExtension
    {
        /// <summary>
        /// 将graph与指定layer下的graph对比, 判定是否相同
        /// <para>layer: 指定的层级</para>
        /// <para>graph: 待对比的graph</para>
        /// </summary>
        public static bool IsGraphEquals(this AnimationLayerMixerPlayable mixerPlayable, int layer, in PlayableGraph graph)
        {
            if (layer >= mixerPlayable.GetInputCount()) return false;
            var g = mixerPlayable.GetInput(layer).GetGraph();
            return g.Equals(graph);
        }
    }
}