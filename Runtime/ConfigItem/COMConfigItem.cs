using System;
using System.IO.Ports;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 串口COM口配置项
    /// </summary>
    public class COMConfigItem : BaseOptionsConfigItem<string>
    {
        #region Field
        /// <summary>
        /// 配置项默认值
        /// </summary>
        protected string m_DefaultValue;

        /// <summary>
        /// 配置项默认值字符串
        /// </summary>
        protected string m_DefaultValueString;

        /// <summary>
        /// 配置项的值
        /// </summary>
        protected string m_Value;

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
        public override string DefaultValue => this.m_DefaultValue;

        /// <summary>
        /// 获取配置项默认值字符串
        /// </summary>
        /// <returns>配置项默认值字符串</returns>
        public override string DefaultValueString => this.m_DefaultValueString;

        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <returns>配置项的值</returns>
        public override string Value
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
                this.m_Value = this.m_Options[value];
                this.m_OptionIndex = value;
            }
        }

        // <summary>
        /// 获取配置项选项的字符串集
        /// </summary>
        /// <returns>配置项选项的字符串集</returns>
        public override string[] Options => this.m_Options;
        #endregion

        public COMConfigItem(string name, UIType uiType = UIType.Dropdown, int defaultOptionIndex = 0, int optionIndex = 0, string defaultValue = "COM1", string value = "COM1", string[] options = default)
            : base(name, uiType, defaultValue, value)
        {
            this.m_DefaultOptionIndex = 0;
            this.m_OptionIndex = 0;
            this.m_Options = SerialPort.GetPortNames();

            this.m_DefaultValue = this.m_Options[0];
            this.m_DefaultValueString = defaultValue;
            this.m_Value = this.m_Options[0];
        }

        #region Function
        /// <summary>
        /// 尝试将文本解析至指定的类型
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="t">字符串</param>
        /// <returns>是否解析成功</returns>
        protected override bool TryParse(string text, out string t)
        {
            t = text;
            if (!text.StartsWith("COM"))
                return false;
            string numText = text.Substring(3, text.Length - 3);
            return byte.TryParse(numText, out _);
        }
        #endregion
    }
}