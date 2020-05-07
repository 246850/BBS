using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BBS.Spider.Core
{
    public sealed class HtmlAdapter
    {
        public static string LoadHtml(string url, Encoding encoding =null)
        {
            using (WebClient web = new WebClient())
            {
                if (encoding == null) encoding = Encoding.UTF8;
                web.Encoding = encoding;
                string html = web.DownloadString(url);
                if (string.IsNullOrWhiteSpace(html))
                {
                    throw new Exception("加载网页异常：" + url);
                }
                return html;
            }
        }
        public static HtmlDocument LoadDocument(string url, Encoding encoding = null)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(LoadHtml(url, encoding));
            return document;
        }
    }
}
