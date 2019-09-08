using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using  System.Linq;
using System.Text;

namespace RMES.Util
{
    public class HtmlUtil
    {
        #region 获取图片
        /// <summary>
        /// 获取HTML文本的图片地址
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>/
        /// 
        public static List<string> GetImageUrl(string html)
        {
            var resultStr = new List<string>();
            var r = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);//忽视大小写
            var mc = r.Matches(html);

            foreach (Match m in mc)
            {
                resultStr.Add(m.Groups["imgUrl"].Value.ToLower());
            }

            if (resultStr.Count > 0)
            {
                return resultStr;
            }

            resultStr.Clear();
            return resultStr;
        }
        #endregion

        /// <summary>
        /// 获取内容摘要
        /// </summary>
        /// <returns></returns>
        public static string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return "";
            }

            html = html.Replace("\n", "").Replace(" ", "").Replace("&nbsp;", " ").Replace("<br>", "\n");
            var regex = new Regex(@"<[^>]+>|</[^>]+>");
            var strOutput = regex.Replace(html, "");
            return strOutput;
        }

        /// <summary>
        /// 提取摘要，是否清除HTML代码 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="StripHTML"></param>
        /// <returns></returns>
        public static string GetContentSummary(string content, int length, bool StripHTML)
        {
            if (string.IsNullOrEmpty(content) || length == 0)
                return ""; if (StripHTML)
            {
                Regex re = new Regex("<[^>]*>");
                content = re.Replace(content, "");
                content = content.Replace("　", "").Replace(" ", "");
                if (content.Length <= length)
                    return content;
                else
                    return content.Substring(0, length) + "……";
            }
            else
            {
                if (content.Length <= length)
                    return content;
                int pos = 0, npos = 0, size = 0;
                bool firststop = false, notr = false, noli = false;
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    if (pos >= content.Length)
                        break;
                    string cur = content.Substring(pos, 1);
                    if (cur == "<")
                    {
                        string next = content.Substring(pos + 1, 3).ToLower();
                        if (next.IndexOf("p") == 0 && next.IndexOf("pre") != 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                        }
                        else if
                            (next.IndexOf("/p") == 0 && next.IndexOf("/pr") != 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length) sb.Append("<br/>");
                        }
                        else if (next.IndexOf("br") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                                sb.Append("<br/>");
                        }
                        else if
                            (next.IndexOf("img") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                                size += npos - pos + 1;
                            }
                        }
                        else if
                            (next.IndexOf("li") == 0 || next.IndexOf("/li") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!noli && next.IndexOf("/li") == 0)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                    noli = true;
                                }
                            }
                        }
                        else if (next.IndexOf("tr") == 0 || next.IndexOf("/tr") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!notr && next.IndexOf("/tr") == 0)
                                {
                                    sb.Append(content.Substring(pos, npos - pos)); notr = true;
                                }
                            }
                        }
                        else if (next.IndexOf("td") == 0 || next.IndexOf("/td") == 0)
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            if (size < length)
                            {
                                sb.Append(content.Substring(pos, npos - pos));
                            }
                            else
                            {
                                if (!notr)
                                {
                                    sb.Append(content.Substring(pos, npos - pos));
                                }
                            }
                        }
                        else
                        {
                            npos = content.IndexOf(">", pos) + 1;
                            sb.Append(content.Substring(pos, npos - pos));
                        }
                        if (npos <= pos)
                            npos = pos + 1;
                        pos = npos;
                    }
                    else
                    {
                        if (size < length)
                        {
                            sb.Append(cur);
                            size++;
                        }
                        else
                        {
                            if (!firststop)
                            {
                                sb.Append("……");
                                firststop = true;
                            }
                        }
                        pos++;
                    }
                }
                return sb.ToString();
            }
        }

    }
}
