using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolGood.Words;

namespace RMES.Util
{
    public static class IllegalWordsVerify
    {
        private const string keywordsPath = "Illegal/IllegalKeywords.txt";
        private const string urlsPath = "Illegal/IllegalUrls.txt";
        private const string infoPath = "Illegal/IllegalInfo.txt";
        private const string bitPath = "Illegal/IllegalBit.iws";

        #region GetIllegalWordsSearch

        private static IllegalWordsSearch _search;

        private static IllegalWordsSearch GetIllegalWordsSearch()
        {
            if (_search == null)
            {
                var ipath = Path.GetFullPath(infoPath);
                if (File.Exists(ipath) == false)
                {
                    _search = CreateIllegalWordsSearch();
                }
                else
                {
                    var texts = File.ReadAllText(ipath).Split('|');
                    if (new FileInfo(Path.GetFullPath(keywordsPath)).LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") !=
                        texts[0] ||
                        new FileInfo(Path.GetFullPath(urlsPath)).LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") !=
                        texts[1]
                    )
                    {
                        _search = CreateIllegalWordsSearch();
                    }
                    else
                    {
                        var s = new IllegalWordsSearch();
                        s.Load(Path.GetFullPath(bitPath));
                        _search = s;
                    }
                }
            }
            return _search;
        }

        private static IllegalWordsSearch CreateIllegalWordsSearch()
        {
            var words1 = File.ReadAllLines(Path.GetFullPath(keywordsPath), Encoding.UTF8);
            var words2 = File.ReadAllLines(Path.GetFullPath(urlsPath), Encoding.UTF8);
            var words = new List<string>();
            foreach (var item in words1)
            {
                words.Add(item.Trim());
            }
            foreach (var item in words2)
            {
                words.Add(item.Trim());
            }

            var search = new IllegalWordsSearch();
            search.SetKeywords(words);

            search.Save(Path.GetFullPath(bitPath));

            var text = new FileInfo(Path.GetFullPath(keywordsPath)).LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") + "|"
                       + new FileInfo(Path.GetFullPath(urlsPath)).LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
            File.WriteAllText(Path.GetFullPath(infoPath), text);

            return search;
        }

        #endregion

        public static List<IllegalWordsSearchResult> FindAll(string text)
        {
            var search = GetIllegalWordsSearch();
            return search.FindAll(text);
        }

        public static IllegalWordsSearchResult FindFirst(string text)
        {
            var search = GetIllegalWordsSearch();
            return search.FindFirst(text);
        }

        public static bool ContainsAny(string text)
        {
            var search = GetIllegalWordsSearch();
            return search.ContainsAny(text);
        }

        public static string Replace(string text, char replaceChar = '*')
        {
            var search = GetIllegalWordsSearch();
            return search.Replace(text, replaceChar);
        }

    }
}
