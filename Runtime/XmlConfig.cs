using System;
using System.IO;
using System.Net;
using System.Xml;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SimpleFramework.Config
{
    /// <summary>
    /// xml配置
    /// </summary>
    public class XmlConfig : BaseConfig
    {
        #region Function
        /// <summary>
        /// 读取配置
        /// </summary>
        public override void Read()
        {
            if (!File.Exists(this.m_ConfigPath))
                return;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(this.m_ConfigPath);
            XmlElement root = xmlDocument.DocumentElement;

            XmlNodeList units = root.ChildNodes;
            List<IConfigUnit> configUnits = new List<IConfigUnit>(units.Count);
            for (int i = 0; i < units.Count; i++)
            {
                XmlNode unit = units[i];
                if (unit.NodeType == XmlNodeType.Comment)
                    continue;

                XmlNodeList items = unit.ChildNodes;
                List<IConfigItem> configItems = new List<IConfigItem>();
                for (int j = 0; j < items.Count; j++)
                {
                    XmlNode item = items[j];
                    if (item.NodeType == XmlNodeType.Comment)
                        continue;

                    string name = item.Attributes["Name"].Value;
                    //处理方式
                    XmlAttribute processModeAttribute = item.Attributes["ProcessMode"];
                    bool hasProcessMode = processModeAttribute != null;
                    int processMode = 0;
                    if (hasProcessMode)
                    {
                        if (processModeAttribute.Value == "Default")
                            processMode = 0;
                        else if (processModeAttribute.Value == "Auto")
                            processMode = 1;
                    }
                    else
                        processMode = 0;
                    //数据类型（处理方式为默认时）
                    string typeName = item.Attributes["Type"].Value;
                    ConfigDataType configDataType = default;
                    if (processMode == 0)
                    {
                        if (!Enum.TryParse<ConfigDataType>(typeName, out configDataType))
                            continue;
                    }
                    //UI显示类型
                    if (!Enum.TryParse<UIType>(item.Attributes["UIType"].Value, out UIType uiType))
                        continue;
                    string defaultValue = item.Attributes["DefaultValue"].Value;
                    string value = item.Attributes["Value"].Value;
                    IConfigItem configItem = default;
                    if (uiType == UIType.Slider)
                    {
                        //默认处理方式
                        if (processMode == 0)
                        {
                            string minValue = item.Attributes["MinValue"].Value;
                            string maxValue = item.Attributes["MaxValue"].Value;
                            configItem = this.MinMaxConfigItem(name, configDataType, uiType, defaultValue, value, minValue, maxValue);
                        }
                        //自动查找类，逻辑由类自身处理
                        else if (processMode == 1)
                        {
                            Type type = this.GetType(typeName);
                            configItem = (IConfigItem)Activator.CreateInstance(type, new object[] { name, UIType.Slider, defaultValue, value });
                        }
                    }
                    else if (uiType == UIType.Dropdown)
                    {
                        //默认处理方式
                        if (processMode == 0)
                        {
                            int defaultOptionIndex = Convert.ToInt32(item.Attributes["DefaultOptionIndex"].Value);
                            int optionIndex = Convert.ToInt32(item.Attributes["OptionIndex"].Value);

                            XmlNodeList optionList = item.ChildNodes;
                            List<string> options = new List<string>();
                            for (int k = 0; k < optionList.Count; k++)
                            {
                                XmlNode option = optionList[k];
                                if (option.NodeType == XmlNodeType.Comment)
                                    continue;
                                options.Add(option.InnerText);
                            }
                            configItem = this.OptionsConfigItem(name, configDataType, uiType, defaultValue, value, defaultOptionIndex, optionIndex, options.ToArray());
                        }
                        //自动查找类，逻辑由类自身处理
                        else if (processMode == 1)
                        {
                            Type type = this.GetType(typeName);
                            //枚举
                            if (type.IsEnum)
                            {
                                string enumTypeName = $"SimpleFramework.Config.EnumConfigItem`1";
                                Type enumType = this.GetType(enumTypeName).MakeGenericType(type);
                                configItem = (IConfigItem)Activator.CreateInstance(enumType, new object[] { name, UIType.Dropdown, 0, 0, defaultValue, value, null });
                            }
                            //自定义类
                            else
                                //构造函数的参数需固定,type写枚举类型，判断枚举类型强制EnumConfigItem
                                configItem = (IConfigItem)Activator.CreateInstance(type, new object[] { name, UIType.Dropdown, 0, 0, defaultValue, value, null });
                        }
                    }
                    else
                    {
                        if (processMode == 0)
                            configItem = this.ConfigItem(name, configDataType, uiType, defaultValue, value);
                        //自动查找类，逻辑由类自身处理
                        else if (processMode == 1)
                        {
                            Type type = this.GetType(typeName);
                            configItem = (IConfigItem)Activator.CreateInstance(type, new object[] { name, uiType, defaultValue, value });
                        }
                    }
                    configItems.Add(configItem);
                }
                configUnits.Add(new ConfigUnit(unit.LocalName, configItems.ToArray()));
            }
            this.m_Configunits = configUnits.ToArray();
        }

        /// <summary>
        /// 写入配置
        /// </summary>
        public override void Save()
        {
            if (File.Exists(this.m_ConfigPath))
                File.Delete(this.m_ConfigPath);
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode header = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(header);
            XmlElement config = xmlDoc.CreateElement("Config");

            for (int i = 0; i < this.m_Configunits.Length; i++)
            {
                int unitIndex = i;
                IConfigUnit unit = this.m_Configunits[unitIndex];
                IConfigItem[] items = unit.Items;
                XmlElement unitXmlElement = xmlDoc.CreateElement(unit.Name);
                for (int j = 0; j < items.Length; j++)
                {
                    int itemIndex = j;
                    IConfigItem item = items[itemIndex];
                    string name = item.Name;
                    string type = item.Type.Name;
                    string uiType = item.UIType.ToString();
                    string defaultValue = item.DefaultValueString;
                    string value = item.ValueString;

                    Type itemType = item.GetType();
                    XmlElement itemXmlElement = xmlDoc.CreateElement("ConfigItem");
                    itemXmlElement.SetAttribute("Name", name);
                    itemXmlElement.SetAttribute("Type", type);
                    itemXmlElement.SetAttribute("UIType", uiType);
                    itemXmlElement.SetAttribute("DefaultValue", defaultValue);
                    itemXmlElement.SetAttribute("Value", value);
                    if (item.UIType == UIType.Slider)
                    {
                        if (itemType.IsGenericType)
                        {
                            IMinMaxConfigItem minMaxConfigItem = item as IMinMaxConfigItem;
                            itemXmlElement.SetAttribute("MinValue", minMaxConfigItem.MinValueString);
                            itemXmlElement.SetAttribute("MaxValue", minMaxConfigItem.MaxValueString);
                        }
                        else
                        {
                            itemXmlElement.SetAttribute("Type", itemType.ToString());
                            itemXmlElement.SetAttribute("ProcessMode", "Auto");
                        }
                    }
                    else if (item.UIType == UIType.Dropdown)
                    {
                        if (itemType.IsGenericType)
                        {
                            IOptionsConfigItem optionsConfigItem = item as IOptionsConfigItem;
                            itemXmlElement.SetAttribute("DefaultOptionIndex", optionsConfigItem.DefaultIndex.ToString());
                            itemXmlElement.SetAttribute("OptionIndex", optionsConfigItem.Index.ToString());

                            //枚举(特殊类型)
                            if (itemType.Name == "EnumConfigItem`1" && item.Type.IsEnum)
                            {
                                itemXmlElement.SetAttribute("Type", item.Type.ToString());
                                itemXmlElement.SetAttribute("ProcessMode", "Auto");
                            }
                            //普通类型
                            else
                            {
                                string[] options = optionsConfigItem.Options;
                                for (int k = 0; k < options.Length; k++)
                                {
                                    XmlElement optionXmlElement = xmlDoc.CreateElement("Option");
                                    optionXmlElement.InnerText = options[k];
                                    itemXmlElement.AppendChild(optionXmlElement);
                                }
                            }
                        }
                        else
                        {
                            itemXmlElement.SetAttribute("Type", itemType.ToString());
                            itemXmlElement.SetAttribute("ProcessMode", "Auto");
                        }
                    }
                    else
                    {
                        if (!itemType.IsGenericType)
                        {
                            itemXmlElement.SetAttribute("Type", itemType.ToString());
                            itemXmlElement.SetAttribute("ProcessMode", "Auto");
                        }
                    }
                    unitXmlElement.AppendChild(itemXmlElement);
                }
                config.AppendChild(unitXmlElement);
            }

            xmlDoc.AppendChild(config);

            xmlDoc.Save(this.m_ConfigPath);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="unitName">配置单元名称</param>
        /// <param name="itemName">配置项名称</param>
        /// <param name="t">配置</param>
        /// <typeparam name="T">配置数据类型</typeparam>
        /// <returns>是否获取成功</returns>
        public override bool GetConfig<T>(string unitName, string itemName, out T t)
        {
            t = default;
            if (!Array.Exists(this.m_Configunits, unit => unit.Name == unitName))
                return false;
            IConfigUnit configUnit = this.m_Configunits.First(unit => unit.Name == unitName);
            if (!Array.Exists(configUnit.Items, item => item.Name == itemName))
                return false;
            IConfigItem configItem = configUnit.Items.First(item => item.Name == itemName);
            if (typeof(T) != configItem.Type)
                return false;

            t = (configItem as IConfigItem<T>).Value;
            return true;
        }

        private IConfigItem ConfigItem(string name, ConfigDataType dataType, UIType uiType, string defaultValue, string value)
        {
            IConfigItem configItem = default;
            switch (dataType)
            {
                case ConfigDataType.Byte:
                    configItem = new ConfigItem<byte>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.SByte:
                    configItem = new ConfigItem<sbyte>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Int16:
                    configItem = new ConfigItem<short>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.UInt16:
                    configItem = new ConfigItem<ushort>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Int32:
                    configItem = new ConfigItem<int>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.UInt32:
                    configItem = new ConfigItem<uint>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Int64:
                    configItem = new ConfigItem<long>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.UInt64:
                    configItem = new ConfigItem<ulong>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Single:
                    configItem = new ConfigItem<float>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Double:
                    configItem = new ConfigItem<double>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Decimal:
                    configItem = new ConfigItem<decimal>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Boolean:
                    configItem = new ConfigItem<bool>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Char:
                    configItem = new ConfigItem<char>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.String:
                    configItem = new ConfigItem<string>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.DateTime:
                    configItem = new ConfigItem<DateTime>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.TimeSpan:
                    configItem = new ConfigItem<TimeSpan>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.IPAddress:
                    configItem = new ConfigItem<IPAddress>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.IPEndPoint:
                    configItem = new ConfigItem<IPEndPoint>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Vector2:
                    configItem = new ConfigItem<Vector2>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Vector3:
                    configItem = new ConfigItem<Vector3>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Color:
                    configItem = new ConfigItem<Color>(name, uiType, defaultValue, value);
                    break;
                case ConfigDataType.Color32:
                    configItem = new ConfigItem<Color32>(name, uiType, defaultValue, value);
                    break;
                default:
                    break;
            }
            return configItem;
        }

        private IConfigItem MinMaxConfigItem(string name, ConfigDataType dataType, UIType uiType, string defaultValue, string value, string minValue, string maxValue)
        {
            IConfigItem configItem = default;
            switch (dataType)
            {
                case ConfigDataType.Byte:
                    configItem = new MinMaxConfigItem<byte>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.SByte:
                    configItem = new MinMaxConfigItem<sbyte>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Int16:
                    configItem = new MinMaxConfigItem<short>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.UInt16:
                    configItem = new MinMaxConfigItem<ushort>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Int32:
                    configItem = new MinMaxConfigItem<int>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.UInt32:
                    configItem = new MinMaxConfigItem<uint>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Int64:
                    configItem = new MinMaxConfigItem<long>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.UInt64:
                    configItem = new MinMaxConfigItem<ulong>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Single:
                    configItem = new MinMaxConfigItem<float>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Double:
                    configItem = new MinMaxConfigItem<double>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Decimal:
                    configItem = new MinMaxConfigItem<decimal>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Boolean:
                    configItem = new MinMaxConfigItem<bool>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Char:
                    configItem = new MinMaxConfigItem<char>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.String:
                    configItem = new MinMaxConfigItem<string>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.DateTime:
                    configItem = new MinMaxConfigItem<DateTime>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.TimeSpan:
                    configItem = new MinMaxConfigItem<TimeSpan>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.IPAddress:
                    configItem = new MinMaxConfigItem<IPAddress>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.IPEndPoint:
                    configItem = new MinMaxConfigItem<IPEndPoint>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Vector2:
                    configItem = new MinMaxConfigItem<Vector2>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Vector3:
                    configItem = new MinMaxConfigItem<Vector3>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Color:
                    configItem = new MinMaxConfigItem<Color>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                case ConfigDataType.Color32:
                    configItem = new MinMaxConfigItem<Color32>(name, uiType, defaultValue, value, minValue, maxValue);
                    break;
                default:
                    break;
            }
            return configItem;
        }

        private IConfigItem OptionsConfigItem(string name, ConfigDataType dataType, UIType uiType, string defaultValue, string value, int defaultOptionIndex, int optionIndex, string[] Options)
        {
            IConfigItem configItem = default;
            switch (dataType)
            {
                case ConfigDataType.Byte:
                    configItem = new OptionsConfigItem<byte>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.SByte:
                    configItem = new OptionsConfigItem<sbyte>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Int16:
                    configItem = new OptionsConfigItem<short>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.UInt16:
                    configItem = new OptionsConfigItem<ushort>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Int32:
                    configItem = new OptionsConfigItem<int>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.UInt32:
                    configItem = new OptionsConfigItem<uint>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Int64:
                    configItem = new OptionsConfigItem<long>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.UInt64:
                    configItem = new OptionsConfigItem<ulong>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Single:
                    configItem = new OptionsConfigItem<float>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Double:
                    configItem = new OptionsConfigItem<double>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Decimal:
                    configItem = new OptionsConfigItem<decimal>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Boolean:
                    configItem = new OptionsConfigItem<bool>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Char:
                    configItem = new OptionsConfigItem<char>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.String:
                    configItem = new OptionsConfigItem<string>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.DateTime:
                    configItem = new OptionsConfigItem<DateTime>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.TimeSpan:
                    configItem = new OptionsConfigItem<TimeSpan>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.IPAddress:
                    configItem = new OptionsConfigItem<IPAddress>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.IPEndPoint:
                    configItem = new OptionsConfigItem<IPEndPoint>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Vector2:
                    configItem = new OptionsConfigItem<Vector2>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Vector3:
                    configItem = new OptionsConfigItem<Vector3>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Color:
                    configItem = new OptionsConfigItem<Color>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                case ConfigDataType.Color32:
                    configItem = new OptionsConfigItem<Color32>(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, Options);
                    break;
                default:
                    break;
            }
            return configItem;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>类型</returns>
        private Type GetType(string name)
        {
            Type type = Type.GetType(name);
            if (type != null)
                return type;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                type = assembly.GetType(name);
                if (type != null)
                    break;
            }

            if (type == null)
                throw new TypeLoadException($"Not Found Type: {name}");

            return type;
        }
        #endregion
    }
}