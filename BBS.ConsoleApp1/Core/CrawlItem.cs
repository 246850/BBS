using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Spider.Core
{
    public class CrawlItem
    {
        public string Title { get; set; }
        public int AccountId { get; set; }
        public int CatalogId { get; set; }
        public string Contents { get; set; }
        public List<string> Tags { get; set; }
    }
}
