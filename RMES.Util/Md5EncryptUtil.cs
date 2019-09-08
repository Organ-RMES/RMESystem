using System.Security.Cryptography;
using System.Text;

namespace RMES.Util
{
    /// <summary>
    /// MD5加密帮助类
    /// </summary>
    public static class Md5EncryptUtil
    {
        /// <summary>
        /// 计算指定字符串的MD5哈希值
        /// </summary>
        /// <param name="message">要进行哈希计算的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }
            else
            {
                var md5 = MD5.Create();
                var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(message));
                var sb = new StringBuilder();
                foreach (var b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="input">要加密的内容</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Encrypt(string input, string salt)
        {
            var message = input + salt;
            if (string.IsNullOrEmpty(message))
            {
                return string.Empty;
            }
            else
            {
                var md5 = MD5.Create();
                var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(message));
                var sb = new StringBuilder();
                foreach (var b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
