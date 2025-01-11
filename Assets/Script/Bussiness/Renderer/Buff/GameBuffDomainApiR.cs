using System.Collections.Generic;

namespace GamePlay.Bussiness.Renderer
{
    public interface GameBuffDomainApiR
    {
        public bool TryGetBuffModel(int buffId, out GameBuffModelR buffModel);
        public List<GameBuffModelR> GetBuffModelList();

        /// <summary>
        /// 转移buff组件内容, 包括buff列表, 以及buff特效节点挂载转移
        /// <para> 参考的buff组件 </para>
        /// <para> 目标角色 </para>
        /// </summary>
        public void TranserBuffCom(GameBuffComR refBuffCom, GameRoleEntityR targetRole);
    }
}