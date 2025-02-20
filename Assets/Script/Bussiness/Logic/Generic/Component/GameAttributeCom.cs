using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Infrastructure;

namespace GamePlay.Bussiness.Logic
{
    public class GameAttributeCom
    {
        private List<GameAttribute> _attributes;

        public GameAttributeCom()
        {
            this._attributes = new List<GameAttribute>();
        }

        public void Clear()
        {
            this._attributes.Clear();
        }

        public GameAttributeCom Clone()
        {
            var com = new GameAttributeCom();
            com.CopyFrom(this);
            return com;
        }

        public void CopyFrom(GameAttributeCom com)
        {
            this._attributes = new List<GameAttribute>(com._attributes);
        }

        public override string ToString()
        {
            var str = "";
            foreach (var attr in this._attributes)
            {
                str += attr.ToString() + "\n";
            }
            return str;
        }

        public GameAttributeArgs ToArgs()
        {
            var args = new GameAttributeArgs();
            args.attributes = this._attributes.ToArray();
            return args;
        }

        public void SetByArgs(in GameAttributeArgs args)
        {
            if (args.attributes == null) return;
            this._attributes = new List<GameAttribute>(args.attributes);
        }

        public void SetAttribute(in GameAttribute attribute)
        {
            this.SetAttribute(attribute.type, attribute.value);
        }

        public void SetAttribute(GameAttributeType type, float value)
        {
            var oldV = this.GetValue(type);
            if (oldV == value) return;
            this._isDirty = true;

            var index = this._attributes.FindIndex((a) => a.type == type);
            if (index == -1) this._attributes.Add(new GameAttribute() { type = type, value = value });
            else this._attributes[index] = new GameAttribute() { type = type, value = value };
        }

        public bool CheckDirty()
        {
            return this._isDirty;
        }
        public void ClearDirty()
        {
            this._isDirty = false;
        }
        private bool _isDirty = false;

        public bool TryGetAttribute(GameAttributeType type, out GameAttribute attribute)
        {
            attribute = default;
            var idx = this._attributes.FindIndex((a) => a.type == type);
            if (idx == -1)
            {
                return false;
            }
            attribute = this._attributes[idx];
            return true;
        }

        public float GetValue(GameAttributeType type)
        {
            var index = this._attributes.FindIndex((a) => a.type == type);
            if (index == -1) return 0;
            return this._attributes[index].value;
        }

        /// <summary>
        /// 获取属性值的字符串
        /// <para>大于等于1000, 转化为k, 保留一位小数</para>
        /// <para>大于等于10000, 转化为w, 保留一位小数</para>
        /// <para>大于等于1000 0000, 转化为kw, 保留一位小数</para>
        /// </summary>
        public string GetValueStr(GameAttributeType type)
        {
            var value = this.GetValue(type);
            return value.GetFormatNumStr();
        }

        public void Foreach(System.Action<GameAttribute> action)
        {
            this._attributes.Foreach(action);
        }
    }
}
