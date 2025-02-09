using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public static class UIPathUtil
    {
        /// <summary>
        /// 获取角色头像路径
        /// <para>typeId: 角色类型ID</para>
        /// </summary>
        public static string GetRoleHead(int typeId)
        {
            return "UI/System/Common/Image/RoleHead/" + typeId;
        }

        /// <summary>
        /// 获取属性图标路径
        /// <para>type: 属性类型</para>
        /// </summary>
        public static string GetAttributeIcon(GameAttributeType type)
        {
            return "UI/System/Common/Image/Attribute/" + type;
        }
    }
}