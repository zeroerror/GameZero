namespace GamePlay.Bussiness.Logic
{
    public class GameBuffConditionEntity_WhenAttributeChange : GameBuffConditionEntityBase
    {
        public GameBuffConditionModel_WhenAttributeChange model { get; private set; }

        public GameBuffConditionEntity_WhenAttributeChange(GameBuffEntity buff, GameBuffConditionModel_WhenAttributeChange model) : base(buff)
        {
            this.model = model;
        }

        protected override bool _Check()
        {

            var entityListA = this._domainApi.entitySelectApi.SelectEntities(this.model.selectorA, this._buff, false);
            var entityListB = this._domainApi.entitySelectApi.SelectEntities(this.model.selectorB, this._buff, false);
            var entityA = entityListA?.Count > 0 ? entityListA[0] : null;
            var entityB = entityListB?.Count > 0 ? entityListB[0] : null;

            var needRefTarget = this.model.refTypeA.IsTargetRef() || this.model.refTypeB.IsTargetRef();
            if (needRefTarget && !entityB)
            {
                // 需要参考目标, 但是目标不存在
                return false;
            }

            var refValueA = this.model.refTypeA.GetRefAttributeValue(entityA, entityB, this.model.valueA, this.model.valueFormatA);
            var refValueB = this.model.refTypeB.GetRefAttributeValue(entityA, entityB, this.model.valueB, this.model.valueFormatB);
            var compareResult = this.model.compareType.Compare(refValueA, refValueB);
            return compareResult;
        }

        public override void Clear()
        {
        }
    }
}