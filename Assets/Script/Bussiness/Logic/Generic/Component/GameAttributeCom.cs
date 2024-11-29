using System.Collections.Generic;

namespace GamePlay.Bussiness.Logic
{
    public class GameAttributeCom
    {

        public List<GameAttribute> attributes { get; private set; }

        public GameAttributeCom()
        {
            this.attributes = new List<GameAttribute>();
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
            args.attributes = this.attributes.GetRange(0, this.attributes.Count);
            return args;
        }

        public void SetByArgs(in GameAttributeArgs args)
        {
        }
    }
}
