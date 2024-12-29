using System;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameBuffConditionEntityBase
    {
        public bool isSatisfied { get; private set; }

        protected GameBuffEntity _buff;

        // 查找角色实体
        public delegate GameEntityBase FindRoleEntityDelegate(GameEntityType entityType, int entityId);
        public FindRoleEntityDelegate FindEntity;

        // 伤害
        public delegate void ForEachActionRecordDelegate_Dmg(in Action<GameActionRecord_Dmg> actionRecord);
        public ForEachActionRecordDelegate_Dmg ForEachActionRecord_Dmg;
        // 治疗
        public delegate void ForEachActionRecordDelegate_Heal(in Action<GameActionRecord_Heal> actionRecord);
        public ForEachActionRecordDelegate_Heal ForEachActionRecord_Heal;
        // 发射投射物
        public delegate void ForEachActionRecordDelegate_LaunchProjectile(in Action<GameActionRecord_LaunchProjectile> actionRecord);
        public ForEachActionRecordDelegate_LaunchProjectile ForEachActionRecord_LaunchProjectile;
        // 击退
        public delegate void ForEachActionRecordDelegate_KnockBack(in Action<GameActionRecord_KnockBack> actionRecord);
        public ForEachActionRecordDelegate_KnockBack ForEachActionRecord_KnockBack;
        // 属性修改
        public delegate void ForEachActionRecordDelegate_AttributeModify(in Action<GameActionRecord_AttributeModify> actionRecord);
        public ForEachActionRecordDelegate_AttributeModify ForEachActionRecord_AttributeModify;
        // 挂载buff
        public delegate void ForEachActionRecordDelegate_AttachBuff(in Action<GameActionRecord_AttachBuff> actionRecord);
        public ForEachActionRecordDelegate_AttachBuff ForEachActionRecord_AttachBuff;
        // 召唤角色
        public delegate void ForEachActionRecordDelegate_SummonRoles(in Action<GameActionRecord_SummonRoles> actionRecord);
        public ForEachActionRecordDelegate_SummonRoles ForEachActionRecord_SummonRoles;

        public GameBuffConditionEntityBase(GameBuffEntity buff)
        {
            _buff = buff;
        }

        public void Tick(float dt)
        {
            _Tick(dt);
            isSatisfied = _Check();
        }

        protected virtual void _Tick(float dt) { }
        protected abstract bool _Check();
        public virtual void Clear() { }

        public static bool operator !(GameBuffConditionEntityBase condition)
        {
            return condition == null;
        }

        public static bool operator true(GameBuffConditionEntityBase condition)
        {
            return condition != null;
        }

        public static bool operator false(GameBuffConditionEntityBase condition)
        {
            return condition == null;
        }
    }
}