using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using GamePlay.Core;
using GameVec2 = UnityEngine.Vector2;
namespace GamePlay.Bussiness.Logic
{
    public class GameActionModel_CharacterTransform : GameActionModelBase
    {
        public readonly int transRoleId;
        public readonly GameActionModel_CharacterTransformAttribute[] attributeList;

        public GameActionModel_CharacterTransform(
            int typeId,
            GameEntitySelector selector,
            GameActionPreconditionSetModel preconditionSet,
            in GameVec2 randomValueOffset,
            int transRoleId,
            GameActionModel_CharacterTransformAttribute[] attributeList
        ) : base(GameActionType.CharacterTransform, typeId, selector, preconditionSet, randomValueOffset)
        {
            this.transRoleId = transRoleId;
            this.attributeList = attributeList;
        }

        public override GameActionModelBase GetCustomModel(float customParam)
        {
            // 属性数值 = 自定义参数 * 原数值
            var count = this.attributeList?.Length ?? 0;
            var list = new GameActionModel_CharacterTransformAttribute[count];
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var attribute = this.attributeList[i];
                    var cloneA = attribute.Clone();
                    cloneA.value = GameMath.Floor(customParam * attribute.value);
                    list[i] = cloneA;
                }
            }
            return new GameActionModel_CharacterTransform(
                this.typeId,
                this.selector,
                this.preconditionSet,
                this.randomValueOffset,
                this.transRoleId,
                list
            );
        }
    }
}