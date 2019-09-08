using System;

namespace RMES.Util
{
    public class RandomUtil
    {
        private static readonly string[] Chars = new string[] {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
            "1","2","3","4","5","6","7","8","9","0","~","!","@","#","$","%","^","&","*","(",")","-","+","*","?","|"
        };

        private static readonly string[] Numbers = new string[] { "1","2","3","4","5","6","7","8","9","0" };

        /// <summary>
        /// 获取指定长度的随机字符串。包含大小写字母、数字及部分特殊字符
        /// </summary>
        /// <param name="length"></param>
        /// <param name="containSpecialChars">是否包含特殊字符</param>
        /// <returns></returns>
        public static string GetStringFromFullChars(int length, bool containSpecialChars = true)
        {
            var result = "";
            int charSize = containSpecialChars ? 78 : 62;

            for(var i = 0; i < length; i++)
            {
                var ran = new Random().Next(0, Chars.Length);
                result += Chars[ran];
            }

            return result;
        }

        /// <summary>
        /// 获取由数字组成的指定长度的随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetStringFromNumbers(int length)
        {
            var result = "";

            for (var i = 0; i < length; i++)
            {
                var ran = new Random().Next(0, Numbers.Length);
                result += Numbers[ran];
            }

            return result;
        }
    }
}
