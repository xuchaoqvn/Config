namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置单元
    /// </summary>
    public interface IConfigUnit
    {
        #region Property
        /// <summary>
        /// 获取配置单元名称
        /// </summary>
        /// <value>配置单元名称</value>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取配置项集合
        /// </summary>
        /// <value>配置项集合</value>
        IConfigItem[] Items
        {
            get;
        }
        #endregion
    }
}