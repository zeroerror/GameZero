namespace GamePlay.Bussiness.Logic
{
    public interface GameSkillDomainApi
    {
        public GameSkillEntity CreateSkill(GameRoleEntity role, int typeId);

        /// <summary> 尝试获取技能模型 </summary>
        public bool TryGetModel(int typeId, out GameSkillModel model);

        /// <summary>
        /// 检查技能条件, 包含CD、消耗、选择器等
        /// <para> role: 施法者 </para>
        /// <para> skill: 技能 </para>
        /// <para> target: 施法目标 </para>
        /// <para> ignoreDistanceCondition: 是否忽略距离 </para>
        /// </summary>
        public bool CheckSkillCondition(GameRoleEntity role, GameSkillEntity skill, GameEntityBase target, bool ignoreDistanceCondition = false);

        /// <summary>
        /// 检查施法条件, 包括角色状态条件和技能条件
        /// <para> role: 施法者 </para>
        /// <para> skill: 技能 </para>
        /// <para> inputArgs: 输入参数 </para>
        /// <para> ignoreDistanceCondition: 是否忽略距离 </para>
        /// </summary>
        public bool CheckRoleCastCondition(GameRoleEntity role, GameSkillEntity skill, in GameRoleInputArgs inputArgs, bool ignoreDistanceCondition = false);

        /// <summary> 根据技能的目标选取组件, 检查施法条件, 包括角色状态条件和技能条件 </summary>
        public bool CheckCastCondition(GameRoleEntity role, GameSkillEntity skill);

        /// <summary>
        /// 查找针对目标的可施法技能
        /// <para> role: 施法者 </para>
        /// <para> target: 施法目标 </para>
        /// <para> ignoreDistanceCondition: 是否忽略距离, 默认为true, 不会检查距离条件 </para>
        /// </summary>
        public GameSkillEntity FindCastableSkill(GameRoleEntity role, GameEntityBase target, bool ignoreDistanceCondition = true);

        /// <summary> 施放技能 </summary>
        public void CastSkill(GameRoleEntity role, GameSkillEntity skill);

        /// <summary>
        /// 执行角色的被动技能
        /// </summary>
        /// <param name="role"></param>
        public void DoPassiveSkill(GameRoleEntity role);

    }
}
