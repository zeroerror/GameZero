namespace GamePlay.Bussiness.UI
{
    public abstract class UIMgrBase<T> where T : UIMgrBase<T>, new()
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
        private static T _instance;
    }
}