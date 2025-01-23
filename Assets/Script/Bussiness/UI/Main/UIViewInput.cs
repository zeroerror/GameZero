using System;
namespace GamePlay.Bussiness.UI
{
    public class UIViewInput
    {
        public Action closeAction { get; private set; }
        public object customData { get; private set; }

        public UIViewInput(object customData, Action closeAction = null)
        {
            this.closeAction = closeAction;
            this.customData = customData;
        }
    }
}