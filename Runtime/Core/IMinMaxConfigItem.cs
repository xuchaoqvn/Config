namespace SimpleFramework.Config
{
    /// <summary>
    /// 最大最小配置项接口
    /// </summary>
    public interface IMinMaxConfigItem
    {
        #region Property
        /// <summary>
        /// 获取最小值字符串
        /// </summary>
        /// <value>最小值字符串</value>
        string MinValueString
        {
            get;
        }

        /// <summary>
        /// 获取最大值字符串
        /// </summary>
        /// <value>最大值字符串</value>
        string MaxValueString
        {
            get;
        }
        #endregion
    }

    /// <summary>
    /// 最大最小配置项接口(泛型)
    /// </summary>
    public interface IMinMaxConfigItem<T> : IConfigItem<T>, IMinMaxConfigItem
    {
        #region Property
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <value>最小值</value>
        T MinValue
        {
            get;
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <value>最大值</value>
        T MaxValue
        {
            get;
        }
        #endregion
    }
}