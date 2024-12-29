using System.Collections.Generic;
using static GamePlay.Bussiness.Logic.GameBuffConditionEntityBase;

namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionSetEntity
    {
        public GameBuffConditionEntity_Duration durationEntity { get; private set; }
        public GameBuffConditionEntity_TimeInterval timeIntervalEntity { get; private set; }
        public GameBuffConditionEntity_WhenDoAction whenDoActionEntity { get; private set; }

        private List<GameBuffConditionEntityBase> _entityList;

        public GameBuffConditionSetEntity(GameBuffEntity buff, GameBuffConditionSetModel model)
        {
            this._entityList = new List<GameBuffConditionEntityBase>();
            if (model.durationModel != null)
            {
                this.durationEntity = new GameBuffConditionEntity_Duration(buff, model.durationModel);
                this._entityList.Add(this.durationEntity);
            }
            if (model.timeIntervalModel != null)
            {
                this.timeIntervalEntity = new GameBuffConditionEntity_TimeInterval(buff, model.timeIntervalModel);
                this._entityList.Add(this.timeIntervalEntity);
            }
            if (model.whenDoActionModel != null)
            {
                this.whenDoActionEntity = new GameBuffConditionEntity_WhenDoAction(buff, model.whenDoActionModel);
                this._entityList.Add(this.timeIntervalEntity);
            }
        }

        public void Inject(
            ForEachActionRecordDelegate_Dmg forEachActionRecord_Dmg,
            ForEachActionRecordDelegate_Heal forEachActionRecord_Heal,
            ForEachActionRecordDelegate_LaunchProjectile forEachActionRecord_LaunchProjectile,
            ForEachActionRecordDelegate_KnockBack forEachActionRecord_KnockBack,
            ForEachActionRecordDelegate_AttributeModify forEachActionRecord_AttributeModify,
            ForEachActionRecordDelegate_AttachBuff forEachActionRecord_AttachBuff,
            ForEachActionRecordDelegate_SummonRoles forEachActionRecord_SummonRoles
        )
        {
            foreach (var entity in this._entityList) m_inject(entity);
            void m_inject(GameBuffConditionEntityBase conditionEntity)
            {
                if (!conditionEntity) return;
                conditionEntity.ForEachActionRecord_Dmg = forEachActionRecord_Dmg;
                conditionEntity.ForEachActionRecord_Heal = forEachActionRecord_Heal;
                conditionEntity.ForEachActionRecord_LaunchProjectile = forEachActionRecord_LaunchProjectile;
                conditionEntity.ForEachActionRecord_KnockBack = forEachActionRecord_KnockBack;
                conditionEntity.ForEachActionRecord_AttributeModify = forEachActionRecord_AttributeModify;
                conditionEntity.ForEachActionRecord_AttachBuff = forEachActionRecord_AttachBuff;
                conditionEntity.ForEachActionRecord_SummonRoles = forEachActionRecord_SummonRoles;
            }
        }

        public void Clear()
        {
            this._entityList.ForEach(entity => entity.Clear());
        }

        public void Tick(float dt)
        {
            this._entityList.ForEach(entity => entity.Tick(dt));
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
            if (!this.durationEntity) return;
            this.durationEntity.Clear();
        }

        public void StackTime()
        {
            if (!this.durationEntity) return;
            this.durationEntity.extraStackCount += 1;
        }
    }
}