namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置单元
    /// </summary>
    public class ConfigUnit : IConfigUnit
    {
        #region Field
        /// <summary>
        /// 配置单元名称
        /// </summary>
        private string m_Name;

        /// <summary>
        /// 配置项集合
        /// </summary>
        private IConfigItem[] m_IConfigItems;
        #endregion

        #region Property
        /// <summary>
        /// 获取配置单元名称
        /// </summary>
        /// <value>配置单元名称</value>
        public string Name => this.m_Name;

        /// <summary>
        /// 获取配置项集合
        /// </summary>
        /// <value>配置项集合</value>
        public IConfigItem[] Items => this.m_IConfigItems;
        #endregion

        public ConfigUnit(string name, IConfigItem[] items)
        {
            this.m_Name = name;
            this.m_IConfigItems = items;
        }
    }
}