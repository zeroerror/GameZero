using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.Render
{
    public interface GameShaderEffectDomainApi
    {
        /// <summary>
        /// 在Renderer播放Shader特效
        /// <para>shaderEffectId: 特效 ID</para>
        /// <para>renderer: 渲染器</para>
        /// </summary>
        public void PlayShaderEffect(int shaderEffectId, Renderer renderer);

        /// <summary>
        /// 在所有Renderer播放Shader特效
        /// <para>shaderEffectId: 特效 ID</para>
        /// <para>renderer: 渲染器数组</para>
        /// </summary>
        public void PlayShaderEffect(int shaderEffectId, Renderer[] renderers);

        /// <summary>
        /// 在实体上播放Shader特效
        /// <para>shaderEffectId: 特效 ID</para>
        /// <para>entity: 实体</para>
        /// </summary>
        public void PlayShaderEffect(int shaderEffectId, GameEntityBase entity);

        /// <summary>
        /// 停止Renderer的Shader特效
        /// <para>renderer: 渲染器</para>
        /// </summary>
        public void StopShaderEffect(Renderer renderer);

        /// <summary>
        /// 停止所有Renderer的Shader特效
        /// <para>renderers: 渲染器数组</para>
        /// </summary>
        public void StopShaderEffects(Renderer[] renderers);

        /// <summary>
        /// 停止实体的所有Shader特效
        /// <para>entity: 实体</para>
        /// </summary>
        public void StopAllShaderEffects(GameEntityBase entity);
    }
}