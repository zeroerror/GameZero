using System;

namespace GamePlay.Bussiness.Logic
{
    public abstract class GameBuffConditionEntityBase
    {
        public bool isSatisfied => _isSatisfied;
        private bool _isSatisfied;

        protected GameBuffEntity _buff;

        #region [========================================查找角色实体]
        public delegate GameEntityBase FindRoleEntityDelegate(GameEntityType entityType, int entityId);
        public FindRoleEntityDelegate FindEntity;
        #endregion

        #region [========================================查找行为记录]
        // 伤害
        public delegate void ForeachActionRecordDelegate_Dmg(in Action<GameActionRecord_Dmg> actionRecord);
        public ForeachActionRecordDelegate_Dmg ForeachActionRecord_Dmg;
        // 治疗
        public delegate void ForeachActionRecordDelegate_Heal(in Action<GameActionRecord_Heal> actionRecord);
        public ForeachActionRecordDelegate_Heal ForeachActionRecord_Heal;
        // 发射投射物
        public delegate void ForeachActionRecordDelegate_LaunchProjectile(in Action<GameActionRecord_LaunchProjectile> actionRecord);
        public ForeachActionRecordDelegate_LaunchProjectile ForeachActionRecord_LaunchProjectile;
        // 击退
        public delegate void ForeachActionRecordDelegate_KnockBack(in Action<GameActionRecord_KnockBack> actionRecord);
        public ForeachActionRecordDelegate_KnockBack ForeachActionRecord_KnockBack;
        // 属性修改
        public delegate void ForeachActionRecordDelegate_AttributeModify(in Action<GameActionRecord_AttributeModify> actionRecord);
        public ForeachActionRecordDelegate_AttributeModify ForeachActionRecord_AttributeModify;
        // 挂载buff
        public delegate void ForeachActionRecordDelegate_AttachBuff(in Action<GameActionRecord_AttachBuff> actionRecord);
        public ForeachActionRecordDelegate_AttachBuff ForeachActionRecord_AttachBuff;
        // 召唤角色
        public delegate void ForeachActionRecordDelegate_SummonRoles(in Action<GameActionRecord_SummonRoles> actionRecord);
        public ForeachActionRecordDelegate_SummonRoles ForeachActionRecord_SummonRoles;
        #endregion

        #region [========================================查找状态记录]
        public delegate void ForeachRoleStateRecordDelegate(in Action<GameRoleStateRecord> stateRecord);
        public ForeachRoleStateRecordDelegate ForeachRoleStateRecord;
        #endregion

        public GameBuffConditionEntityBase(GameBuffEntity buff)
        {
            _buff = buff;
        }

        public void Tick(float dt)
        {
            _Tick(dt);
            this._isSatisfied = _Check();
        }

        protected virtual void _Tick(float dt) { }
        protected abstract bool _Check();
        public virtual void Clear()
        {
            this._isSatisfied = false;
        }

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