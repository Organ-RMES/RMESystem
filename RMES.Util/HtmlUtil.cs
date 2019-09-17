using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        /// 提取摘要，
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="stripHtml">是否清除HTML代码 </param>
        /// <returns></returns>
        public static string GetContentSummary(string content, int length, bool stripHtml = true)
        {
            if (string.IsNullOrEmpty(content) || length == 0)
            {
                return "";
            }

            if (stripHtml)
            {
                var re = new Regex("<[^>]*>");
                content = re.Replace(content, "");
                content = content.Replace("　", "").Replace(" ", "");

                return content.Length <= length ? content : content.Substring(0, length) + "……";
            }

            if (content.Length <= length)
            {
                return content;
            }

            var pos = 0;
            var size = 0;
            bool firststop = false, notr = false, noli = false;
            var sb = new StringBuilder();
            while (true)
            {
                if (pos >= content.Length)
                {
                    break;
                }

                var cur = content.Substring(pos, 1);
                if (cur == "<")
                {
                    var next = content.Substring(pos + 1, 3).ToLower();
                    var nPos = 0;
                    if (next.IndexOf("p", StringComparison.Ordinal) == 0 && next.IndexOf("pre", StringComparison.Ordinal) != 0)
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                    }
                    else if (next.IndexOf("/p", StringComparison.Ordinal) == 0 && next.IndexOf("/pr", StringComparison.Ordinal) != 0)
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                        if (size < length) sb.Append("<br/>");
                    }
                    else if (next.IndexOf("br", StringComparison.Ordinal) == 0)
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                        if (size < length)
                            sb.Append("<br/>");
                    }
                    else if (next.IndexOf("img", StringComparison.Ordinal) == 0)
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, nPos - pos));
                            size += nPos - pos + 1;
                        }
                    }
                    else if (next.IndexOf("li", StringComparison.Ordinal) == 0 || next.IndexOf("/li", StringComparison.Ordinal) == 0)
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, nPos - pos));
                        }
                        else
                        {
                            if (!noli && next.IndexOf("/li", StringComparison.Ordinal) == 0)
                            {
                                sb.Append(content.Substring(pos, nPos - pos));
                                noli = true;
                            }
                        }
                    }
                    else if (next.IndexOf("tr", StringComparison.Ordinal) == 0 || next.IndexOf("/tr", StringComparison.Ordinal) == 0)
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, nPos - pos));
                        }
                        else
                        {
                            if (!notr && next.IndexOf("/tr", StringComparison.Ordinal) == 0)
                            {
                                sb.Append(content.Substring(pos, nPos - pos)); notr = true;
                            }
                        }
                    }
                    else if (next.IndexOf("td", StringComparison.Ordinal) == 0 || next.IndexOf("/td", StringComparison.Ordinal) == 0)
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                        if (size < length)
                        {
                            sb.Append(content.Substring(pos, nPos - pos));
                        }
                        else
                        {
                            if (!notr)
                            {
                                sb.Append(content.Substring(pos, nPos - pos));
                            }
                        }
                    }
                    else
                    {
                        nPos = content.IndexOf(">", pos, StringComparison.Ordinal) + 1;
                        sb.Append(content.Substring(pos, nPos - pos));
                    }

                    if (nPos <= pos)
                    {
                        nPos = pos + 1;
                    }

                    pos = nPos;
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
