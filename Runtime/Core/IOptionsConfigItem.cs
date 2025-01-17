namespace SimpleFramework.Config
{
    /// <summary>
    /// 选项配置项接口
    /// </summary>
    public interface IOptionsConfigItem
    {
        #region Property
        /// <summary>
        /// 获取或设置配置项默认选项索引
        /// </summary>
        /// <value>配置项默认选项索引</value>
        int DefaultIndex
        {
            get;
        }

        /// <summary>
        /// 获取或设置配置项选项索引
        /// </summary>
        /// <value>配置项选项索引</value>
        int Index
        {
            get;
            set;
        }

        /// <summary>
        /// 配置项选项
        /// </summary>
        /// <value></value>
        string[] Options
        {
            get;
        }
        #endregion
    }
}