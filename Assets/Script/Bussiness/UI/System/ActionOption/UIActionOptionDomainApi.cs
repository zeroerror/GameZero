using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public interface UIActionOptionDomainApi
    {
        /// <summary>
        /// 选择选项
        /// <para>option: 选项</para>
        /// </summary>
        public int ChooseOption(GameActionOptionModel option);
    }
}