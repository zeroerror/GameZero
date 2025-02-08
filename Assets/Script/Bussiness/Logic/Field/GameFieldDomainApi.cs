namespace GamePlay.Bussiness.Logic
{
    public interface GameFieldDomainApi
    {
        /// <summary>
        /// 加载场景
        /// <para>fieldId: 场景ID</para>
        /// </summary>
        public GameFieldEntity LoadField(int fieldId);

        /// <summary>
        /// 销毁场景
        /// <para>field: 场景实体</para>
        /// </summary>
        public void DestroyField(GameFieldEntity field);
    }
}