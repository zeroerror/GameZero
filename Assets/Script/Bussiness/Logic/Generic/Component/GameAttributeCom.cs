using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameAttributeCom
    {

        private List<GameAttribute> _attributes;

        public GameAttributeCom()
        {
            this._attributes = new List<GameAttribute>();
        }

        public void Reset()
        {
        }

        public override string ToString()
        {
            return $"";
        }

        public GameAttributeArgs ToArgs()
        {
            var args = new GameAttributeArgs();
            args.attributes = this._attributes.GetRange(0, this._attributes.Count);
            return args;
        }

        public void SetByArgs(in GameAttributeArgs args)
        {
        }

        public void SetAttribute(GameAttribute attribute)
        {
            var index = this._attributes.FindIndex((a) => a.type == attribute.type);
            if (index == -1) this._attributes.Add(attribute);
            else this._attributes[index] = attribute;
        }

        public void GetAttribute(GameAttributeType type, out GameAttribute attribute)
        {
            attribute = this._attributes.Find((a) => a.type == type);
        }

        public float GetValue(GameAttributeType type)
        {
            var index = this._attributes.FindIndex((a) => a.type == type);
            if (index == -1) return 0;
            return this._attributes[index].value;
        }
    }
}
