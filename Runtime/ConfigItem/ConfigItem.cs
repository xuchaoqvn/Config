using System;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 默认配置项
    /// 常见的数据类型
    /// T即数据本身的类型，不需要通过其他方式获取最大最小和选项之类的属性值
    /// .net数据类型、字符串、decimal、DateTime、TimeSpan、IPAddress、IPEndPoint等具有TryParse函数的类型；
    /// Unity的Vector2、Vector3、Color、Color32
    /// 若此类不满足，则新建一个类继承BaseConfigItem<T>,重写函数即可，例如获取本机已链接的相机名称，T可以是string，函数的内部可由WebCameraTexture类实现
    /// </summary>
    /// <typeparam name="T">配置项数据类型</typeparam>
    public class ConfigItem<T> : BaseConfigItem<T>
    {
        #region Field
        /// <summary>
        /// 配置项默认值
        /// </summary>
        protected T m_DefaultValue;

        /// <summary>
        /// 配置项默认值字符串
        /// </summary>
        protected string m_DefaultValueString;

        /// <summary>
        /// 配置项的值
        /// </summary>
        protected T m_Value;
        #endregion

        #region Proterty
        /// <summary>
        /// 获取配置项默认值
        /// </summary>
        /// <returns>配置项默认值</returns>
        public override T DefaultValue => this.m_DefaultValue;

        /// <summary>
        /// 获取配置项默认值字符串
        /// </summary>
        /// <returns>配置项默认值字符串</returns>
        public override string DefaultValueString => this.m_DefaultValueString;

        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <returns>配置项的值</returns>
        public override T Value
        {
            get => this.m_Value;
            set => this.m_Value = value;
        }

        /// <summary>
        /// 获取配置项值的字符串
        /// </summary>
        /// <returns>配置项值的字符串</returns>
        public override string ValueString
        {
            get => this.m_Value.ToString();
            set
            {
                if (!this.TryParse(value, out this.m_Value))
                    throw new FormatException("Input string was not in a correct format.");
            }
        }
        #endregion

        public ConfigItem(string name, UIType uiType, string defaultValue = default, string value = default)
            : base(name, uiType)
        {
            if (!this.TryParse(defaultValue, out this.m_DefaultValue))
                throw new FormatException("Input string was not in a correct format.");
            if (!this.TryParse(value, out this.m_Value))
                throw new FormatException("Input string was not in a correct format.");

            this.m_DefaultValueString = defaultValue;
        }
    }
}