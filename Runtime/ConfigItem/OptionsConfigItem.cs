using System;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 选项配置项
    /// </summary>
    /// <typeparam name="T">配置项数据类型</typeparam>
    public class OptionsConfigItem<T> : BaseOptionsConfigItem<T>
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

        /// <summary>
        /// 配置项默认选项索引
        /// </summary>
        protected int m_DefaultOptionIndex;

        /// <summary>
        /// 配置项选项索引
        /// </summary>
        protected int m_OptionIndex;

        /// <summary>
        /// 配置项选项的字符串集
        /// </summary>
        protected string[] m_Options;
        #endregion

        #region Property
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

        /// <summary>
        /// 获取配置项默认选项索引
        /// </summary>
        /// <value>配置项默认选项索引</value>
        public override int DefaultIndex => this.m_DefaultOptionIndex;

        /// <summary>
        /// 获取配置项选项索引
        /// </summary>
        /// <value>配置项选项索引</value>
        public override int Index
        {
            get => this.m_OptionIndex;
            set
            {
                if (this.m_Options == null || this.m_Options.Length <= 0)
                    return;
                if (value < 0 || value >= this.m_Options.Length)
                    return;
                if (!this.TryParse(this.m_Options[value], out this.m_Value))
                    return;

                this.m_OptionIndex = value;
            }
        }

        // <summary>
        /// 获取配置项选项的字符串集
        /// </summary>
        /// <returns>配置项选项的字符串集</returns>
        public override string[] Options => this.m_Options;
        #endregion

        public OptionsConfigItem(string name, UIType uiType = UIType.Dropdown, int defaultOptionIndex = 0, int optionIndex = 0, string defaultValue = default, string value = default, string[] options = default)
            : base(name, uiType, defaultValue, value)
        {
            if (!this.TryParse(defaultValue, out this.m_DefaultValue))
                throw new FormatException("Input string was not in a correct format.");
            if (!this.TryParse(value, out this.m_Value))
                throw new FormatException("Input string was not in a correct format.");
            this.m_DefaultValueString = defaultValue;

            this.m_DefaultOptionIndex = defaultOptionIndex;
            this.m_OptionIndex = optionIndex;
            this.m_Options = options;
        }
    }
}