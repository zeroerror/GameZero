using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public interface GameShaderEffectDomainApi
    {
        /// <summary>
        /// 播放 Shader 特效
        /// <para>shaderEffectId: 特效 ID</para>
        /// <para>renderer: 渲染器</para>
        /// </summary>
        public void PlayShaderEffect(int shaderEffectId, Renderer renderer);

        /// <summary>
        /// 播放 Shader 特效
        /// <para>shaderEffectId: 特效 ID</para>
        /// <para>renderer: 渲染器数组</para>
        /// </summary>
        public void PlayShaderEffect(int shaderEffectId, Renderer[] renderers);
    }
}