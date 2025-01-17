namespace SimpleFramework.Config
{
    /// <summary>
    /// 选项配置项基类
    /// </summary>
    /// <typeparam name="T">配置项数据类型</typeparam>
    public abstract class BaseOptionsConfigItem<T> : BaseConfigItem<T>, IOptionsConfigItem
    {
        #region Property
        /// <summary>
        /// 获取配置项默认选项索引
        /// </summary>
        /// <value>配置项默认选项索引</value>
        public abstract int DefaultIndex
        {
            get;
        }

        /// <summary>
        /// 获取配置项选项索引
        /// </summary>
        /// <value>配置项选项索引</value>
        public abstract int Index
        {
            get;
            set;
        }

        // <summary>
        /// 获取配置项选项的字符串集
        /// </summary>
        /// <returns>配置项选项的字符串集</returns>
        public abstract string[] Options
        {
            get;
        }
        #endregion

        protected BaseOptionsConfigItem(string name, UIType uiType = UIType.Dropdown, string defaultValue = default, string value = default)
            : base(name, uiType)
        {

        }
    }
}