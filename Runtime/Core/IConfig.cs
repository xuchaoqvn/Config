namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置接口
    /// </summary>
    public interface IConfig
    {
        #region Property
        /// <summary>
        /// 获取或设置配置路径
        /// </summary>
        /// <value>配置路径</value>
        string ConfigPath
        {
            get;
            set;
        }

        /// <summary>
        /// 获取配置单元
        /// </summary>
        /// <value>配置单元</value>
        IConfigUnit[] ConfigUnits
        {
            get;
        }
        #endregion

        #region Function
        /// <summary>
        /// 读取配置
        /// </summary>
        void Read();

        /// <summary>
        /// 写入配置
        /// </summary>
        void Save();
        #endregion
    }
}