namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置基类
    /// </summary>
    public abstract class BaseConfig : IConfig
    {
        #region Field
        /// <summary>
        /// 配置路径
        /// </summary>
        protected string m_ConfigPath;

        /// <summary>
        /// 配置单元集合
        /// </summary>
        protected IConfigUnit[] m_Configunits;
        #endregion

        #region Property
        /// <summary>
        /// 获取配置单元集合
        /// </summary>
        public IConfigUnit[] ConfigUnits => this.m_Configunits;

        /// <summary>
        /// 获取或设置配置路径
        /// </summary>
        /// <value>配置路径</value>
        public string ConfigPath
        {
            get => this.m_ConfigPath;
            set => this.m_ConfigPath = value;
        }
        #endregion

        #region Function
        /// <summary>
        /// 读取配置
        /// </summary>
        public abstract void Read();

        /// <summary>
        /// 写入配置
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="unitName">配置单元名称</param>
        /// <param name="itemName">配置项名称</param>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <returns>>配置</returns>
        public abstract bool GetConfig<T>(string unitName, string itemName, out T t);
        #endregion
    }
}