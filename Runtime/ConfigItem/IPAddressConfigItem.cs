using System.Linq;
using System.Net;

namespace SimpleFramework.Config
{
    /// <summary>
    /// IP地址配置项
    /// </summary>
    public class IPAddressConfigItem : OptionsConfigItem<string>
    {
        #region Property
        /// <summary>
        /// 获取配置项选项索引
        /// </summary>
        /// <value>配置项选项索引</value>
        public override int Index
        {
            get => base.Index;
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
        #endregion

        public IPAddressConfigItem(string name, UIType uiType = UIType.Dropdown, int defaultOptionIndex = 0, int optionIndex = 0, string defaultValue = "127.0.0.1", string value = "127.0.0.1", string[] options = default)
            : base(name, uiType, defaultOptionIndex, optionIndex, defaultValue, value, options)
        {
            this.m_DefaultOptionIndex = defaultOptionIndex;
            this.m_OptionIndex = optionIndex;
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] iPAddresses = ipHostEntry.AddressList.Where(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray();
            this.m_Options = new string[iPAddresses.Length];
            for (int i = 0; i < iPAddresses.Length; i++)
                this.m_Options[i] = iPAddresses[i].ToString();
        }
    }
}