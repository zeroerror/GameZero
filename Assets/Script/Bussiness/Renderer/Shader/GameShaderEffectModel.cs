namespace GamePlay.Bussiness.Render
{
    public class GameShaderEffectModel
    {
        public readonly int typeId;
        public readonly string desc;
        public readonly string shaderUrl;
        public readonly int loopCount;
        public readonly GameShaderEffectPropTimeLineModel[] propTimeLines;

        public readonly float length;

        public GameShaderEffectModel(
            int typeId,
            string desc,
            string shaderUrl,
            int loopCount,
            GameShaderEffectPropTimeLineModel[] propTimeLines
        )
        {
            this.typeId = typeId;
            this.desc = desc;
            this.shaderUrl = shaderUrl;
            this.loopCount = loopCount == 0 ? int.MaxValue : loopCount;
            this.propTimeLines = propTimeLines;

            foreach (var timeLine in propTimeLines)
            {
                if (timeLine.endTime > this.length)
                {
                    this.length = timeLine.endTime;
                }
            }
        }

        public override string ToString()
        {
            return $"GameShaderEffectModel: 描述={desc}, shader路径={shaderUrl}, 循环次数={loopCount}, 参数时间轴={propTimeLines}";
        }
    }
}