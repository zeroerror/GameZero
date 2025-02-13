using System.Collections.Generic;
using GamePlay.Core;
using static GamePlay.Bussiness.Logic.GameBuffConditionEntityBase;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionSetEntity
    {
        private List<GameBuffConditionEntityBase> _entityList;
        private GameBuffConditionEntity_Duration durationEntity => this._entityList.Find(entity => entity is GameBuffConditionEntity_Duration) as GameBuffConditionEntity_Duration;

        public GameBuffConditionSetEntity(GameBuffEntity buff, GameBuffConditionSetModel model)
        {
            this._entityList = new List<GameBuffConditionEntityBase>();
            if (model.durationModel != null)
            {
                var durationEntity = new GameBuffConditionEntity_Duration(buff, model.durationModel);
                this._entityList.Add(durationEntity);
            }
            if (model.timeIntervalModel != null)
            {
                var timeIntervalEntity = new GameBuffConditionEntity_TimeInterval(buff, model.timeIntervalModel);
                this._entityList.Add(timeIntervalEntity);
            }
            if (model.whenDoActionModel != null)
            {
                var whenDoActionEntity = new GameBuffConditionEntity_WhenDoAction(buff, model.whenDoActionModel);
                this._entityList.Add(whenDoActionEntity);
            }
            if (model.whenRoleStateEnterModel != null)
            {
                var entity = new GameBuffConditionEntity_WhenRoleStateEnter(buff, model.whenRoleStateEnterModel);
                this._entityList.Add(entity);
            }
            if (model.whenUnitCountChangeModel != null)
            {
                var entity = new GameBuffConditionEntity_WhenUnitCountChange(buff, model.whenUnitCountChangeModel);
                this._entityList.Add(entity);
            }
            if (model.whenAttributeChangeModel != null)
            {
                var entity = new GameBuffConditionEntity_WhenAttributeChange(buff, model.whenAttributeChangeModel);
                this._entityList.Add(entity);
            }
        }

        public void Inject(
            FindRoleEntityDelegate findEntity,
            GameDomainApi domainApi,
            ForeachActionRecordDelegate_Dmg forEachActionRecord_Dmg,
            ForeachActionRecordDelegate_Heal forEachActionRecord_Heal,
            ForeachActionRecordDelegate_LaunchProjectile forEachActionRecord_LaunchProjectile,
            ForeachActionRecordDelegate_KnockBack forEachActionRecord_KnockBack,
            ForeachActionRecordDelegate_AttributeModify forEachActionRecord_AttributeModify,
            ForeachActionRecordDelegate_AttachBuff forEachActionRecord_AttachBuff,
            ForeachActionRecordDelegate_SummonRoles forEachActionRecord_SummonRoles,
            ForeachRoleStateRecordDelegate forEachRoleStateRecord
        )
        {
            foreach (var entity in this._entityList) m_inject(entity);
            void m_inject(GameBuffConditionEntityBase conditionEntity)
            {
                if (!conditionEntity) return;
                conditionEntity.FindEntity = findEntity;
                conditionEntity.ForeachActionRecord_Dmg = forEachActionRecord_Dmg;
                conditionEntity.ForeachActionRecord_Heal = forEachActionRecord_Heal;
                conditionEntity.ForeachActionRecord_LaunchProjectile = forEachActionRecord_LaunchProjectile;
                conditionEntity.ForeachActionRecord_KnockBack = forEachActionRecord_KnockBack;
                conditionEntity.ForeachActionRecord_AttributeModify = forEachActionRecord_AttributeModify;
                conditionEntity.ForeachActionRecord_AttachBuff = forEachActionRecord_AttachBuff;
                conditionEntity.ForeachActionRecord_SummonRoles = forEachActionRecord_SummonRoles;
                conditionEntity.ForeachRoleStateRecord = forEachRoleStateRecord;
                conditionEntity.setDomainApi(domainApi);
            }
        }

        public void Clear()
        {
            this._entityList.Foreach(entity => entity.Clear());
        }

        public void Tick(float dt)
        {
            this._entityList.Foreach(entity => entity.Tick(dt));
        }

        /// <summary> 判定条件集合是否存在有效条件 </summary>
        public bool IsValid()
        {
            return this._entityList.Count > 0;
        }

        public bool CheckSatisfied()
        {
            var unsatisfiedEntity = this._entityList.Find(entity => !entity.isSatisfied);
            return unsatisfiedEntity == null;
        }

        public void RefreshTime()
        {
            var durationEntity = this.durationEntity;
            if (!durationEntity) return;
            durationEntity.Clear();
        }

        public void StackTime()
        {
            var durationEntity = this.durationEntity;
            if (!durationEntity) return;
            durationEntity.stackCount_extra += 1;
        }
    }
}