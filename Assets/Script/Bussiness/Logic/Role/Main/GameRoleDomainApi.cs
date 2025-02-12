using System;
using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public interface GameRoleDomainApi
    {
        public GameRoleFSMDomainApi fsmApi { get; }

        /// <summary>
        /// 获取角色模板    
        /// </summary>
        public GameRoleTemplate GetRoleTemplate();

        /// <summary>
        /// 创建角色
        /// <para>typeId: 类型Id</para>
        /// <para>campId: 阵营Id</para>
        /// <para>transArgs: 变换参数</para>
        /// <para>isUser: 是否为用户 </para>
        /// </summary>
        public GameRoleEntity CreateRole(int typeId, int campId, in GameTransformArgs transArgs, bool isUser);

        /// <summary>
        /// 创建玩家角色
        /// <para>typeId: 类型Id</para>
        /// <para>transArgs: 变换参数</para>
        /// <para>isUser: 是否为用户 </para>
        /// </summary>
        public GameRoleEntity CreatePlayerRole(int typeId, in GameTransformArgs transArgs, bool isUser);

        /// <summary>
        /// 创建怪物角色
        /// <para>typeId: 类型Id</para>
        /// <para>transArgs: 变换参数</para>
        /// </summary>
        public GameRoleEntity CreateEnemyRole(int typeId, in GameTransformArgs transArgs);

        /// <summary>
        /// 遍历所有角色
        /// <para>action: 行为</para>
        /// </summary>
        public void ForeachAllRoles(Action<GameRoleEntity> action);

        /// <summary>
        /// 根据实体Id查找角色
        /// <para>entityId: 实体Id</para>
        /// </summary>
        public GameRoleEntity FindByEntityId(int entityId);

        /// <summary>
        /// 移除所有角色
        /// </summary>
        public void RemoveAllRoles();

        /// <summary>
        /// 获取最近的敌人
        /// <para>entity: 实体</para>
        /// </summary>
        public GameRoleEntity GetNearestEnemy(GameEntityBase entity);

        /// <summary>
        /// 获取用户角色
        /// </summary>
        public GameRoleEntity GetUserRole();

        /// <summary>
        /// 获取指定阵营的所有角色
        /// <para>campId: 阵营Id</para>
        /// </summary>
        public List<GameRoleEntity> GetCampRoles(int campId);

        /// <summary>
        /// 获取角色输入
        /// <para>entityId: 实体Id</para>
        /// <para>input: 输入</para>
        /// </summary>
        public bool TryGetPlayerInput(int entityId, out GameRoleInputArgs input);

        /// <summary>
        /// 设置玩家角色输入
        /// <para>input: 输入</para>
        /// </summary>
        public void SetUserRoleInput(in GameRoleInputArgs input);

        /// <summary>
        /// 设置角色输入
        /// <para>input: 输入</para>
        /// </summary>
        public void SetRoleInput(int entityId, in GameRoleInputArgs input);

        /// <summary>
        /// 变换角色
        /// <para>role: 角色</para>
        /// <para>transRoleId: 变换角色Id</para>
        /// <para>record: 变换记录</para>
        /// </summary>
        public void TransformRole(GameRoleEntity role, int transRoleId, in GameActionRecord_CharacterTransform record);

        /// <summary>
        /// 移除所有角色的Buff
        /// </summary>
        public void DetachAllRolesBuffs();

        /// <summary>
        /// 召唤角色
        /// <para>model: 召唤角色行为模型</para>
        /// <para>summoner: 召唤者</para>
        /// <para>transArgs: 变换参数</para>
        /// </summary>
        public GameRoleEntity[] SummonRoles(GameActionModel_SummonRoles action, GameEntityBase summoner, in GameTransformArgs transArgs);

        /// <summary>
        /// 召唤角色
        /// <para>summoner: 召唤者</para>
        /// <para>typeId: 类型Id</para>
        /// <para>campType: 阵营类型</para>
        /// <para>count: 个数</para>
        /// <para>transArgs: 变换参数</para>
        /// </summary>
        public GameRoleEntity[] SummonRoles(
            GameEntityBase summoner,
            int typeId,
            GameCampType campType,
            int count,
            in GameTransformArgs transArgs
        );
    }
}
