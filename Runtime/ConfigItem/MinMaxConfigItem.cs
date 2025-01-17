
using System;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 最大最小值配置项
    /// </summary>
    /// <typeparam name="T">配置项数据类型</typeparam>
    public class MinMaxConfigItem<T> : BaseMinMaxConfigItem<T>
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
        /// 配置项最小值
        /// </summary>
        protected T m_MinValue;

        // <summary>
        /// 配置项最小值字符串
        /// </summary>
        protected string m_MinValueString;

        /// <summary>
        /// 配置项最大值
        /// </summary>
        protected T m_MaxValue;

        /// <summary>
        /// 配置项最大值字符串
        /// </summary>
        protected string m_MaxValueString;
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
        /// 获取配置项最小值
        /// </summary>
        /// <returns>配置项最小值</returns>
        public override T MinValue => this.m_MinValue;

        /// <summary>
        /// 获取配置项最小值的字符串
        /// </summary>
        /// <returns>配置项最小值的字符串</returns>
        public override string MinValueString => this.m_MinValueString;

        /// <summary>
        /// 获取配置项最大值
        /// </summary>
        /// <returns>配置项最大值</returns>
        public override T MaxValue => this.m_MaxValue;

        /// <summary>
        /// 获取配置项最大值的字符串
        /// </summary>
        /// <returns>配置项最大值的字符串</returns>
        public override string MaxValueString => this.m_MaxValueString;
        #endregion

        public MinMaxConfigItem(string name, UIType uiType = UIType.Slider, string defaultValue = default, string value = default, string minValue = default, string maxValue = default)
            : base(name, uiType, defaultValue, value)
        {
            if (!this.TryParse(defaultValue, out this.m_DefaultValue))
                throw new FormatException("Input string was not in a correct format.");
            if (!this.TryParse(value, out this.m_Value))
                throw new FormatException("Input string was not in a correct format.");
            this.m_DefaultValueString = defaultValue;

            if (!this.TryParse(minValue, out this.m_MinValue))
                throw new FormatException("Input string was not in a correct format.");
            this.m_MinValueString = minValue;
            if (!this.TryParse(maxValue, out this.m_MaxValue))
                throw new FormatException("Input string was not in a correct format.");
            this.m_MaxValueString = maxValue;
        }
    }
}