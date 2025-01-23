using GamePlay.Bussiness.Logic;
using UnityEngine;

namespace GamePlay.Bussiness.UI
{
    public interface UIJumpTextDomainApi
    {
        /// <summary>
        /// 伤害跳字
        /// <para> pos: 位置 </para>
        /// <para> dmgType: 伤害类型 </para>
        /// <para> style: 样式 </para>
        /// <para> txt: 文本 </para>
        /// </summary>
        public void JumpText_Dmg(in Vector2 screenPos, GameActionDmgType dmgType, int style, string txt, float scale = 1.0f);
    }
}