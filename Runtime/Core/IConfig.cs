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

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="unitName">配置单元名称</param>
        /// <param name="itemName">配置项名称</param>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <returns>>配置</returns>
        bool GetConfig<T>(string unitName, string itemName, out T t);
        #endregion
    }
}