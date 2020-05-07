using BBS.Spider.Core;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace BBS.Spider.Crawls
{
    public class xiaohua_zol_com_cn : ICrawl
    {
        private static readonly HashSet<string> caches = new HashSet<string>();
        public List<CrawlItem> Execute()
        {
            if (caches.Count > 100) caches.Clear();

            Encoding encoding = Encoding.GetEncoding("GBK");
            HtmlDocument document = HtmlAdapter.LoadDocument("http://xiaohua.zol.com.cn/new/1.html", encoding);
            var nodes = document.DocumentNode.SelectNodes(".//li[@class='article-summary']");
            if (nodes == null) return null;

            List<CrawlItem> list = new List<CrawlItem>();
            foreach (var item in nodes)
            {
                var aNode = item.SelectSingleNode("span[2]/a");
                string href = aNode.GetAttributeValue("href", string.Empty);

                if (!string.IsNullOrWhiteSpace(href) && !caches.Contains(href))
                {
                    caches.Add(href);

                    HtmlDocument document1 = HtmlAdapter.LoadDocument("http://xiaohua.zol.com.cn" + href, encoding);
                    var titleNode = document1.DocumentNode.SelectSingleNode(".//h1[@class='article-title']");
                    var contentNode = document1.DocumentNode.SelectSingleNode(".//div[@class='article-text']");
                    if(titleNode !=null && contentNode != null)
                    {
                        var model = new CrawlItem
                        {
                            Title = titleNode.InnerText,
                            Contents = contentNode.InnerHtml,
                            CatalogId = 5,
                            AccountId = 1,
                        };
                        if(model.Title.Length< 8)
                        {
                            model.Tags = new List<string>() { model.Title };
                        }
                        list.Add(model);
                    }
                }

                Thread.Sleep(5000); // 停留5秒
            }
            return list;
        }
    }
}
