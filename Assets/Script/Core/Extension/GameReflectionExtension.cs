using System.Reflection;

namespace GamePlay.Core
{
    public static class GameReflectionExtension
    {
        /// <summary>
        /// 获取对象的字段或属性
        /// <para>fieldName: 字段或属性名称</para>
        /// </summary>
        public static object GetField(this object obj, string fieldName)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (field != null)
            {
                return field.GetValue(obj);
            }

            PropertyInfo property = obj.GetType().GetProperty(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (property != null)
            {
                return property.GetValue(obj);
            }

            return null;
        }
    }
}