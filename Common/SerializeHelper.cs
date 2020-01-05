using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SerializeHelper
    {
        /// <summary>
        /// 将一个object转换为一个json格式字符串（序列化）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SerializeToString(object value) {
            return JsonConvert.SerializeObject(value);
        }
        /// <summary>
        /// 将一个json格式字符串转换为一个自定义的格式（反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(string str) {
            return JsonConvert.DeserializeObject<T>(str);
        }

    }
}
