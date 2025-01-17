using System;
using System.Reflection;

namespace SimpleFramework.Config
{
    /// <summary>
    /// 配置项基类
    /// </summary>
    /// <typeparam name="T">配置项数据类型</typeparam>
    public abstract class BaseConfigItem<T> : IConfigItem<T>
    {
        #region Field
        /// <summary>
        /// 配置项名称
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// 泛型类型
        /// </summary>
        protected Type m_Type;

        /// <summary>
        /// 配置项UI类型
        /// </summary>
        protected UIType m_UIType;
        #endregion

        #region Peoperty
        /// <summary>
        /// 获取配置项名称
        /// </summary>
        /// <returns>配置项名称</returns>
        public string Name => this.m_Name;

        /// <summary>
        /// 获取配置项数据类型
        /// </summary>
        /// <returns>配置项数据类型</returns> 
        public Type Type => this.m_Type;

        /// <summary>
        /// 获取配置项UI类型
        /// </summary>
        /// <returns>配置项UI类型</returns>
        public UIType UIType => this.m_UIType;

        /// <summary>
        /// 获取配置项默认值
        /// </summary>
        /// <returns>配置项默认值</returns>
        public virtual T DefaultValue
        {
            get;
        }

        /// <summary>
        /// 获取配置项默认值字符串
        /// </summary>
        /// <returns>配置项默认值字符串</returns>
        public abstract string DefaultValueString
        {
            get;
        }

        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <returns>配置项的值</returns>
        public abstract T Value
        {
            get;
            set;
        }

        /// <summary>
        /// 获取配置项值的字符串
        /// </summary>
        /// <returns>配置项值的字符串</returns>
        public abstract string ValueString
        {
            get;
            set;
        }
        #endregion

        public BaseConfigItem(string name, UIType uiType)
        {
            this.m_Name = name;
            this.m_Type = typeof(T);
            this.m_UIType = uiType;
        }

        #region Function
        /// <summary>
        /// 是否将文本解析至指定的类型
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>是否解析成功</returns>
        public bool TryParse(string text) => this.TryParse(text, out _);

        /// <summary>
        /// 尝试将文本解析至指定的类型
        /// </summary>
        /// <param name="text">文本</param>
        /// <typeparam name="T">指定的类型</typeparam>
        /// <returns>是否解析成功</returns>
        protected virtual bool TryParse(string text, out T t)
        {
            t = default;
            Type type = typeof(T);
            string methodName = "TryParse";
            Type[] parameterTypes = new Type[] { typeof(string), type.MakeByRefType() };
            MethodInfo methodInfo = type.GetMethod(methodName, parameterTypes);
            if (methodInfo == null)
            {
                type = typeof(ConfigExtension);
                methodInfo = type.GetMethod(methodName, parameterTypes);
            }

            object[] objects = new object[] { text, t };
            bool successful = (bool)methodInfo.Invoke(t, objects);
            t = (T)objects[1];
            return successful;
        }
        #endregion
    }
}