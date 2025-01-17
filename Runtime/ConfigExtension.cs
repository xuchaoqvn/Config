using System.Net;
using UnityEngine;

namespace SimpleFramework.Config
{
    public static class ConfigExtension
    {
        /// <summary>
        /// 尝试将文本解析成IPEndPoint
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="color">IPEndPoint</param>
        /// <returns>是否解析成功</returns>
        public static bool TryParse(this string text, out string @string)
        {
            @string = text;
            return true;
        }

        /// <summary>
        /// 尝试将文本解析成IPEndPoint
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="color">IPEndPoint</param>
        /// <returns>是否解析成功</returns>
        public static bool TryParse(this string text, out IPEndPoint iPEndPoint)
        {
            iPEndPoint = default;
            string[] values = text.Split(':');
            if (values.Length < 2)
                return false;
            if (!IPAddress.TryParse(values[0], out IPAddress iPAddress) || !int.TryParse(values[1], out int port))
                return false;
            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                return false;

            if (iPEndPoint == null)
                iPEndPoint = new IPEndPoint(IPAddress.Any, 8306);

            iPEndPoint.Address = iPAddress;
            iPEndPoint.Port = port;

            return true;
        }

        /// <summary>
        /// 尝试将文本解析成Vector2
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="color">Vector2</param>
        /// <returns>是否解析成功</returns>
        public static bool TryParse(this string text, out Vector2 vector2)
        {
            vector2 = Vector2.zero;
            if (text[0] != '(' || text[text.Length - 1] != ')')
                return false;
            text = text.Remove(text.Length - 1, 1).Remove(0, 1);
            string[] values = text.Split(',');
            if (values.Length < 2)
                return false;
            if (!float.TryParse(values[0], out float x) || !float.TryParse(values[1], out float y))
                return false;

            vector2 = new Vector2(x, y);
            return true;
        }

        /// <summary>
        /// 尝试将文本解析成Vector3
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="color">Vector3</param>
        /// <returns>是否解析成功</returns>
        public static bool TryParse(this string text, out Vector3 vector3)
        {
            vector3 = Vector3.zero;
            if (text[0] != '(' || text[text.Length - 1] != ')')
                return false;
            text = text.Remove(text.Length - 1, 1).Remove(0, 1);
            string[] values = text.Split(',');
            if (values.Length < 3)
                return false;
            if (!float.TryParse(values[0], out float x) || !float.TryParse(values[1], out float y) || !float.TryParse(values[2], out float z))
                return false;

            vector3 = new Vector3(x, y, z);
            return true;
        }

        /// <summary>
        /// 尝试将文本解析成Color
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="color">Color</param>
        /// <returns>是否解析成功</returns>
        public static bool TryParse(this string text, out Color color)
        {
            color = Color.white;
            if (!text.StartsWith("RGBA(") || text[text.Length - 1] != ')')
                return false;
            text = text.Remove(text.Length - 1, 1).Remove(0, 5);
            string[] values = text.Split(',');
            if (values.Length < 4)
                return false;
            if (!float.TryParse(values[0], out float r) || !float.TryParse(values[1], out float g) || !float.TryParse(values[2], out float b) | !float.TryParse(values[2], out float a))
                return false;

            color = new Color(r, g, b, a);
            return true;
        }

        /// <summary>
        /// 尝试将文本解析成Color32
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="color">Color32</param>
        /// <returns>是否解析成功</returns>
        public static bool TryParse(this string text, out Color32 color)
        {
            color = Color.white;
            if (!text.StartsWith("RGBA(") || text[text.Length - 1] != ')')
                return false;
            text = text.Remove(text.Length - 1, 1).Remove(0, 5);
            string[] values = text.Split(',');
            if (values.Length < 4)
                return false;
            if (!byte.TryParse(values[0], out byte r) || !byte.TryParse(values[1], out byte g) || !byte.TryParse(values[2], out byte b) | !byte.TryParse(values[2], out byte a))
                return false;

            color = new Color32(r, g, b, a);
            return true;
        }
    }
}