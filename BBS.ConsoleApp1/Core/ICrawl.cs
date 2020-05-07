using System;
using System.Collections.Generic;
using System.Text;

namespace BBS.Spider.Core
{
    public interface ICrawl
    {
        List<CrawlItem> Execute();
    }
}
