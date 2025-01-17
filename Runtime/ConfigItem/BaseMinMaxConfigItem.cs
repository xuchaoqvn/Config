namespace SimpleFramework.Config
{
    /// <summary>
    /// 最大最小值配置项基类
    /// </summary>
    /// <typeparam name="T">配置项数据类型</typeparam>
    public abstract class BaseMinMaxConfigItem<T> : BaseConfigItem<T>, IMinMaxConfigItem<T>
    {
        #region Property
        /// <summary>
        /// 获取配置项最小值的字符串
        /// </summary>
        /// <returns>配置项最小值的字符串</returns>
        public abstract string MinValueString
        {
            get;
        }

        /// <summary>
        /// 获取配置项最大值的字符串
        /// </summary>
        /// <returns>配置项最大值的字符串</returns>
        public abstract string MaxValueString
        {
            get;
        }

        /// <summary>
        /// 获取配置项最小值
        /// </summary>
        /// <returns>配置项最小值</returns>
        public abstract T MinValue
        {
            get;
        }

        /// <summary>
        /// 获取配置项最大值
        /// </summary>
        /// <returns>配置项最大值</returns>
        public abstract T MaxValue
        {
            get;
        }
        #endregion

        protected BaseMinMaxConfigItem(string name, UIType uiType = UIType.Slider, string defaultValue = default, string value = default, string minValue = default, string maxValue = default)
            : base(name, uiType)
        {

        }
    }
}