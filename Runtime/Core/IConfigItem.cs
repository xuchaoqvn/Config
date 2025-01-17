
using System;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置项接口
    /// </summary>
    public interface IConfigItem
    {
        #region Property
        /// <summary>
        /// 获取配置项名称
        /// </summary>
        /// <returns>配置项名称</returns>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取配置项数据类型
        /// </summary>
        /// <returns>配置项数据类型</returns> 
        Type Type
        {
            get;
        }

        /// <summary>
        /// 获取配置项UI类型
        /// </summary>
        /// <returns>配置项UI类型</returns>
        UIType UIType
        {
            get;
        }

        /// <summary>
        /// 获取配置项默认值字符串
        /// </summary>
        /// <returns>配置项默认值字符串</returns>
        string DefaultValueString
        {
            get;
        }

        /// <summary>
        /// 获取配置项值的字符串
        /// </summary>
        /// <returns>配置项值的字符串</returns>
        string ValueString
        {
            get;
            set;
        }
        #endregion

        #region Function
        /// <summary>
        /// 是否将文本解析至指定的类型
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>是否解析成功</returns>
        bool TryParse(string text);
        #endregion
    }

    /// <summary>
    /// 配置项接口（泛型）
    /// </summary>
    public interface IConfigItem<T> : IConfigItem
    {
        #region Property
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <value>默认值</value>
        T DefaultValue
        {
            get;
        }

        /// <summary>
        /// 获取当前值
        /// </summary>
        /// <value>当前值</value>
        T Value
        {
            get;
        }
        #endregion
    }
}