using System.Collections.Generic;
using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public class UIActionOptionSelectModel
    {
        public int lv;
        public GameActionOptionModel option;
    }

    public class UIActionOptionModel
    {
        /// <summary> 已选列表 </summary>
        public List<UIActionOptionSelectModel> selectedOptions;

        public UIActionOptionModel()
        {
            this.selectedOptions = new List<UIActionOptionSelectModel>();
        }

        public bool TryGetOption(GameActionOptionModel option, out UIActionOptionSelectModel selectModel)
        {
            selectModel = this.selectedOptions.Find(model => model.option.typeId == option.typeId);
            return selectModel != null;
        }

        public void Add(UIActionOptionSelectModel selectModel)
        {
            this.selectedOptions.Add(selectModel);
        }
    }
}