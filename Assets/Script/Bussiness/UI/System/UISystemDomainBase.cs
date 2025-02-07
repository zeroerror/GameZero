using GamePlay.Bussiness.Logic;

namespace GamePlay.Bussiness.UI
{
    public abstract class UISystemDomainBase
    {
        protected UIContext _context;

        public UISystemDomainBase()
        {
        }

        public virtual void Inject(UIContext context)
        {
            this._context = context;
            this._BindEvents();
        }

        public virtual void Destroy()
        {
            this._UnbindEvents();
        }

        protected virtual void _BindEvents() { }
        protected virtual void _UnbindEvents() { }
        public virtual void Tick(float dt) { }

        public void OpenUI<T>(UIViewInput input) where T : UIBase
        {
            this._context.domainApi.directorApi.OpenUI<T>(input);
        }

        public void CloseUI<T>() where T : UIBase
        {
            this._context.domainApi.directorApi.CloseUI<T>();
        }
    }
}